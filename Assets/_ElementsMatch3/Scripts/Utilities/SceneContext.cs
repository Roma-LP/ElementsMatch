using _ElementsMatch3.Scripts.Grid;
using _ElementsMatch3.Scripts.Levels;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Utilities
{
    public class SceneContext : Singleton<SceneContext>
    {
        [SerializeField] private GridBuilder _gridBuilder;
        
        private LevelLoader _levelLoader;
        private DebugLogger _debugLogger;

        public DebugLogger DebugLogger => _debugLogger;
        
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
            
            LevelData levelData =  _levelLoader.LoadLevel("level_01");
            _gridBuilder.GenerateGrid(levelData);
        }
    }
}
