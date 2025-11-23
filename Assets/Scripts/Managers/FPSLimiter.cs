namespace Managers
{
    using UnityEngine;

    public class FPSLimiter : MonoBehaviour
    {
        [Range(-1, 240)]
        public int targetFPS = 144;

        void Awake()
        {
            ApplySettings();
        }

        void ApplySettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFPS;
        }

        void OnValidate()
        {
            ApplySettings();
        }
    }
}