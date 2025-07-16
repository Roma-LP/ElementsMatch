using System;
using _ElementsMatch3.Scripts.Grid;
using _ElementsMatch3.Scripts.UI;

namespace _ElementsMatch3.Scripts.Levels
{
    public class LevelController : IDisposable
    {
        private readonly LevelLoader _levelLoader;
        private readonly GridManager _gridManager;
        private readonly UIButtonsLinks _uiButtonsLinks;
        
        public LevelController(LevelLoader levelLoader, GridManager gridManager, UIButtonsLinks uiButtonsLinks)
        {
            _levelLoader = levelLoader;
            _gridManager = gridManager;
            _uiButtonsLinks = uiButtonsLinks;
            
            _uiButtonsLinks.OnRestartPressed += RestartButtonHandler;
            _uiButtonsLinks.OnNextLevelPressed += NextLevelButtonHandler;
        }

        public void StartLevel()
        {
            _gridManager.ReGenerateGrid(_levelLoader.GetFirstLevel());
            _gridManager.ReFillGrid(_levelLoader.GetFirstLevel());
        }

        private void RestartButtonHandler()
        {
            _gridManager.ReFillGrid(_levelLoader.GetLevelByNumber(_levelLoader.CurrentLevelNumber));
        }

        private void NextLevelButtonHandler()
        {
            LevelData levelData = _levelLoader.GetNextLevel();
            
            _gridManager.ReGenerateGrid(levelData);
            _gridManager.ReFillGrid(levelData);
        }

        public void Dispose()
        {
            _uiButtonsLinks.OnRestartPressed -= RestartButtonHandler;
            _uiButtonsLinks.OnNextLevelPressed -= NextLevelButtonHandler;
        }
    }
}