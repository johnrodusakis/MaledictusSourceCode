using Maledictus.CustomSoap;
using Maledictus.CustomUI;
using Maledictus.GameMenu;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.Inventory
{
    public abstract class SlotUI<T> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler where T : ItemSO
    {
        [Tab("UI")]
        [SerializeField] protected CustomThemeSO _theme;

        [SerializeField] protected CustomImage _itemBackground;
        [SerializeField] protected CustomImage _itemHighlight;
        [SerializeField] protected CustomImage _itemIcon;

        [SerializeField] protected NotificationUI _itemNotificationUI;

        [Tab("Events")]
        [SerializeField] protected ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] protected ScriptableEventNoParam _onHideItemTooltip;
        [SerializeField] protected ScriptableEventNoParam _onItemNewNotification;

        public bool HasNotification => _itemNotificationUI.HasNotification;

        protected T _item;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            EnableHighlight(true);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            EnableHighlight(false);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            HideTooltip();
            EnableHighlight(false);
        }

        protected void DisplayTooltip()
        {
            if (_item != null)
                _onShowItemTooltip.Raise(new ItemTooltip(transform.position, _item.Name, _item.Level, _item.ItemRarity, _item.Description));
        }

        protected void HideTooltip() => _onHideItemTooltip.Raise();

        protected void EnableHighlight(bool enable)
        {
            if (enable)
                _itemHighlight.FadeIn();
            else
                _itemHighlight.FadeOut();
        }
    }

    public abstract class ItemSlotUI<T> : SlotUI<T> where T : ItemSO
    {
        [Tab("UI")]
        [SerializeField] protected CustomText _itemNameText;
        [SerializeField] protected CustomText _itemStatText;
        [SerializeField] protected CustomText _itemStatDifferenceText;
        [SerializeField] protected CustomImage _itemStatArrowImage;

        [SerializeField] protected CustomText _itemLevelText;
        [SerializeField] protected CustomImage _itemSelect;
        [SerializeField] protected CustomImage _itemBlocked;

        protected ItemSlot<T> _itemSlot;

        public abstract string StatusLabel { get; }
        public abstract float StatusValue { get; }

        public virtual void InitializeSlotUI(ItemSlot<T> itemSlot)
        {
            _item = itemSlot.Item;
            _itemSlot = itemSlot;

            _itemNotificationUI.DisplayNotification(itemSlot.IsNew);

            _itemHighlight.EnableImage(false);
            _itemSelect.EnableImage(itemSlot.IsSelected);

            _itemBackground.SetColor(_theme.GetRarityColor(Rarity.Common));

            _itemIcon.SetAlpha(1f);
            _itemIcon.SetImage(itemSlot.Item.Icon);
            _itemIcon.EnableImage(true);

            _itemBackground.SetColor(_theme.GetRarityColor(itemSlot.Item.ItemRarity));

            _itemNameText.SetText(itemSlot.Item.Name);
            _itemLevelText.SetText($"Lv{itemSlot.Item.Level}");

            var damageText = $"{StatusLabel} {StatusValue}";
            _itemStatText.SetText(damageText);

            var statsDiff = StatusValue - Random.Range(0, 100);

            if (statsDiff > 0)
            {
                _itemStatArrowImage.SetColor(Color.green);
                _itemStatArrowImage.FlipY(false);

                _itemStatDifferenceText.SetColor(Color.green);
                _itemStatDifferenceText.SetText($"{Mathf.Abs(statsDiff)}");
            }
            else if (statsDiff < 0)
            {
                _itemStatArrowImage.SetColor(Color.red);
                _itemStatArrowImage.FlipY(true);

                _itemStatDifferenceText.SetColor(Color.red);
                _itemStatDifferenceText.SetText($"{Mathf.Abs(statsDiff)}");
            }
            else
            {
                _itemStatArrowImage.EnableImage(false);
                _itemStatDifferenceText.SetText(string.Empty);
            }

            var isItemBlocked = itemSlot.Item.Level > Random.Range(999, 99999);
            _itemBlocked.EnableImage(isItemBlocked);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            DisplayTooltip();

            if (_itemSlot.IsNew)
            {
                if (_itemNotificationUI.HasNotification)
                    _itemNotificationUI.DisplayNotification(false);

                _itemSlot.HideNotification();
                _onItemNewNotification.Raise();
            }

            if (_itemSlot.IsSelected) return;

            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();

            if (_itemSlot.IsSelected) return;

            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_itemSlot.IsSelected) return;

            base.OnPointerClick(eventData);
        }
    }

    public abstract class GearSlotUI<T> : SlotUI<T> where T : ItemSO
    {
        [Tab("UI")]
        [SerializeField] protected CustomImage _itemLocked;

        [SerializeField] private Sprite _itemPlaceholderSprite;

        protected GearSlot<T> _gearSlot;

        protected BaseItemController<T> _itemController;

        public virtual void InitializeSlotUI(GearSlot<T> slot, BaseItemController<T> itemController)
        {
            _item = slot.SelectedSlot.Item;

            _gearSlot = slot;
            _itemController = itemController;

            _itemIcon.EnableImage(false);
            _itemHighlight.EnableImage(false);

            _itemBackground.SetColor(_theme.GetRarityColor(Rarity.Common));

            if (!slot.SelectedSlot.IsEmpty && !slot.IsLocked)
            {
                _itemIcon.SetAlpha(1f);
                _itemIcon.SetImage(_item.Icon);
                _itemIcon.EnableImage(true);

                _itemBackground.SetColor(_theme.GetRarityColor(_item.ItemRarity));

                _itemLocked.EnableImage(false);
            }
            else if (slot.IsLocked)
            {
                _itemIcon.SetImage(_itemPlaceholderSprite);
                _itemIcon.SetAlpha(0.01f);
                _itemIcon.EnableImage(true);

                _itemLocked.EnableImage(true);
            }
            else
            {
                _itemIcon.SetImage(_itemPlaceholderSprite);
                _itemIcon.SetAlpha(0.01f);
                _itemIcon.EnableImage(true);

                _itemLocked.EnableImage(false);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            DisplayTooltip();

            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();

            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if(!_itemController.IsActive)
                _itemController.ActivateItemList();
        }

        public void DisplayNotification(bool visible)
        {
            if (!_gearSlot.IsLocked)
                _itemNotificationUI.DisplayNotification(visible);
        }
    }
}