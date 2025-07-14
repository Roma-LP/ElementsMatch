using UnityEngine;

namespace _ElementsMatch3.Scripts.Blocks
{
    public class BlockAnimationController : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        private readonly int IDLE = Animator.StringToHash("Idle");
        private readonly int DESTROY = Animator.StringToHash("Destroy");

        private void SetTrigger(int hashAnimation)
        {
            _animator.SetTrigger(hashAnimation);
        }
        
        public void SetTriggerIdle()
        {
            SetTrigger(IDLE);
        }

        public void SetTriggerDestroy()
        {
            SetTrigger(DESTROY);
        }
    }
}