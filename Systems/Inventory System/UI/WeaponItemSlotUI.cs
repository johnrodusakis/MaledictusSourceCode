using Maledictus.Events;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.Inventory
{

    public class WeaponItemSlotUI : ItemSlotUI<WeaponItemSO>
    {
        [Tab("Events")]
        [SerializeField] private ScriptableEventWeaponSlot _onWeaponSelected;

        [SerializeField] private ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] private ScriptableEventNoParam _onHideItemTooltip;

        public void InitializeSlot(ItemSlot<WeaponItemSO> weaponSLot)
        {
            _itemSlot = weaponSLot;

            _itemHighlight.EnableImage(false);
            _itemSelect.EnableImage(weaponSLot.IsSelected);

            _itemBackground.SetColor(_theme.GetRarityColor(ItemRarity.Common));

            _itemIcon.SetAlpha(1f);
            _itemIcon.SetImage(weaponSLot.Item.Icon);
            _itemIcon.EnableImage(true);

            _itemBackground.SetColor(_theme.GetRarityColor(weaponSLot.Item.ItemRarity));

            _itemNameText.SetText(weaponSLot.Item.Name);
            _itemLevelText.SetText($"Lv{weaponSLot.Item.Level}");

            var damageText = $"Atk Dmg {weaponSLot.Item.Damage}";
            _itemStatText.SetText(damageText);

            var statsDiff = weaponSLot.Item.Damage - Random.Range(0, 100);

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

            var isItemBlocked = weaponSLot.Item.Level > Random.Range(999, 99999);
            _itemBlocked.EnableImage(isItemBlocked);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            var item = _itemSlot.Item;

            if (item != null)
                _onShowItemTooltip.Raise(new ItemTooltip(transform.position, item.Name, item.Level, item.ItemRarity, item.WeaponType.ToString()));

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

            _onWeaponSelected.Raise(_itemSlot);
        }
    }
}