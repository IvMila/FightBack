using UnityEngine;
using Cinemachine;

namespace TestTask.Gameplay
{
    public class GameCameras : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _gameCamera;
        [SerializeField] private CinemachineVirtualCamera _startCamera;

        private void Awake()
        {
            _startCamera.Priority = 1;
            _gameCamera.Priority = 0;
        }

        public void SetCameraTarget(Transform target)
        {
            _gameCamera.Follow = target;
        }

        public void EnableGameCamera()
        {
            _gameCamera.Priority = 1;
            _startCamera.Priority = 0;
        }
    }
}