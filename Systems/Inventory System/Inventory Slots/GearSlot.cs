namespace Maledictus.Inventory
{
    [System.Serializable]
    public class GearSlot<T> where T : ItemSO
    {
        public bool IsLocked = false;
        public ItemSlot<T> SelectedSlot { get; private set; }

        public void SetSlot(ItemSlot<T> selectedSlot) => SelectedSlot = selectedSlot;
        public void SetSlot(ItemSlot<T> selectedSlot, bool isLocked)
        {
            SelectedSlot = selectedSlot;
            IsLocked = isLocked;
        }
        public void UnlockSlot() => IsLocked = false;
        public void ClearSlot() => SelectedSlot.ClearSlot();
    }
}