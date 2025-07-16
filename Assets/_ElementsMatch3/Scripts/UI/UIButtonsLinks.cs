using System;
using UnityEngine;
using UnityEngine.UI;

namespace _ElementsMatch3.Scripts.UI
{
    public class UIButtonsLinks : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextLevelButton;
        
        public event Action OnRestartPressed;
        public event Action OnNextLevelPressed;

        public void Init()
        {
            _restartButton.onClick.AddListener(InvokeRestart);
            _nextLevelButton.onClick.AddListener(InvokeNextLevel);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(InvokeRestart);
            _nextLevelButton.onClick.RemoveListener(InvokeNextLevel);
        }

        private void InvokeRestart()
        {
            OnRestartPressed?.Invoke();
        }
        
        private void InvokeNextLevel()
        {
            OnNextLevelPressed?.Invoke();
        }
    }
}