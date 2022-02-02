using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keep track of equipment. Has functions for adding and removing items
public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public Equipment[] defaultItems;
    public SkinnedMeshRenderer targetMesh;
    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;

    // Callback for when an item is equipped/unequipped
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    void Start()
    {
        // Cache a reference to invetory singleton
        inventory = Inventory.instance;

        // Initialize currentEquipment based on number of equipment slots
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems();
    }

    // Equip new item
    public void Equip(Equipment newItem)
    {
        // Find out what slot an item fits in, unequip it and equip new item
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = Unequip(slotIndex);

        // Trigget callback for equiping an item
        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, null);
        }

        SetEquipmentBlendShapes(newItem, 100);

        // Insert the item into the slot
        currentEquipment[slotIndex] = newItem;
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }

    // Unequip an item with a particular index
    public Equipment Unequip(int slotIndex)
    {
        // Only do this if an item is there, add old item to the inventory
        if(currentEquipment[slotIndex] != null)
        {
            if(currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }

            Equipment oldItem = currentEquipment[slotIndex];
            SetEquipmentBlendShapes(oldItem, 0);

            // Check to see if inventory is full before unequiping the item
            if (inventory.Add(oldItem))
            {
                currentEquipment[slotIndex] = null;
            }

            // Trigger callback for unequiping the item
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }

            return oldItem;
        }
        return null;
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }

        EquipDefaultItems();
    }

    void SetEquipmentBlendShapes(Equipment item, int weight)
    {
        foreach(EquipmentMeshRegion blendShape in item.coveredMeshRegions)
        {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight);
        }
    }

    void EquipDefaultItems()
    {
        foreach(Equipment item in defaultItems)
        {
            Equip(item);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }
}
