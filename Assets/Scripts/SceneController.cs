using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TestTask.Gameplay
{
    using System;
    using TestTask.UI;

    public interface ISceneController
    {
        CarBehaviour GetPlayer();
        void AddCoins(int count);
        event Action OnVictory;
        event Action OnDefeat;
    }

    public class SceneController : MonoBehaviour, ISceneController
    {
        public static ISceneController Instance;

        public event Action OnVictory;
        public event Action OnDefeat;

        [SerializeField] private GameCameras _cameras;
        [SerializeField] private CarBehaviour _carPrefab;
        [SerializeField] private SpawnGround _spawnGround;

        private CarBehaviour _player;

        private int _coin;

        private int _currentLevel = 1;
        private int _upLevel = 1;

        private void Awake()
        {
            Instance = this;

            _player = Instantiate(_carPrefab, transform);

            _player.OnDie += Defeat;
            _cameras.SetCameraTarget(_player.transform);
            _currentLevel = PlayerPrefs.GetInt("upLevel");

        }

        private void Start()
        {
            StartCoroutine(GameSequence());
        }

        private void Update()
        {

        }
        private IEnumerator GameSequence()
        {
            Time.timeScale = 0;

            yield return null;

            UIController.Instance.ShowScreen<TitleUI>();

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            UIController.Instance.ShowScreen<GameUI>(hideOthers: true);
            _cameras.EnableGameCamera();

            UIController.Instance.GetScreen<GameUI>().SetLevel(_currentLevel);

            Time.timeScale = 1;

            var playerStartPosition = _player.transform.position;
            var finishPosition = _spawnGround.GetLastGround().transform.position;

            var progress = 0f;

            do
            {
                progress = MapValue(_player.transform.position.z, playerStartPosition.z, finishPosition.z, 0, 1);

                UIController.Instance.GetScreen<GameUI>().SetBarValue01(progress);

                yield return null;
            } while (progress < 1);

            Victory();
        }

        private float MapValue(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public void AddCoins(int count)
        {
            _coin = PlayerPrefs.GetInt("coin");
            PlayerPrefs.SetInt("coin", count + _coin);

            UIController.Instance.GetScreen<GameUI>().SetCoin(_coin);
        }

        public void LastLevel()
        {
            _currentLevel = PlayerPrefs.GetInt("upLevel");
            PlayerPrefs.SetInt("upLevel", _upLevel + _currentLevel);
            UIController.Instance.GetScreen<GameUI>().SetLevel(_currentLevel);
        }

        private void Defeat()
        {
            _player.StopMove();

            UIController.Instance.ShowScreen<FailedUI>().SetCoin(_coin);

            GameOver();

            OnDefeat?.Invoke();
        }

        private void Victory()
        {
            _player.StopMove();

            UIController.Instance.ShowScreen<VictoryUI>().SetCoin(_coin);

            GameOver();

            OnVictory?.Invoke();
        }

        private void GameOver()
        {
            StartCoroutine(WaitToRestart());
        }

        private IEnumerator WaitToRestart()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            SceneManager.LoadScene(0);
            LastLevel();
            UIController.Instance.HideAll();
        }

        public CarBehaviour GetPlayer()
        {
            return _player;
        }

        private void OnDisable()
        {
            _player.OnDie -= Defeat;
        }
    }
}
