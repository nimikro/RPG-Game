using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";    // Name of the item
    public Sprite icon = null;              // Item icon
    public bool isDefaultItem = false;      // Is the item default wear?
    public int itemAmount = 1;              // Default amount of the item
    public int maxStack = 10;               // Default maximum stack amount for the item

    public virtual void Use()
    {
        // Use the item
        // This method is virtual, meaning it will be overwritten based on functionality of each object
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
