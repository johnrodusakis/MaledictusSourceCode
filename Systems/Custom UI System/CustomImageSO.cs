using UnityEngine;

namespace Maledictus.CustomUI
{
    [CreateAssetMenu(menuName = "CustomUI/Image")]
    public class CustomImageSO : ScriptableObject
    {
        public CustomThemeSO Theme;

        public Sprite Sprite;
    }
}