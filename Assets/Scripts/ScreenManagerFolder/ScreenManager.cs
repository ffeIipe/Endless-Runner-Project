using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace ScreenManagerFolder
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager Instance { get; private set; }
        public List<ScreenRegistration> screenPrefabs;
        
        private Stack<BaseScreen> _screenStack = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            foreach (var registration in screenPrefabs) 
            {
                if (registration.screen != null)
                {
                    registration.screen.Hide();
                }
            }

            PushScreen(ScreenType.Gameplay, false);
        }

        public void PopScreen()
        {
            if (_screenStack.Count <= 1) return;

            var screenToPop = _screenStack.Pop();
            screenToPop.Hide();

            var newTopScreen = _screenStack.Peek();
            newTopScreen.Show();
        }

        public void PushScreen(ScreenType screenType, bool hidePrevious)
        {
            if (hidePrevious && _screenStack.Count > 0)
            {
                var currentTop = _screenStack.Peek();
                currentTop.Hide();
            }

            var newScreen = GetScreen(screenType);
            if (newScreen)
            {
                _screenStack.Push(newScreen);
                newScreen.Show();
            }
            else Debug.LogWarning("¡Pantalla no encontrada!: " + screenType);
        }

        public void SwitchToScreen(ScreenType screenType)
        {
            while (_screenStack.Count > 0)
            {
                var screenToPop = _screenStack.Pop();
                screenToPop.Hide();
            }

            PushScreen(screenType, false);
        }

        public BaseScreen GetScreen(ScreenType screenType)
        {
            var registration = screenPrefabs.FirstOrDefault(s => s.screenType == screenType);
            return registration?.screen;
        }
    }
}