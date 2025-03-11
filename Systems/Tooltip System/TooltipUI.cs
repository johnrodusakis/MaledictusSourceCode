using DG.Tweening;
using Maledictus.CustomUI;
using Maledictus.CustomSoap;
using Obvious.Soap;
using UnityEngine;
using VInspector;

namespace Maledictus.Tooltip
{
    public class TooltipUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _tooltipContent;
        [SerializeField] private CustomThemeSO _theme;
        public Inventory.Rarity Rarity;

        [Space(15f)]
        [Header("Header UI")]
        [SerializeField] private CustomText _title;
        [SerializeField] private CustomText _level;

        [SerializeField] private CustomImage _backgroundColor;

        [Space(15f)]
        [Header("Body UI")]
        [SerializeField] private CustomText _description;
        [SerializeField] private CustomText _rarity;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] private ScriptableEventNoParam _onHideItemTooltip;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            _onHideItemTooltip.OnRaised += HideTooltip;
            _onShowItemTooltip.OnRaised += DisplayItemTooltip;
        }

        private void OnDisable()
        {
            _onHideItemTooltip.OnRaised -= HideTooltip;
            _onShowItemTooltip.OnRaised -= DisplayItemTooltip;
        }

        private void DisplayItemTooltip(ItemTooltip item)
        {
            _title.SetText(item.Name);
            _level.SetText($"{item.Level} lvl");
            _backgroundColor.SetColor(_theme.GetRarityColor(item.Rarity));

            _description.SetText(item.Description);
            _rarity.SetText($"{item.Rarity} Item");

            SetTooltipPosition(item.Position);
            ShowTooltip();
        }

        [Button]
        private void ShowTooltip()
        {
            ResizeTooltip();
            _canvasGroup.DOFade(1f, 0.1f).SetEase(Ease.OutCirc);
        }

        [Button] 
        private void HideTooltip() => _canvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InCirc);
        private void ResizeTooltip() => this.GetComponent<RectTransform>().sizeDelta = _tooltipContent.sizeDelta;
        private void SetTooltipPosition(Vector2 position)
        {
            var tooltipHalfSize = _tooltipContent.sizeDelta * 0.5f;
            var itemSlotSize = 50f;
            var offset = Vector2.zero;
            var mousePos = Input.mousePosition;
            
            bool isLeftHalf = mousePos.x <= Screen.width * 0.5f;
            bool isTopHalf = mousePos.y >= Screen.height * 0.5f;
            
            if (isLeftHalf)
                offset.x = tooltipHalfSize.x + itemSlotSize;
            else
                offset.x = -tooltipHalfSize.x - itemSlotSize;

            if (isTopHalf)
                offset.y = itemSlotSize - tooltipHalfSize.y;
            else
                offset.y = -itemSlotSize + tooltipHalfSize.y;

            transform.position = position + offset;
        }
    }

    [System.Serializable]
    public class ItemTooltip
    {
        public Vector2 Position;

        public string Name;
        public int Level;
        public Inventory.Rarity Rarity;
        public string Description;

        public ItemTooltip(Vector2 position, string name, int level, Inventory.Rarity rarity, string description)
        {
            Position = position;

            Name = name;
            Level = level;
            Rarity = rarity;
            Description = description;
        }
    }
}