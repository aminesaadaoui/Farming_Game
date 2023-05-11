using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    public override void Pickup()
    {
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);
        if (handSlotItem == null) return;
        UIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ? ", PlaceItemsInShippingBin);
    }

    void PlaceItemsInShippingBin()
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        itemsToShip.Add(new ItemSlotData(handSlot));

        handSlot.Empty();

        InventoryManager.Instance.RenderHand();

        foreach (ItemSlotData item in itemsToShip)
        {
            Debug.Log($" in the shipping bin : {item.itemData.name} * {item.quantity}");
        }
    }

    public static void ShipItems()
    {

    }
}
