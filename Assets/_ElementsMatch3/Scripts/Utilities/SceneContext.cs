using _ElementsMatch3.Scripts.Balloons;
using _ElementsMatch3.Scripts.GameSaves;
using _ElementsMatch3.Scripts.Grid;
using _ElementsMatch3.Scripts.Levels;
using _ElementsMatch3.Scripts.UI;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Utilities
{
    public class SceneContext : Singleton<SceneContext>
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private UIButtonsLinks _uiButtonsLinks;
        [SerializeField] private UITextLevel _uiTextLevel;
        [SerializeField] private GridBlocksAnimation _gridBlocksAnimation;
        [SerializeField] private BalloonSineMover _balloonSineMover;
        
        private LevelLoader _levelLoader;
        private DebugLogger _debugLogger;
        private LevelController _levelController;
        private GameSaveContainer _gameSaveContainer;

        public DebugLogger DebugLogger => _debugLogger;
        public GridManager GridManager => _gridManager;
        public UIButtonsLinks UIButtonsLinks => _uiButtonsLinks;
        public GridBlocksAnimation GridBlocksAnimation => _gridBlocksAnimation;
        public GameSaveContainer GameSaveContainer => _gameSaveContainer;
        
        protected override void Awake()
        {
            base.Awake();
            
            Bootstrapper();
        }
        
        private void Bootstrapper()
        {
            Application.targetFrameRate = 60;

            _debugLogger = new DebugLogger();
            _gameSaveContainer = new GameSaveContainer(_debugLogger);
            _levelLoader = new LevelLoader(_gameSaveContainer);
            
            _gridManager.Init();
            _uiButtonsLinks.Init();
            _uiTextLevel.Init(_levelLoader);
            _balloonSineMover.Init();
            
            _levelController = new LevelController(_levelLoader, _gridManager, _uiButtonsLinks, _gameSaveContainer);
            
            _levelController.StartLevel();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _levelController.Dispose();
            _gameSaveContainer.Dispose();
        }
        
        private void OnApplicationPause(bool pause)
        {
            _gameSaveContainer.Dispose();
        }

        private void OnApplicationQuit()
        {
            _gameSaveContainer.Dispose();
        }
    }
}
