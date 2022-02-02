using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot; // Slot to store equipment in
    public SkinnedMeshRenderer mesh;
    public EquipmentMeshRegion[] coveredMeshRegions;

    public int armorModifier;       // Increase/decrease in armor
    public int damageModifier;      // Increase/decrease in damage

    public override void Use()
    {
        base.Use();
        // Remove item from the inventory
        RemoveFromInventory();
        // Equip the item
        EquipmentManager.instance.Equip(this);
    }
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }
public enum EquipmentMeshRegion { Legs, Arms, Torso } // Corresponds to body blendshapes
