using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Configs;
using _ElementsMatch3.Scripts.Levels;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Grid
{
    public class GridBuilder : MonoBehaviour
    {
        [SerializeField] private Transform _gridRoot;
        [SerializeField] private BlockConfigContainer _blockConfigs;
        [SerializeField] private Vector2 _cellSize = new Vector2(1, 1);
        
        public void GenerateGrid(LevelData level)
        {
            Vector2 offset = new Vector2(
                -((level.width - 1) * _cellSize.x) / 2f,
                0
            );
            
            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    BlockType type = level.grid[level.height - 1 - y][x];
                    if (type == BlockType.None) continue;

                    MatchBlock matchBlockPrefab = _blockConfigs.GetBloockByType(type);
                    
                    MatchBlock matchBlockInit = Instantiate(matchBlockPrefab, _gridRoot);
                    matchBlockInit.transform.localPosition = new Vector3(x * _cellSize.x, y * _cellSize.y, 0) + (Vector3)offset;
                    matchBlockInit.SetConfig();
                }
            }
        }
    }
}