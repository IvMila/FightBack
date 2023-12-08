using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Gameplay
{
    public class HealthSlider : MonoBehaviour
    {
        [SerializeField] private Slider _sliderHP;
        [SerializeField] private Image _fillArea;
        private float _timeScele = 0;
        private int _targetHP;

        public int SetHealth(int hp)
        {
            _targetHP = hp;
            _timeScele = 0;
            StartCoroutine(LerpHealth());
            return hp;
        }

        private void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        private IEnumerator LerpHealth()
        {
            float speedLerp = 5;
            float startHP = _sliderHP.value;

            while (_timeScele < 1)
            {
                _timeScele += Time.deltaTime * speedLerp;

                _sliderHP.value = Mathf.Lerp(startHP, _targetHP, _timeScele);
                yield return null;
            }
        }
    }
}
