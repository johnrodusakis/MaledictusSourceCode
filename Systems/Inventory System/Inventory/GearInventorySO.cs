namespace Maledictus.Inventory
{
    [System.Serializable]
    public class GearInventorySO<T> where T : ItemSO
    {
        public GearSlot<T>[] Slots;
        public int SelectedSlotIndex { get; private set; } = 0;

        public GearInventorySO(int numberOfGearSlots) => Slots = new GearSlot<T>[numberOfGearSlots];

        public void SetSelectedSlotIndex(int index) => SelectedSlotIndex = index;
        public void SetSelectedSlot(ItemSlot<T> slot) => Slots[SelectedSlotIndex].SetSlot(slot);
        public ItemSlot<T> GetSelectedSlot() => Slots[SelectedSlotIndex].SelectedSlot;

        public void ClearInventory()
        {
            foreach (var slot in Slots)
                slot.ClearSlot();
        }
    }
}