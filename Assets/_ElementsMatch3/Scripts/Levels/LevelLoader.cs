using System;
using System.Collections.Generic;
using System.Linq;
using _ElementsMatch3.Scripts.Utilities;
using UnityEngine;
using Newtonsoft.Json;

namespace _ElementsMatch3.Scripts.Levels
{
    public class LevelLoader
    {
        private const string LEVELS_FOLDER_PATH = "Levels";

        private Dictionary<int, LevelData> _levelsByNumber = new();
        private int[] _sortedLevelNumbers;
        private int _currentLevelNumber = -1;

        public int CurrentLevelNumber
        {
            get => _currentLevelNumber;
            private set
            {
                if (_currentLevelNumber != value)
                {
                    _currentLevelNumber = value;
                    OnСurrentLevelChanged?.Invoke(_currentLevelNumber);
                }
            }
        }

        public event Action<int> OnСurrentLevelChanged;

        public LevelLoader()
        {
            LoadAllLevels();
        }

        private void LoadAllLevels()
        {
            TextAsset[] levelFiles = Resources.LoadAll<TextAsset>(LEVELS_FOLDER_PATH);

            if (levelFiles == null || levelFiles.Length == 0)
            {
                SceneContext.Instance.DebugLogger.PrintException(nameof(LevelLoader),
                    $"No level files found in Resources/{LEVELS_FOLDER_PATH}/");
                return;
            }

            foreach (TextAsset file in levelFiles)
            {
                try
                {
                    LevelData level = JsonConvert.DeserializeObject<LevelData>(file.text);

                    if (_levelsByNumber.ContainsKey(level.levelNumber))
                    {
                        SceneContext.Instance.DebugLogger.PrintException(nameof(LevelLoader),
                            $"Duplicate level number '{level.levelNumber}' in file '{file.name}'");
                        continue;
                    }

                    _levelsByNumber[level.levelNumber] = level;
                }
                catch (System.Exception ex)
                {
                    SceneContext.Instance.DebugLogger.PrintException(nameof(LevelLoader),
                        $"Failed to load level from '{file.name}': {ex.Message}");
                }
            }

            _sortedLevelNumbers = _levelsByNumber.Keys.OrderBy(key => key).ToArray();
        }

        public LevelData GetLevelByNumber(int levelNumber)
        {
            if (_levelsByNumber.TryGetValue(levelNumber, out var level))
            {
                CurrentLevelNumber = levelNumber;
                return level;
            }

            SceneContext.Instance.DebugLogger.PrintException(nameof(LevelLoader),
                $"Level number '{levelNumber}' not found.");
            return null;
        }

        public LevelData GetNextLevel()
        {
            int currentIndex = System.Array.IndexOf(_sortedLevelNumbers, CurrentLevelNumber);

            if (currentIndex == -1 || _sortedLevelNumbers.Length == 0)
            {
                SceneContext.Instance.DebugLogger.PrintException(nameof(LevelLoader), "Current level is not valid or no levels available.");
                return null;
            }

            if (++currentIndex >= _sortedLevelNumbers.Length)
            {
                currentIndex = 0;
            }

            int nextLevelNumber = _sortedLevelNumbers[currentIndex];
            return GetLevelByNumber(nextLevelNumber);
        }

        public int[] GetAvailableLevelNumbers() => _sortedLevelNumbers;

        public LevelData GetFirstLevel()
        {
            return _sortedLevelNumbers.Length > 0 ? GetLevelByNumber(_sortedLevelNumbers[0]) : null;
        }
    }
}