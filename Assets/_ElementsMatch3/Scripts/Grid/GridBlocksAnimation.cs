using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _ElementsMatch3.Scripts.Blocks;
using _ElementsMatch3.Scripts.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Grid
{
    public class GridBlocksAnimation : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 5f)] private float _moveDuration = 0.3f;
        [SerializeField] private Ease _moveEase = Ease.Linear;
        [SerializeField, Range(100, 2000)] private int _destroyDuration = 1000;

        private HashSet<Tweener> _activeAnimations = new HashSet<Tweener>();

        public void AnimateMove(MatchBlock block, Vector3 targetPosition, Action onComplete = null)
        {
            if (block == null)
            {
                SceneContext.Instance.DebugLogger.PrintLog(nameof(GridBlocksAnimation), ("Block is null."));
                return;
            }

            Tweener tween = null;
            tween = block.transform
                .DOLocalMove(targetPosition, _moveDuration)
                .SetEase(_moveEase)
                .SetLink(block.gameObject)
                .OnComplete(() => { onComplete?.Invoke(); })
                .OnKill(() => { _activeAnimations.Remove(tween); });

            _activeAnimations.Add(tween);
        }

        public async UniTask AnimateMoveAsync(MatchBlock block, Vector3 targetPosition)
        {
            if (block == null) return;

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Tweener tween = null;
            tween = block.transform
                .DOLocalMove(targetPosition, _moveDuration)
                .SetEase(_moveEase)
                .SetLink(block.gameObject, LinkBehaviour.KillOnDestroy)
                .OnComplete(() =>
                {
                    _activeAnimations.Remove(tween);
                    tcs.TrySetResult(true);
                })
                .OnKill(() =>
                {
                    _activeAnimations.Remove(tween);
                    tcs.TrySetResult(true);
                });

            _activeAnimations.Add(tween);
            await tcs.Task;
        }

        public async UniTask AnimateDestroyAsync(MatchBlock block, Action onComplete = null)
        {
            block.PlayDestroyAnimation();
            await UniTask.Delay(_destroyDuration);
            onComplete?.Invoke();
        }

        public void AnimateBatchMove(List<(MatchBlock block, Vector3 target)> moves, Action onComplete = null)
        {
            int completedCount = 0;

            foreach ((MatchBlock block, Vector3 target) in moves)
            {
                AnimateMove(block, target, () =>
                {
                    completedCount++;
                    if (completedCount >= moves.Count)
                    {
                        onComplete?.Invoke();
                    }
                });
            }
        }
    }
}