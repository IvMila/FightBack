using UnityEngine;

namespace TestTask.UI
{
    public class UIInstantiate : MonoBehaviour
    {
        private void Awake()
        {
            InstantiateUI();
        }

        private void InstantiateUI()
        {
            if (UIController.Instance == null)
            {
                DontDestroyOnLoad(Instantiate(Resources.Load<UIController>("UI/UIController")));
            }
        }
    }
}
