using Maledictus.CustomUI;
using UnityEngine;

namespace Maledictus.GameMenu
{
    [RequireComponent(typeof(CustomImage))]
    public class NotificationUI : MonoBehaviour
    {
        private CustomImage _notificationImage;

        private void Awake()
        {
            _notificationImage = GetComponent<CustomImage>();
        }

        public bool HasNotification => _notificationImage.IsImageEnabled;

        public void DisplayNotification(bool visible)
        {
            if(visible)
                _notificationImage.FadeIn();
            else
                _notificationImage.FadeOut();

        }
    }
}