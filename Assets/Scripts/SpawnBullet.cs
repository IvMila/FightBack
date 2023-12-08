using UnityEngine;
using System.Collections;

namespace TestTask.Gameplay
{
    public class SpawnBullet : MonoBehaviour
    {
        [SerializeField] private BulletBehaviour _bulletPrefab;

        [SerializeField] private float _reloadTime = 0.2f;

        private Coroutine _coroutineCreateBullet;

        private void Start()
        {
            _coroutineCreateBullet = StartCoroutine(CoroutineCreateBullet());
            SceneController.Instance.OnVictory += StopCoroutineBullet;
        }

        private void StopCoroutineBullet()
        {
            if (_coroutineCreateBullet != null)
                StopCoroutine(_coroutineCreateBullet);      
        }

        private IEnumerator CoroutineCreateBullet()
        {
            do
            {
                yield return new WaitForSeconds(_reloadTime);
                CreateBullet();
            } while (true);
        }

        private BulletBehaviour CreateBullet()
        {
            return Instantiate(_bulletPrefab, transform.position, transform.rotation);
        }

        private void OnDisable()
        {
            SceneController.Instance.OnVictory -= StopCoroutineBullet;
        }
    }
}
