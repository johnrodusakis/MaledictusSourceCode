namespace Maledictus.Inventory
{
    [System.Serializable]
    public class InventorySlot<T> where T : ItemSO
    {
        public T Item;

        public bool IsEmpty => Item == null;

        public void SetItem(T item) => Item = item;
        public virtual void ClearSlot() => SetItem(default);
    }
}