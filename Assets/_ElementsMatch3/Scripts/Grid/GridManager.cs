using System.Collections.Generic;
using System.Linq;
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
        private bool _isBlockMoving;

        public void Init()
        {
            _gridBuilder = new GridBuilder(_gridRoot, _blockConfigs);

            _gridBlocksAnimation = SceneContext.Instance.GridBlocksAnimation;
        }

        public async UniTask TryMoveBlock(Vector2Int from, Vector2Int direction)
        {
            if (_isBlockMoving)
                return;

            Vector2Int to = from + direction;

            if (!IsInside(to))
                return;

            GridCellData fromCell = _grid[from.x, from.y];
            GridCellData toCell = _grid[to.x, to.y];

            if (direction == Vector2Int.up && toCell.IsEmptyCell)
                return;

            _isBlockMoving = true;
            
            await SwapBlocks(fromCell, toCell);
            
            await NormalizeFallingBlock();
            
            await NormalizeSecondPhase();
            
            _isBlockMoving = false;
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
            bool foundAnyMatches;

            do
            {
                foundAnyMatches = false;
                List<HashSet<GridCellData>> matchedAreas = FindMatchedAreas();

                List<GridCellData> cellsToDestroy = new List<GridCellData>();

                foreach (HashSet<GridCellData> area in matchedAreas)
                {
                    if (ContainsValidLine(area))
                    {
                        foundAnyMatches = true;
                        cellsToDestroy.AddRange(area);
                    }
                }

                if (!foundAnyMatches)
                    break;

                List<UniTask> destroyTasks = new List<UniTask>();
                foreach (GridCellData cell in cellsToDestroy)
                {
                    MatchBlock block = cell.MatchBlockInCell;
                    if (block != null)
                    {
                        destroyTasks.Add(_gridBlocksAnimation.AnimateDestroyAsync(block, () =>
                        {
                            Destroy(block.gameObject);
                        }));
                        cell.UpdateCell(null);
                    }
                }

                await UniTask.WhenAll(destroyTasks);

                await NormalizeFallingBlock();

            } while (foundAnyMatches);
        }
        
        private List<HashSet<GridCellData>> FindMatchedAreas()
        {
            List<HashSet<GridCellData>> areas = new List<HashSet<GridCellData>>();
            bool[,] visited = new bool[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (visited[x, y])
                        continue;

                    GridCellData start = _grid[x, y];
                    if (start.IsEmptyCell)
                        continue;

                    BlockType type = start.MatchBlockInCell.BlockType;
                    HashSet<GridCellData> area = new HashSet<GridCellData>();
                    Queue<GridCellData> queue = new Queue<GridCellData>();
                    queue.Enqueue(start);

                    while (queue.Count > 0)
                    {
                        GridCellData current = queue.Dequeue();
                        Vector2Int pos = current.GridPosition;

                        if (visited[pos.x, pos.y])
                            continue;

                        visited[pos.x, pos.y] = true;
                        area.Add(current);

                        foreach (Vector2Int offset in new Vector2Int[] {
                                     Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                        {
                            Vector2Int neighborPos = pos + offset;
                            if (!IsInside(neighborPos))
                                continue;

                            GridCellData neighbor = _grid[neighborPos.x, neighborPos.y];
                            if (visited[neighborPos.x, neighborPos.y])
                                continue;

                            if (!neighbor.IsEmptyCell && neighbor.MatchBlockInCell.BlockType == type)
                            {
                                queue.Enqueue(neighbor);
                            }
                        }
                    }

                    if (area.Count >= 3)
                        areas.Add(area);
                }
            }

            return areas;
        }
        
        private bool ContainsValidLine(HashSet<GridCellData> area)
        {
            var groupedByY = area.GroupBy(cell => cell.GridPosition.y);
            foreach (var group in groupedByY)
            {
                var ordered = group.OrderBy(cell => cell.GridPosition.x).ToList();
                int count = 1;
                for (int i = 1; i < ordered.Count; i++)
                {
                    if (ordered[i].GridPosition.x == ordered[i - 1].GridPosition.x + 1)
                    {
                        count++;
                        if (count >= 3)
                            return true;
                    }
                    else
                    {
                        count = 1;
                    }
                }
            }

            var groupedByX = area.GroupBy(cell => cell.GridPosition.x);
            foreach (var group in groupedByX)
            {
                var ordered = group.OrderBy(cell => cell.GridPosition.y).ToList();
                int count = 1;
                for (int i = 1; i < ordered.Count; i++)
                {
                    if (ordered[i].GridPosition.y == ordered[i - 1].GridPosition.y + 1)
                    {
                        count++;
                        if (count >= 3)
                            return true;
                    }
                    else
                    {
                        count = 1;
                    }
                }
            }

            return false;
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