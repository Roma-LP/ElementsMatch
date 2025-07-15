using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Configs;
using _ElementsMatch3.Scripts.Levels;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Transform _gridRoot;
        [SerializeField] private BlockConfigContainer _blockConfigs;

        private GridBuilder _gridBuilder;
        private MatchBlock[,] _grid;
        private int _width;
        private int _height;

        public void Init(LevelData level)
        {
            _gridBuilder = new GridBuilder(_gridRoot, _blockConfigs);

            _width = level.width;
            _height = level.height;
            _grid = _gridBuilder.GenerateGrid(level);
        }

        public void TryMoveBlock(Vector2Int from, Vector2Int direction)
        {
            Vector2Int to = from + direction;

            if (!IsInside(to)) return;

            var fromBlock = _grid[from.x, from.y];
            var toBlock = _grid[to.x, to.y];

            if (direction == Vector2Int.up && toBlock == null) return;

            if (toBlock != null)
            {
                SwapBlocks(from, to);
            }
            else
            {
                _grid[to.x, to.y] = fromBlock;
                _grid[from.x, from.y] = null;

                fromBlock.transform.localPosition = GetLocalPosition(to);
                fromBlock.UpdatePosition(to, this);
            }
        }

        private void SwapBlocks(Vector2Int a, Vector2Int b)
        {
            var aBlock = _grid[a.x, a.y];
            var bBlock = _grid[b.x, b.y];

            _grid[a.x, a.y] = bBlock;
            _grid[b.x, b.y] = aBlock;

            Vector3 aPos = aBlock.transform.localPosition;
            aBlock.transform.localPosition = bBlock.transform.localPosition;
            bBlock.transform.localPosition = aPos;

            aBlock.UpdatePosition(b, this);
            bBlock.UpdatePosition(a, this);
        }

        private bool IsInside(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height;
        }

        private Vector3 GetLocalPosition(Vector2Int pos)
        {
            Vector2 gridOffset = new Vector2(-(_width - 1) / 2f, 0);
            return new Vector3(pos.x, pos.y, 0) + (Vector3)gridOffset;
        }
    }
}