using UnityEngine;

namespace TestTask.UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
