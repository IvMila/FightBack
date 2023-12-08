using System;
using System.Collections;
using UnityEngine;

namespace TestTask.Gameplay
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(Collider))]

    public class StickmanBehaviour : MonoBehaviour
    {
        private static int ANIMATOR_RUN_TRIGGER = Animator.StringToHash("Running");
        private static int ANIMATOR_DIE_TRIGGER = Animator.StringToHash("Death");
        private static int ANIMATOR_WALK_TRIGGER = Animator.StringToHash("Walk");

        public event Action OnHit;

        private CarBehaviour Player => SceneController.Instance.GetPlayer();

        private Animator _stickmanAnimator;
        private Rigidbody _rigidbody;
        private Collider _collider;

        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private GameObject _coinParticles;
        [SerializeField] private HealthSlider _hpBehaviour;

        [SerializeField] private ParticleSystem _bloodParticle;

        [SerializeField] private float _walkSpeed = 0.1f;
        [SerializeField] private float _runSpeed = 7f;
        [SerializeField] private float _speedRotatiion = 20f;
        [SerializeField] private float _agroRadius = 20f;

        [SerializeField] private int _stickmanHealthMax = 100;
        private int _stickmanHealth;

        private float _currentSpeeed;

        private float _distanceBetweenPlayer = float.MaxValue;

        private bool _isAlive = true;

        public bool IsAlive => _isAlive;

        [SerializeField] private int _damage = 25;

        private void Start()
        {
            _stickmanAnimator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            _stickmanHealth = _stickmanHealthMax;

            _hpBehaviour.gameObject.SetActive(false);

            SceneController.Instance.OnVictory += Die;
        }

        private void OnDisable()
        {
            SceneController.Instance.OnVictory -= Die;
        }

        private void Update()
        {
            if (!_isAlive)
                return;
            if (Player)
                _distanceBetweenPlayer = Vector3.Distance(transform.position, Player.transform.position);

            if (Player && Player.IsAlive && _distanceBetweenPlayer < _agroRadius)
            {
                SetMoveType(MoveType.Run);

                var targetDirection = Player.transform.position - transform.position;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), _speedRotatiion * Time.deltaTime);
            }
            else
            {
                SetMoveType(MoveType.Walk);
            }
        }

        private void FixedUpdate()
        {
            if (!_isAlive)
                return;
            _rigidbody.velocity = transform.forward * _currentSpeeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var car = collision.collider.GetComponent<CarBehaviour>();

            if (car)
            {
                car.Hit(_damage);
                Destroy(gameObject);
            }
        }

        public void Hit(int damage)
        {
            if (!_isAlive)
                return;

            _stickmanHealth = Mathf.Max(_stickmanHealth - damage, 0);

            _hpBehaviour.gameObject.SetActive(true);
            _hpBehaviour.SetHealth(_stickmanHealth);


            Destroy(Instantiate(_bloodParticle, transform.position, Quaternion.identity), 3f);

            OnHit?.Invoke();

            if (_stickmanHealth <= 0)
                Die();
        }

        private void SetMoveType(MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Walk:
                    {
                        _currentSpeeed = _walkSpeed;
                        _stickmanAnimator.SetTrigger(ANIMATOR_WALK_TRIGGER);
                    };break;
                case MoveType.Run:
                    {
                        _currentSpeeed = _runSpeed;
                        _stickmanAnimator.SetTrigger(ANIMATOR_RUN_TRIGGER);
                    };break;
            }
        }

        private void Die()
        {
            if (!_isAlive)
               return;

            _isAlive = false;

            _collider.enabled = false;
            _rigidbody.isKinematic = true;

            _hpBehaviour.gameObject.SetActive(false);

            _meshRenderer.sharedMaterial = _deathMaterial;

            SceneController.Instance.AddCoins(10);

            StartCoroutine(DieCoroutine());

            IEnumerator DieCoroutine()
            {
                _stickmanAnimator.SetTrigger(ANIMATOR_DIE_TRIGGER);
                Destroy(Instantiate(_coinParticles, transform.position, Quaternion.identity), 2f);

                yield return new WaitForSeconds(2f);
                Destroy(gameObject);
            }
        }

        private enum MoveType
        {
            Walk,
            Run
        }
    }
}

