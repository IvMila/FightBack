using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace TestTask.UI
{
    public class GameUI : UIScreen
    {
        [SerializeField] private Button _pauseButton;

        [SerializeField] private Image _progressBar;

        [SerializeField] private TextMeshProUGUI _coinText;

        [SerializeField] private TextMeshProUGUI _levelText;

        private bool _isPause = false;

        private void Start()
        {
            _pauseButton.onClick.AddListener(TogglePause);
            SetBarValue01(0);
        }

        private void TogglePause()
        {
            if(_pauseButton)
            {
                _isPause = !_isPause;
                PauseManager();
            }
        }

        private void PauseManager()
        {
            Time.timeScale = _isPause ? 0 : 1;
        }

        public void SetBarValue01(float value01)
        {
            _progressBar.fillAmount = value01;
        }

        public void SetCoin(int coin)
        {
            _coinText.text = coin.ToString();
        }

        public void SetLevel(int level)
        {
            _levelText.text = level.ToString();
        }
    }
}
