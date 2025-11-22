using System;
using Enums;
using ScreenManagerFolder;
using UnityEngine;

namespace Obstacles
{
    public class Final : MonoBehaviour
    {
        private Trigger _trigger;

        private void Awake()
        {
            _trigger = GetComponentInChildren<Trigger>();
        }

        private void OnEnable()
        {
            _trigger.OnTriggered += ShowFinalScreen;
        }

        private void ShowFinalScreen()
        {
            ScreenManager.Instance.PushScreen(ScreenType.FinalMenu, true);
        }
    }
}