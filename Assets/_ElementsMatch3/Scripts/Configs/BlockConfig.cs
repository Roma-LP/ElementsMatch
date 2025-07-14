using _ElementsMatch3.Scripts.Blocks;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/BlockConfig")]
    public class BlockConfig : ScriptableObject
    {
        [field:SerializeField] public BlockType BlockType { get; private set; }
        [field:SerializeField] public MatchBlock MatchBlock { get; private set; }
    }
}