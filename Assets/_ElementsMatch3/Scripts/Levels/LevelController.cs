using System;
using _ElementsMatch3.Scripts.GameSaves;
using _ElementsMatch3.Scripts.Grid;
using _ElementsMatch3.Scripts.UI;

namespace _ElementsMatch3.Scripts.Levels
{
    public class LevelController : IDisposable
    {
        private readonly LevelLoader _levelLoader;
        private readonly GridManager _gridManager;
        private readonly UIButtonsLinks _uiButtonsLinks;
        private readonly GameSaveContainer _gameSaveContainer;
        
        public LevelController(LevelLoader levelLoader, GridManager gridManager, UIButtonsLinks uiButtonsLinks, GameSaveContainer gameSaveContainer)
        {
            _levelLoader = levelLoader;
            _gridManager = gridManager;
            _uiButtonsLinks = uiButtonsLinks;
            _gameSaveContainer = gameSaveContainer;
            
            _uiButtonsLinks.OnRestartPressed += RestartButtonHandler;
            _uiButtonsLinks.OnNextLevelPressed += NextLevelHandler;
            _gridManager.OnGridEmpty += NextLevelHandler;
        }

        public void StartLevel()
        {
            if (_gameSaveContainer.GameSessionData.TryGetLevelData(out LevelData levelData))
            {
                StartGameByLevelData(levelData);
            }
            else
            {
                LevelData firstLevelData = _levelLoader.GetFirstLevel();
                _gameSaveContainer.GameSessionData.SetLevel(firstLevelData.LevelNumber);
                _gameSaveContainer.GameSessionData.CreateNewGridSize(firstLevelData.Width,firstLevelData.Height);
                StartGameByLevelData(_levelLoader.GetFirstLevel());
            }
        }

        private void StartGameByLevelData(LevelData levelData)
        {
            _gridManager.ReGenerateGrid(levelData);
            _gridManager.ReFillGrid(levelData);
        }

        private void RestartButtonHandler()
        {
            _gridManager.ReFillGrid(_levelLoader.GetLevelByNumber(_levelLoader.CurrentLevelNumber));
        }

        private void NextLevelHandler()
        {
            LevelData levelData = _levelLoader.GetNextLevel();
            _gameSaveContainer.GameSessionData.SetLevel(levelData.LevelNumber);
            _gameSaveContainer.GameSessionData.CreateNewGridSize(levelData.Width,levelData.Height);
            StartGameByLevelData(levelData);
        }

        public void Dispose()
        {
            _uiButtonsLinks.OnRestartPressed -= RestartButtonHandler;
            _uiButtonsLinks.OnNextLevelPressed -= NextLevelHandler;
            _gridManager.OnGridEmpty -= NextLevelHandler;
        }
    }
}
