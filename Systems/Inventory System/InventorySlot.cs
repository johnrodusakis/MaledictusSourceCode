using UnityEngine;

namespace Maledictus.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemSO Item;

        [Min(0)]
        public int Amount;

        public bool IsLocked { get; private set; } = false;
        public bool IsEmpty => Item == null;

        public bool IsSelected { get; private set; } = false;

        public InventorySlot()
        {
            ClearSlot();
            IsLocked = false;
            IsSelected = false;
        }

        public InventorySlot(ItemSO item, int amount)
        {
            SetItem(item, amount);
            IsLocked = false;
            IsSelected = false;
        }

        public InventorySlot(ItemSO item, int amount, bool isLocked)
        {
            SetItem(item, amount);
            IsLocked = isLocked;
            Item = item;
            IsSelected = false;
        }

        public void SetItem(ItemSO item, int amount)
        {
            Item = item;
            Amount = amount;
        }

        public void AddAmount(int value) => Amount += value;
        public void RemoveAmount(int value) => Amount -= value;

        public void ClearSlot() => SetItem(null, 0);

        public void UnlockSlot() => IsLocked = false;

        public void SelectSlot() => IsSelected = true;
        public void DeselectSlot() => IsSelected = false;
    }
}