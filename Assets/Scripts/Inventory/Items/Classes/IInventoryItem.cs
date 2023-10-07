public interface IInventoryItem
{
    string GetID();
    int GetQuantity();
    bool IsStackable();
}
