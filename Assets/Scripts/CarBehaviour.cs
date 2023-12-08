using UnityEngine;
using System;

namespace TestTask.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarBehaviour : MonoBehaviour
    {
        public event Action OnDie;

        public bool IsAlive => _isAlive;

        [SerializeField] private int _speedCar;

        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private HealthSlider _sliderBehaviourPlayerHP;

        private int _heath;

        private Rigidbody _rigidbody;

        private bool _isAlive = true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _heath = _maxHealth;
        }

        private void FixedUpdate()
        {
            MoveForward();
        }

        private void MoveForward()
        {
            _rigidbody.velocity = Vector3.forward * _speedCar;
        }

        public void StopMove()
        {
            _speedCar = 0;
        }

        public void Hit(int damage)
        {
            if (!_isAlive)
                return;

            _heath = Mathf.Max(_heath - damage, 0);
            _sliderBehaviourPlayerHP.SetHealth(_heath);

            if (_heath <= 0)
                Die();
        }

        private void Die()
        {
            if (!_isAlive)
                return;

            _isAlive = false;
            gameObject.SetActive(false);

            OnDie?.Invoke();
        }
    }
}
