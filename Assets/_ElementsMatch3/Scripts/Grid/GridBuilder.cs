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

        private GridCellData[,] _gridCellData;

        public GridBuilder(Transform gridRoot, BlockConfigContainer blockConfigs)
        {
            _gridRoot = gridRoot;
            _blockConfigs = blockConfigs;
        }

        private void ClearGridCell()
        {
            if (_gridCellData == null) return;
            
            foreach (GridCellData cell in _gridCellData)
            {
                if(cell.IsEmptyCell) continue;
                
                Object.Destroy(cell.MatchBlockInCell.gameObject);
            }
        }

        public GridCellData[,] GenerateGrid(LevelData level)
        {
            ClearGridCell();
            
            Vector2 offset = new Vector2(
                -((level.width - 1) * _cellSize.x) / 2f,
                0
            );

            _gridCellData = new GridCellData[level.width, level.height];

            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    Vector3 cellLocalTransformPositionInGrid = new Vector3(x * _cellSize.x, y * _cellSize.y, 0) + (Vector3)offset;
                    Vector2Int cellPositionInGrid = new Vector2Int(x, y);
                    
                    _gridCellData[x, y] = new GridCellData(null, cellPositionInGrid, cellLocalTransformPositionInGrid);
                }
            }

            return _gridCellData;
        }

        public void FillGrid(LevelData level)
        {
            ClearGridCell();
            
            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    BlockType type = level.grid[level.height - 1 - y][x];
                    if (type == BlockType.None)
                    {
                        _gridCellData[x, y].UpdateCell(null);
                        continue;
                    }

                    MatchBlock matchBlockPrefab = _blockConfigs.GetBloockByType(type);
                    MatchBlock matchBlockInit = Object.Instantiate(matchBlockPrefab, _gridRoot);
                    matchBlockInit.SetConfig(_gridCellData[x, y].GridPosition, type);
                    
                    _gridCellData[x, y].UpdateCell(matchBlockInit);
                }
            }
        }
    }
}