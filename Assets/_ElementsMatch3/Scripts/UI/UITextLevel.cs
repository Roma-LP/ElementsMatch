using _ElementsMatch3.Scripts.Levels;
using TMPro;
using UnityEngine;

namespace _ElementsMatch3.Scripts.UI
{
    public class UITextLevel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentLevel;
        [SerializeField] private string format = "LEVEL: {0:D2}";

        private LevelLoader _levelLoader;

        public void Init(LevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
            SetNewCurrentLevelText(_levelLoader.CurrentLevelNumber);

            _levelLoader.OnСurrentLevelChanged += SetNewCurrentLevelText;
        }

        private void OnDestroy()
        {
            _levelLoader.OnСurrentLevelChanged -= SetNewCurrentLevelText;
        }

        private void SetNewCurrentLevelText(int levelNumber)
        {
            _currentLevel.text = string.Format(format, levelNumber);
        }
    }
}
