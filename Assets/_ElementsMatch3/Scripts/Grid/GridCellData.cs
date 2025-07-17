using System;
using _ElementsMatch3.Scripts.Blocks;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Grid
{
    [Serializable]
    public class GridCellData
    {
        private MatchBlock _matchBlock;
        private Vector2Int _gridPosition;
        private Vector3 _localPosition;

        public MatchBlock MatchBlockInCell => _matchBlock;
        public Vector2Int GridPosition => _gridPosition;
        public Vector3 LocalPosition => _localPosition;
        public bool IsEmptyCell => _matchBlock == null;

        public GridCellData(MatchBlock matchBlock, Vector2Int gridPosition, Vector3 localPosition)
        {
            _matchBlock = matchBlock;
            _gridPosition = gridPosition;
            _localPosition = localPosition;
        }

        public void UpdateCell(MatchBlock matchBlock, bool isUpdateVisualPosition = false)
        {
            _matchBlock = matchBlock;
            
            if (_matchBlock == null)
                return;
            
            if(isUpdateVisualPosition)
                    _matchBlock.transform.localPosition = _localPosition;
            
            _matchBlock.UpdatePosition(_gridPosition);
        }
    }
}