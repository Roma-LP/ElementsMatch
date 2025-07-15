using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Configs;
using _ElementsMatch3.Scripts.Levels;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Grid
{
    public class GridBuilder
    {
        private readonly Transform _gridRoot;
        private readonly BlockConfigContainer _blockConfigs;
        private readonly Vector2 _cellSize = new Vector2(1, 1);
        
        private MatchBlock[,] _grid;
        
        public GridBuilder(Transform gridRoot, BlockConfigContainer blockConfigs)
        {
            _gridRoot = gridRoot;
            _blockConfigs = blockConfigs;
        }

        public MatchBlock[,] GenerateGrid(LevelData level)
        {
            Vector2 offset = new Vector2(
                -((level.width - 1) * _cellSize.x) / 2f,
                0
            );

            _grid = new MatchBlock[level.width, level.height];
            
            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    BlockType type = level.grid[level.height - 1 - y][x];
                    if (type == BlockType.None) continue;

                    MatchBlock matchBlockPrefab = _blockConfigs.GetBloockByType(type);
                    
                    MatchBlock matchBlockInit = Object.Instantiate(matchBlockPrefab, _gridRoot);
                    matchBlockInit.transform.localPosition = new Vector3(x * _cellSize.x, y * _cellSize.y, 0) + (Vector3)offset;
                    matchBlockInit.SetConfig(new Vector2Int(x, y), type);
                    _grid[x,y] = matchBlockInit;
                }
            }

            return _grid;
        }
    }
}