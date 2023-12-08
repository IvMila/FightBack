using UnityEngine;

namespace TestTask.Gameplay
{
    public class TurretController : MonoBehaviour
    {
        [SerializeField] private LayerMask _inputLayers;
        [SerializeField] private float _smoothSpeed = 4f;

        private Vector3 _lookDirection = Vector3.forward;

        private void Update()
        {
            if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 1000, _inputLayers))
            {
                _lookDirection = (raycastHit.point - transform.position).normalized;
                _lookDirection.y = 0;

                transform.forward = Vector3.Slerp(transform.forward, _lookDirection, Time.deltaTime * _smoothSpeed);
            }
        }
    }
}
