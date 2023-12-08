using UnityEngine;

namespace TestTask.Gameplay
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private StickmanBehaviour _stickmanBehavior;

        public void CreateStickmans()
        {
            var countPointSpawn = Random.Range(10, 20);

            for (int i = 0; i < countPointSpawn; i++)
            {
                Quaternion randomRotation = Random.rotation;
                randomRotation.x = 0;
                randomRotation.z = 0;

                Vector3 spawnPoint = transform.position + Random.insideUnitSphere * 40;

                spawnPoint.y = 0;

                Instantiate(_stickmanBehavior, spawnPoint, randomRotation, transform);
            }
        }

        private void Update()
        {
            var delta = SceneController.Instance.GetPlayer().transform.position.z - transform.position.z;

            if (delta > ScriptConstants.OFFSET_BETWEEN_GROUND)
                Destroy(gameObject);
        }
    }
}
