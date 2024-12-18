namespace Maledictus.Interaction
{
    using Maledictus.Inventory;

    public interface IInteractable
    {
        string InteractionMessage();
        void Interact(BaseInventory inventory);
    }
}