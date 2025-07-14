using UnityEngine;

namespace _ElementsMatch3.Scripts.Blocks
{
    public class MatchBlock : MonoBehaviour
    {
        [SerializeField] private BlockAnimationController _animationController;

        public void SetConfig()
        {
            _animationController.SetTriggerIdle();
        }
    }
}