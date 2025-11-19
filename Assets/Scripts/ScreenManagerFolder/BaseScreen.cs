using Enums;
using UnityEngine;

namespace ScreenManagerFolder
{
    public class BaseScreen : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}