using UnityEngine;

namespace Maledictus.CustomUI
{
    [CreateAssetMenu(menuName = "CustomUI/Text")]
    public class CustomTextSO : ScriptableObject
    {
        public CustomThemeSO Theme;

        public Color EnabledColor = Color.white;
        public Color DisabledColor = Color.gray;
    }
}