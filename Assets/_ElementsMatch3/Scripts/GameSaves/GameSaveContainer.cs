using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _ElementsMatch3.Scripts.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace _ElementsMatch3.Scripts.GameSaves
{
    public class GameSaveContainer
    {
        private const string SAVE_FILE_NAME = "save.json";
        private readonly DebugLogger _debugLogger;

        private Dictionary<Type, IProgressData> _components = new();
        private JsonSerializerSettings _jsonSerializerSettings;

        private static string SavePath =>
#if UNITY_EDITOR
            Path.Combine(Application.dataPath, SAVE_FILE_NAME);
#else
            Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
#endif

        public GameSessionData GameSessionData { private set; get; }


        public GameSaveContainer(DebugLogger debugLogger)
        {
            _debugLogger = debugLogger;

            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None,
                Formatting = Formatting.Indented
            };

            ReinitializeData();
            LoadGame();
        }

        private void ReinitializeData()
        {
            GameSessionData = new GameSessionData();

            Register(GameSessionData);
        }

        private void Register<T>(ProgressData<T> component) where T : class, new()
        {
            _components[typeof(T)] = component;
        }

        private void RemoveNullValues()
        {
            List<Type> keysToRemove = _components
                .Where(pair => pair.Value.GetProgressModel() == null)
                .Select(pair => pair.Key)
                .ToList();

            foreach (Type key in keysToRemove)
            {
                _components.Remove(key);
            }
        }

        private void SaveGame()
        {
            Dictionary<string, object> saveData = new Dictionary<string, object>();

            foreach (var pair in _components)
            {
                object data = pair.Value.GetProgressModel();
                saveData[pair.Key.AssemblyQualifiedName] = data;
            }

            string json = JsonConvert.SerializeObject(saveData, _jsonSerializerSettings);
            File.WriteAllText(SavePath, json);
            _debugLogger.PrintLog(nameof(GameSaveContainer), $"Game saved to: {SavePath}");
        }

        private void LoadGame()
        {
            if (!File.Exists(SavePath))
            {
                _debugLogger.PrintLog(nameof(GameSaveContainer), $"No save file found. New Game");
                return;
            }

            string json = File.ReadAllText(SavePath);
            Dictionary<string, JObject> rawData =
                JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json, _jsonSerializerSettings);

            foreach (var pair in _components)
            {
                if (rawData.TryGetValue(pair.Key.AssemblyQualifiedName, out JObject jObj))
                {
                    var data = jObj.ToObject(pair.Key);
                    pair.Value.SetProgressModel(data);
                }
            }

            _debugLogger.PrintLog(nameof(GameSaveContainer), $"Game loaded.");
        }

        public void ClearSaveFile()
        {
            if (File.Exists(SavePath))
            {
                string metaPath = SavePath + ".meta";
                File.Delete(SavePath);
                File.Delete(metaPath);
            }
        }

        public void Dispose()
        {
            RemoveNullValues();
            SaveGame();
        }
    }
}