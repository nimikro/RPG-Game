using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found!");
            return;
        }

        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        Item copyItem = Instantiate(item);

        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                print("Not enough room.");
                return false;
            }

            if (items.Contains(item) && item.itemAmount < item.maxStack)
            {
                item.itemAmount++;
            }
            else
            {
                items.Add(item);
            }

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }

            return true;
        }

        return false;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
