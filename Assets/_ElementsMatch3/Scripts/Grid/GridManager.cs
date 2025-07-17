using System.Collections.Generic;
using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Configs;
using _ElementsMatch3.Scripts.Levels;
using _ElementsMatch3.Scripts.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Transform _gridRoot;
        [SerializeField] private BlockConfigContainer _blockConfigs;

        private GridBuilder _gridBuilder;
        private GridCellData[,] _grid;
        private int _width;
        private int _height;
        private GridBlocksAnimation _gridBlocksAnimation;

        public void Init()
        {
            _gridBuilder = new GridBuilder(_gridRoot, _blockConfigs);

            _gridBlocksAnimation = SceneContext.Instance.GridBlocksAnimation;
        }

        public async UniTask TryMoveBlock(Vector2Int from, Vector2Int direction)
        {
            if (_gridBlocksAnimation.IsPlaying)
                return;

            Vector2Int to = from + direction;

            if (!IsInside(to)) return;

            GridCellData fromCell = _grid[from.x, from.y];
            GridCellData toCell = _grid[to.x, to.y];

            if (direction == Vector2Int.up && toCell.IsEmptyCell) return;
            
            await SwapBlocks(fromCell, toCell);
            
            await NormalizeFallingBlock();
            
            await NormalizeSecondPhase();
            
            Debug.LogError("ok");
        }

        private async UniTask SwapBlocks(GridCellData aCell, GridCellData bCell)
        {
            MatchBlock aBlock = aCell.MatchBlockInCell;
            MatchBlock bBlock = bCell.MatchBlockInCell;
            
            UniTask aAnim = _gridBlocksAnimation.AnimateMoveAsync(aBlock, bCell.LocalPosition);
            UniTask bAnim = bBlock != null
                ? _gridBlocksAnimation.AnimateMoveAsync(bBlock, aCell.LocalPosition)
                : UniTask.CompletedTask;

            bCell.UpdateCell(aBlock);
            aCell.UpdateCell(bBlock);
            
            await UniTask.WhenAll(aAnim, bAnim);
        }
        
        private async UniTask NormalizeFallingBlock()
        {
            List<UniTask> animations = new List<UniTask>();
            
            for (int x = 0; x < _width; x++)
            {
                for (int y = 1; y < _height; y++)
                {
                    GridCellData currentCell = _grid[x, y];
                    if (currentCell.IsEmptyCell)
                        continue;

                    MatchBlock fallingBlock = currentCell.MatchBlockInCell;
                    int targetY = y;
                    
                    while (targetY - 1 >= 0 && _grid[x, targetY - 1].IsEmptyCell)
                    {
                        targetY--;
                    }

                    if (targetY != y)
                    {
                        GridCellData targetCell = _grid[x, targetY];
                        
                        targetCell.UpdateCell(fallingBlock);
                        currentCell.UpdateCell(null);

                        animations.Add(_gridBlocksAnimation.AnimateMoveAsync(fallingBlock, targetCell.LocalPosition));
                    }
                }
            }
            
            await UniTask.WhenAll(animations);
        }
        
        private async UniTask NormalizeSecondPhase()
        {
            await UniTask.Yield();
        }

        private bool IsInside(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height;
        }

        public void ReGenerateGrid(LevelData level)
        {
            _width = level.width;
            _height = level.height;
            _grid = _gridBuilder.GenerateGrid(level);
        }

        public void ReFillGrid(LevelData level)
        {
            _gridBuilder.FillGrid(level);
        }
    }
}