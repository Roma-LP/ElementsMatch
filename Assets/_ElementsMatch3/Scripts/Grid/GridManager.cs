using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Configs;
using _ElementsMatch3.Scripts.Levels;
using DG.Tweening;
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
        private float _duration = 2f;
        private Sequence _currentSwapSequence;

        public void Init()
        {
            _gridBuilder = new GridBuilder(_gridRoot, _blockConfigs);
        }

        public void TryMoveBlock(Vector2Int from, Vector2Int direction)
        {
            Vector2Int to = from + direction;

            if (!IsInside(to)) return;

            GridCellData fromCell = _grid[from.x, from.y];
            GridCellData toCell = _grid[to.x, to.y];

            if (direction == Vector2Int.up && toCell.IsEmptyCell) return;

            SwapBlocks(fromCell, toCell);
        }

        private void SwapBlocks(GridCellData aCell, GridCellData bCell)
        {
            if (_currentSwapSequence?.IsActive() == true && _currentSwapSequence.IsPlaying())
                return;
            
            MatchBlock aBlock = aCell.MatchBlockInCell;
            MatchBlock bBlock = bCell.MatchBlockInCell;

            _currentSwapSequence = DOTween.Sequence();

            _currentSwapSequence
                .Join(aBlock.transform.DOLocalMove(bCell.LocalPosition, _duration));

            if (bBlock != null)
            {
                _currentSwapSequence
                    .Join(bBlock.transform.DOLocalMove(aCell.LocalPosition, _duration));
            }

            _currentSwapSequence.Play().SetLink(aBlock.gameObject).OnComplete(()=>
            {
                bCell.UpdateCell(aBlock);
                aCell.UpdateCell(bBlock);

                _currentSwapSequence = null;
            });
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