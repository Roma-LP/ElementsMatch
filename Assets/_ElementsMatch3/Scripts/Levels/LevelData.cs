using _ElementsMatch3.Scripts.Blocks;

namespace _ElementsMatch3.Scripts.Levels
{
    [System.Serializable]
    public class LevelData
    {
        public int levelNumber;
        public int width;
        public int height;
        public BlockType[][] grid;
    }
}