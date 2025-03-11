namespace Maledictus.Inventory
{
    [System.Serializable]
    public class ItemSlot<T> : InventorySlot<T> where T : ItemSO
    {
        public double Order { get; protected set; }
        public bool IsSelected { get; private set; } = false;
        public bool IsNew { get; private set; } = false;

        public void SetItem(T item, double order)
        {
            Order = order;
            IsNew = true;
            SetItem(item);
        }

        public void SetItem(T item, double order, bool isSelected, bool isNew)
        {
            Order = order;
            IsSelected = isSelected;
            IsNew = isNew;
            SetItem(item);
        }

        public void HideNotification() => IsNew = false;

        public void SelectSlot() => IsSelected = true;
        public void DeselectSlot() => IsSelected = false;

        public override void ClearSlot()
        {
            Order = 0;
            SetItem(default, default);
            DeselectSlot();
        }
    }
}