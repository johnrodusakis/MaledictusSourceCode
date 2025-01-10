using Maledictus.Events;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.Inventory
{
    public class AccessoryItemSlotUI : ItemSlotUI<AccessoryItemSO>
    {
        [Tab("Events")]
        [SerializeField] private ScriptableEventAccessorySlot _onAccessorySelected;

        [SerializeField] private ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] private ScriptableEventNoParam _onHideItemTooltip;

        public void InitializeSlotUI(ItemSlot<AccessoryItemSO> accessorySlot)
        {
            _itemSlot = accessorySlot;

            _itemHighlight.EnableImage(false);
            _itemSelect.EnableImage(accessorySlot.IsSelected);

            _itemBackground.SetColor(_theme.GetRarityColor(ItemRarity.Common));

            _itemIcon.SetImage(accessorySlot.Item.Icon);
            _itemIcon.EnableImage(true);
            _itemIcon.SetAlpha(1f);

            _itemBackground.SetColor(_theme.GetRarityColor(accessorySlot.Item.ItemRarity));

            _itemNameText.SetText(accessorySlot.Item.Name);
            _itemLevelText.SetText($"Lv{accessorySlot.Item.Level}");

            var damageText = $"Atk Dmg {accessorySlot.Item.Armor}";
            _itemStatText.SetText(damageText);

            var statsDiff = accessorySlot.Item.Armor - Random.Range(0, 100);

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

            var isItemBlocked = accessorySlot.Item.Level > Random.Range(999, 99999);
            _itemBlocked.EnableImage(isItemBlocked);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            var item = _itemSlot.Item;

            if (item != null)
                _onShowItemTooltip.Raise(new ItemTooltip(transform.position, item.Name, item.Level, item.ItemRarity, item.AccessoryType.ToString()));

            if (_itemSlot.IsSelected) return;

            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _onHideItemTooltip.Raise();

            if (_itemSlot.IsSelected) return;

            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_itemSlot.IsSelected) return;

            base.OnPointerClick(eventData);

            _onAccessorySelected.Raise(_itemSlot);
        }
    }
}