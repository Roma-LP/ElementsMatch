using _ElementsMatch3.Scripts.Blocks;

namespace _ElementsMatch3.Scripts.Grid
{
    public interface IGridReader
    {
        int Width { get; }
        int Height { get; }
        BlockType[][] Grid { get; }
    }
}