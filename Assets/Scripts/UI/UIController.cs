using System.Collections.Generic;
using UnityEngine;
using System;

namespace TestTask.UI
{
    public interface IUIController
    {
        T ShowScreen<T>(bool hideOthers = false) where T : UIScreen;
        T GetScreen<T>() where T : UIScreen;

        void HideScreen<T>() where T : UIScreen;
        void HideAll();
    }

    public class UIController : MonoBehaviour, IUIController
    {
        public static IUIController Instance;

        private List<UIScreen> _screens = new List<UIScreen>();

        private void Awake()
        {
            Instance = this;

            foreach(var screen in GetComponentsInChildren<UIScreen>(true))
            {
                _screens.Add(screen);
            }

            HideAll();
        }

        public T ShowScreen<T>(bool hideOthers = false) where T : UIScreen
        {
            if (hideOthers)
                HideAll();

            var screen = GetScreen<T>();

            screen.SetActive(true);

            return screen;
        }

        public void HideScreen<T>() where T : UIScreen
        {
            GetScreen<T>().SetActive(false);
        }

        public T GetScreen<T>() where T : UIScreen
        {
            foreach(var screen in _screens)
            {
                if (screen is T)
                    return screen as T;
            }

            throw new NotImplementedException($"{typeof(T).Name} is't assigned");
        }

        public void HideAll()
        {
            foreach (var screen in _screens)
            {
                screen.SetActive(false);
            }
        }

    }
}


