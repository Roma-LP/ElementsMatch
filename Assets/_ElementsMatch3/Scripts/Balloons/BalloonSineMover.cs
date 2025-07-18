using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _ElementsMatch3.Scripts.Balloons
{
    public class BalloonSineMover : MonoBehaviour
    {
        [Header("Balloons settings")]
        [SerializeField] private List<BalloonItem> _balloonPrefabs;
        [SerializeField] private int _ballCount = 3;
        [Header("Sin settings")]
        [SerializeField] private Vector2 _speedRange = new(0.5f, 2f);
        [SerializeField] private Vector2 _heightSineWave = new(0.5f, 1f);
        [SerializeField] private Vector2 _frequencyRange = new(1f, 2f);
        [SerializeField] private Vector2 _ySpawnRange = new(-1f, 3.3f);
        [Header("Screen settings")]
        [SerializeField] private float _offscreenOffset = 0.1f;

        private readonly List<BalloonItem> _activeBalloons = new();
        private Camera _mainCamera;

        public void Init()
        {
            _mainCamera = Camera.main;

            for (int i = 0; i < _ballCount; i++)
            {
                SpawnNewBalloon();
            }
        }

        private void SpawnNewBalloon()
        {
            BalloonItem prefab = _balloonPrefabs[Random.Range(0, _balloonPrefabs.Count)];
            BalloonItem balloon = Instantiate(prefab, transform);
            _activeBalloons.Add(balloon);

            LaunchBalloon(balloon);
        }

        private void LaunchBalloon(BalloonItem balloon)
        {
            bool moveLeftToRight = Random.value > 0.5f;
            float speed = Random.Range(_speedRange.x, _speedRange.y);
            float amplitude = Random.Range(_heightSineWave.x, _heightSineWave.y);
            float frequency = Random.Range(_frequencyRange.x, _frequencyRange.y);
            float yStart = Random.Range(_ySpawnRange.x, _ySpawnRange.y);
            
            Vector3 start = GetEdgePosition(yStart, moveLeftToRight);
            Vector3 end = GetEdgePosition(yStart, !moveLeftToRight);
            balloon.transform.position = start;

            float duration = Vector3.Distance(start, end) / speed;

            Tween tween = balloon.transform.DOMoveX(end.x, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    Vector3 pos = balloon.transform.position;
                    float progress = Mathf.InverseLerp(start.x, end.x, pos.x);
                    float sineY = Mathf.Sin(progress * Mathf.PI * 2 * frequency) * amplitude;
                    balloon.transform.position = new Vector3(pos.x, yStart + sineY, pos.z);
                })
                .OnComplete(() =>
                {
                    LaunchBalloon(balloon);
                });
        }

        private Vector3 GetEdgePosition(float y, bool leftToRight)
        {
            float viewportX = leftToRight ? 0f - _offscreenOffset : 1f + _offscreenOffset;
            float worldX = _mainCamera.ViewportToWorldPoint(new Vector3(viewportX, 0, 0)).x;
            return new Vector3(worldX, y, 0);
        }
    }
}