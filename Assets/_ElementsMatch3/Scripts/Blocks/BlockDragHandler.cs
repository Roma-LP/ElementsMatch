using _ElementsMatch3.Scripts.Grid;
using _ElementsMatch3.Scripts.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _ElementsMatch3.Scripts.Blocks
{
    public class BlockDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private MatchBlock _matchBlock;

        private Vector2 _startDrag;
        private float _swipeLenght = 30f;
        private GridManager _gridManager;

        private void TryCallMoveBlock(Vector2Int direction)
        {
            if (_gridManager == null)
                _gridManager = SceneContext.Instance.GridManager;
            
            _gridManager.TryMoveBlock(_matchBlock.GridPosition, direction);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startDrag = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector2 endDrag = eventData.position;
            Vector2 delta = endDrag - _startDrag;

            if (delta.magnitude < _swipeLenght) return;

            Vector2Int direction;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                direction = delta.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                direction = delta.y > 0 ? Vector2Int.up : Vector2Int.down;

            TryCallMoveBlock(direction);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log($"IDragHandler");
        }
    }
}