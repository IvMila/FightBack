using UnityEngine;

namespace TestTask.Gameplay
{
    public class SpawnGround : MonoBehaviour
    {
        [SerializeField] private Ground[] _grounds;
        [SerializeField] private Ground _finishGround;
        private Ground _lastGround;

        private void Awake()
        {
            InstantiateGround();
        }

        private void InstantiateGround()
        {
            Vector3 startPositionGround = Vector3.zero;

            Quaternion rotationX = Quaternion.Euler(-90, 0, 0);

            for (int count = 0; count < ScriptConstants.COUNT_GROUND; count++)
            {
                var offset = (ScriptConstants.OFFSET_BETWEEN_GROUND * count) + startPositionGround.z;
                var newPosition = new Vector3(0, 0.1f, offset);

                var prefab = _grounds[Random.Range(0, _grounds.Length)];

                if (count == ScriptConstants.COUNT_GROUND - 1)
                    prefab = _finishGround;

                var ground = Instantiate(prefab, newPosition, rotationX, transform);

                if (count > 0)
                    ground.CreateStickmans();

                _lastGround = ground;
            }
        }

        public Ground GetLastGround()
        {
            return _lastGround;
        }
    }
}
