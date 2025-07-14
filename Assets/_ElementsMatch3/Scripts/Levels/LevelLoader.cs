using _ElementsMatch3.Scripts.Utilities;
using UnityEngine;
using Newtonsoft.Json;

namespace _ElementsMatch3.Scripts.Levels
{
    public class LevelLoader
    {
        public LevelData LoadLevel(string levelName)
        {
            TextAsset json = Resources.Load<TextAsset>($"Levels/{levelName}");
            if (json == null)
            {
                SceneContext.Instance.DebugLogger.PrintException(nameof(LevelLoader),$"Level file '{levelName}' not found in Resources/Levels/");
                return null;
            }
            
            LevelData level = JsonConvert.DeserializeObject<LevelData>(json.text);
            return level;
        }
    }
}