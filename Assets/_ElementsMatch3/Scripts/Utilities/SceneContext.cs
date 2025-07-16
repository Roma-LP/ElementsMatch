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
        
        private LevelLoader _levelLoader;
        private DebugLogger _debugLogger;
        private LevelController _levelController;

        public DebugLogger DebugLogger => _debugLogger;
        public GridManager GridManager => _gridManager;
        public UIButtonsLinks UIButtonsLinks => _uiButtonsLinks;
        
        protected override void Awake()
        {
            base.Awake();
            
            Bootstrapper();
        }
        
        private void Bootstrapper()
        {
            Application.targetFrameRate = 60;

            _debugLogger = new DebugLogger();
            _levelLoader = new LevelLoader();
            
            _gridManager.Init();
            _uiButtonsLinks.Init();
            _uiTextLevel.Init(_levelLoader);
            
            _levelController = new LevelController(_levelLoader, _gridManager, _uiButtonsLinks);
            
            _levelController.StartLevel();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _levelController.Dispose();
        }
    }
}
