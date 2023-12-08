using UnityEngine;
using System.Collections;

namespace TestTask.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField] private float _startForce = 100;
        [SerializeField] private float _lifeTime = 2;
        [SerializeField] private int _damage =100;

        private IEnumerator Start()
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * _startForce, ForceMode.Impulse);

            yield return new WaitForSeconds(_lifeTime);

            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var stickman = collision.collider.GetComponent<StickmanBehaviour>();

            if(stickman)
            {
                stickman.Hit(_damage);
                Destroy(gameObject);
            }
        }
    }
}
