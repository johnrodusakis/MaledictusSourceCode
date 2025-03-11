using Maledictus.Inventory;
using TMPro;
using UnityEngine;
using VInspector;

namespace Maledictus.CustomUI
{
    public enum ThemeStyle
    {
        Bold,
        Light,
        Medium,
        Regular,
        SemiBold
    }

    [CreateAssetMenu(menuName = "CustomUI/Theme")]
    public class CustomThemeSO : ScriptableObject
    {
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
        [SerializeField] private TMP_FontAsset _boldFont;
        [SerializeField] private TMP_FontAsset _lightFont;
        [SerializeField] private TMP_FontAsset _mediumFont;
        [SerializeField] private TMP_FontAsset _regularFont;
        [SerializeField] private TMP_FontAsset _semiBoldFont;

        public TMP_FontAsset GetTextFont(ThemeStyle style) => style switch
        {
            ThemeStyle.Bold => _boldFont,
            ThemeStyle.Light => _lightFont,
            ThemeStyle.Medium => _mediumFont,
            ThemeStyle.Regular => _regularFont,
            ThemeStyle.SemiBold => _semiBoldFont,
            _ => null
        };
    }
}