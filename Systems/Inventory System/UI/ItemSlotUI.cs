using Maledictus.CustomUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Maledictus.Inventory
{
    public abstract class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        protected static InventorySlot _selectedInventorySlot;

        [SerializeField] private CustomThemeSO _theme;

        [SerializeField] private CustomImage _itemBackground;
        [SerializeField] private CustomImage _itemHighlight;
        [SerializeField] private CustomImage _itemSelect;
        [SerializeField] protected CustomImage _itemIconImage;

        protected bool _isHovered;
        protected bool _isSelected;

        protected InventorySlot _inventorySlot;

        protected void CreateSlot(InventorySlot inventorySlot)
        {
            _inventorySlot = inventorySlot;

            _itemIconImage.EnableImage(false);
            _itemHighlight.EnableImage(false);
            _itemSelect.EnableImage(false);

            _itemBackground.SetColor(_theme.GetRarityColor(Rarity.Common));

            if (!inventorySlot.IsEmpty && !inventorySlot.IsLocked)
            {
                _itemIconImage.SetAlpha(1f);
                _itemIconImage.SetImage(inventorySlot.Item.Icon);
                _itemIconImage.EnableImage(true);

                _itemBackground.SetColor(_theme.GetRarityColor(inventorySlot.Item.Rarity));

                HandleOccupiedSlot();
            }
            else if(inventorySlot.IsLocked)
                HandleLockedSlot();
            else
                HandleEmptySlot();
        }

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

        protected abstract void HandleOccupiedSlot();
        protected abstract void HandleLockedSlot();
        protected abstract void HandleEmptySlot();

        protected void EnableHighlight(bool enable)
        {
            if(enable)
                _itemHighlight.FadeIn();
            else
                _itemHighlight.FadeOut();

            _isHovered = enable;
        }
    }
}