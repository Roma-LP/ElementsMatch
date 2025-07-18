using System;
using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Levels;
using UnityEngine;

namespace _ElementsMatch3.Scripts.GameSaves
{
    public class GameSessionData : ProgressData<LevelData>
    {
        private LevelData _gameSessionLevelData;

        private void CheckIsGameSessionSubDataNull()
        {
            _gameSessionLevelData ??= new LevelData();
        }
        
        public void SetLevel(int level)
        {
            CheckIsGameSessionSubDataNull();

            _gameSessionLevelData.LevelNumber = level;
        }

        public void SetGrid(Vector2Int gridPosition, BlockType blockType)
        {
            _gameSessionLevelData.SetBlockAtGridPosition(gridPosition.x, gridPosition.y, blockType);
        }

        public void CreateNewGridSize(int width, int height)
        {
            CheckIsGameSessionSubDataNull();

            _gameSessionLevelData.Grid = new BlockType[height,width];
            _gameSessionLevelData.Width = width;
            _gameSessionLevelData.Height = height;
        }
        
        public bool TryGetLevelData(out LevelData levelData)
        {
            levelData = default;
            
            if (_gameSessionLevelData == null)
                return false;
            
            levelData = _gameSessionLevelData;
            return true;
        } 
        
        public override LevelData GetProgressModel()
        {
            return _gameSessionLevelData;
        }

        public override void SetProgressModel(LevelData state)
        {
            _gameSessionLevelData = state;
        }
    }
}