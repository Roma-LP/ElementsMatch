using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Grid;

namespace _ElementsMatch3.Scripts.Levels
{
    [System.Serializable]
    public class LevelData
    {
        public int LevelNumber;
        public int Width;
        public int Height;
        public BlockType[,] Grid;

        private int GetFlippedY(int yFromBottom)
        {
            return Height - 1 - yFromBottom;
        }
        
        public BlockType GetBlockAtGridPosition(int x, int yFromBottom)
        {
            return Grid[GetFlippedY(yFromBottom), x];
        }
        
        public void SetBlockAtGridPosition(int x, int yFromBottom, BlockType type)
        {
            Grid[GetFlippedY(yFromBottom), x] = type;
        }
    }
}