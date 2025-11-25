using System;
using Enums;
using Managers;
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
            EffectsManager.Instance.PlayEffect(TimeWarpType.Slow, OnFinishedTimeWarp);
            EffectsManager.Instance.PlayFadeScreen(false);
        }

        private void OnFinishedTimeWarp()
        {
            ScreenManager.Instance.PushScreen(ScreenType.FinalMenu, false);
            EventManager.GameEvents.IsLevelFinished.Invoke(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}