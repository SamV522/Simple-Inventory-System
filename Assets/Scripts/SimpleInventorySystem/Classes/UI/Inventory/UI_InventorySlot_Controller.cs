using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInventorySystem;
using UnityEngine.EventSystems;

public class UI_InventorySlot_Controller : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private UI_Inventory_Controller Inventory_Controller;
    [SerializeField]
    public bool HasItem { get { return transform.childCount > 0; } }
    public UI_InventoryItem_Controller Item;

    public void OnDrop(PointerEventData eventData)
    {
        TryReceiveDrop(eventData.pointerDrag.GetComponentInChildren<UI_InventoryItem_Controller>());
    }

    public void TryReceiveDrop(UI_InventoryItem_Controller item_Controller)
    {
        if(HasItem && Item.ItemID == item_Controller.ItemID && Item!=item_Controller)
        {
            Debug.Log($"{transform.GetSiblingIndex()} - {item_Controller.OriginSlot}");
            Inventory_Controller.inventory.CombineItems(transform.GetSiblingIndex(), item_Controller.OriginSlot);
        }
        else
        {
            if(item_Controller.OriginInventory == Inventory_Controller.inventory)
            {
                Inventory_Controller.inventory.SwapSlots(transform.GetSiblingIndex(), item_Controller.OriginSlot);
            }
            else
            {
                Inventory_Controller.inventory.ExchangeItems(transform.GetSiblingIndex(), item_Controller.OriginInventory, item_Controller.OriginSlot);
            }
            
        }
    }

    /// TODO: Item filtering should go on the inventory level.  Doh.
    /*private void FilterReceiveItem(UI_FactoryInventoryItem_Controller item_Controller)
    {
        switch(Filter)
        {
            case FilterType.NONE:
                TryReceiveItem(item_Controller);
                break;
            case FilterType.BLACKLIST:
                if(!ItemIDFilter.Contains(item_Controller.ItemID))
                {
                    TryReceiveItem(item_Controller);
                }
                break;
            case FilterType.WHITELIST:
                if(ItemIDFilter.Contains(item_Controller.ItemID))
                {
                    TryReceiveItem(item_Controller);
                }
                break;
        }
    }*/

    private void UpdateSlot(int Slot)
    {
        // this should try and update the slot to the requested item.
        if (HasItem)
        {
            Destroy(Item.gameObject);
            Item = null;
        }
    }
}
