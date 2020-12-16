using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInventorySystem;

// this script should updated 
public class UI_Inventory_Controller : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField]
    private UI_InventorySlot_Controller[] itemSlots;

    [SerializeField]
    private GameObject InventoryItemTemplate;

    // Start is called before the first frame update
    void Start()
    {
        inventory.InventoryEvent += Inventory_InventoryEvent;   
        for(int i=0;i<inventory.maxSize;i++)
        {
            UpdateSlot(i);
        }
    }

    private void OnDestroy()
    {
        inventory.InventoryEvent -= Inventory_InventoryEvent;
    }

    private void Inventory_InventoryEvent(object sender, InventoryEventArgs e)
    {
        switch(e.InventoryEventType)
        {
            case InventoryEventType.UPDATE:
                UpdateSlot(e.Slot.Value);
                break;
            default:
                break;
        }
    }
    
    private void UpdateSlot(int Slot)
    {
        if(inventory.Items[Slot]!=null)
        {
            if (itemSlots[Slot].HasItem)
            {
                itemSlots[Slot].Item.ItemID = inventory.Items[Slot].ID;
                itemSlots[Slot].Item.ItemQuantity = inventory.Items[Slot].Quantity;
            }
            else
            {
                GameObject slotObject = Instantiate(InventoryItemTemplate, itemSlots[Slot].transform);
                itemSlots[Slot].Item = slotObject.GetComponentInChildren<UI_InventoryItem_Controller>();
                itemSlots[Slot].Item.ItemID = inventory.Items[Slot].ID;
                itemSlots[Slot].Item.ItemQuantity = inventory.Items[Slot].Quantity;
            }
            itemSlots[Slot].Item.UpdatePortrait();
            itemSlots[Slot].Item.SetOriginInventory(inventory);
        }
        else
        {
            if(itemSlots[Slot].HasItem)
            {
                Destroy(itemSlots[Slot].transform.GetChild(0).gameObject);
                itemSlots[Slot].Item = null;
            }
        }
    }
}
