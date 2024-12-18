using Maledictus.Inventory;
using TMPro;
using UnityEngine;
using VInspector;

namespace Maledictus.CustomUI
{
    public enum ThemeStyle
    {
        Primary,
        Secondary,
        Tertiary
    }

    [CreateAssetMenu(menuName = "CustomUI/Theme")]
    public class CustomThemeSO : ScriptableObject
    {
        [Foldout("Color Pallet")]
        [SerializeField] private Color _primaryColor = Color.white;
        [SerializeField] private Color _secondaryColor = Color.white;
        [SerializeField] private Color _tertiaryColor = Color.white;

        [SerializeField] private Color _disableColor = Color.white;

        public Color GetBackgroundColor(ThemeStyle style) => style switch
        {
            ThemeStyle.Primary => _primaryColor,
            ThemeStyle.Secondary => _secondaryColor,
            ThemeStyle.Tertiary => _tertiaryColor,
            _ => _disableColor
        };


        [Space(15f)]
        [Header("Rarity Color")]
        [SerializeField] private Color _commonColor = Color.white;
        [SerializeField] private Color _uncommonColor = Color.white;
        [SerializeField] private Color _rareColor = Color.white;
        [SerializeField] private Color _mythicColor = Color.white;
        [SerializeField] private Color _legendaryColor = Color.white;
        
        public Color GetRarityColor(Rarity rarity) => rarity switch
        {
            Rarity.Common => _commonColor,
            Rarity.Uncommon => _uncommonColor,
            Rarity.Rare => _rareColor,
            Rarity.Mythic => _mythicColor,
            Rarity.Legendary => _legendaryColor,
            _ => Color.white,
        };

        [Foldout("Text Data")]
        [SerializeField] private Color _primaryTextColor = Color.white;
        [SerializeField] private Color _secondaryTextColor = Color.white;
        [SerializeField] private Color _tertiaryTextColor = Color.white;

        [SerializeField] private TMP_FontAsset _primaryFont;
        [SerializeField] private TMP_FontAsset _secondaryFont;
        [SerializeField] private TMP_FontAsset _tertiaryFont;

        public Color GetTextColor(ThemeStyle style) => style switch
        {
            ThemeStyle.Primary => _primaryTextColor,
            ThemeStyle.Secondary => _secondaryTextColor,
            ThemeStyle.Tertiary => _tertiaryTextColor,
            _ => _disableColor
        };

        public TMP_FontAsset GetTextFont(ThemeStyle style) => style switch
        {
            ThemeStyle.Primary => _primaryFont,
            ThemeStyle.Secondary => _secondaryFont,
            ThemeStyle.Tertiary => _tertiaryFont,
            _ => null
        };
    }
}