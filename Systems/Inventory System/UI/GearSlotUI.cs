using Maledictus.CustomUI;
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
            EnableHighlight(false);
        }

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
    }

    public abstract class GearSlotUI<T> : SlotUI<T> where T : ItemSO
    {
        [Tab("UI")]
        [SerializeField] protected CustomImage _itemLocked;

        [SerializeField] private Sprite _itemPlaceholderSprite;

        protected GearSlot<T> _gearSlot;

        protected GameObject _itemListObject;

        protected void InitializeGearSlotUI(bool isAvailable, bool isLocked, Sprite icon, ItemRarity rarity)
        {
            _itemIcon.EnableImage(false);
            _itemHighlight.EnableImage(false);

            _itemBackground.SetColor(_theme.GetRarityColor(ItemRarity.Common));

            if (isAvailable)
            {
                _itemIcon.SetAlpha(1f);
                _itemIcon.SetImage(icon);
                _itemIcon.EnableImage(true);

                _itemBackground.SetColor(_theme.GetRarityColor(rarity));

                _itemLocked.EnableImage(false);
            }
            else if (isLocked)
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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            _itemListObject.SetActive(true);
        }
    }
}