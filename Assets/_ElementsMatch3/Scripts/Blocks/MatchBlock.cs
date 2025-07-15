using _ElementsMatch3.Scripts.Grid;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Blocks
{
    public class MatchBlock : MonoBehaviour
    {
        [SerializeField] private BlockAnimationController _animationController;

        private BlockDragHandler _blockDragHandler;

        //public event Action<Vector2Int, Vector2Int> BlockDragEnded;

        public BlockType BlockType { get; private set; }
        public Vector2Int GridPosition { get; private set; }

        public void SetConfig(Vector2Int gridPosition, BlockType blockType)
        {
            _animationController.SetTriggerIdle();

            BlockType = blockType;
            GridPosition = gridPosition;
        }

        public void UpdatePosition(Vector2Int pos, GridManager manager)
        {
            GridPosition = pos;
        }
    }
}