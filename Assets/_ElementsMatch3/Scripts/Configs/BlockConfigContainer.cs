using System.Collections.Generic;
using System.Linq;
using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Utilities;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/BlockConfigContainer")]
    public class BlockConfigContainer: ScriptableObject
    {
        [SerializeField] private List<BlockConfig> _blockConfigs;

        public MatchBlock GetBloockByType(BlockType blockType)
        {
            MatchBlock matchBlock = _blockConfigs.FirstOrDefault(block => block.BlockType == blockType)?.MatchBlock;
            
            if (matchBlock == null)
                SceneContext.Instance.DebugLogger.PrintException(nameof(BlockConfigContainer),$"MatchBlock for {blockType} not found!");

            return matchBlock;
        }
    }
}