using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleInventorySystem
{
    public class InventoryItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public string Name => ItemDatabase.FetchItemByID(ID).Name;
        public string Description => ItemDatabase.FetchItemByID(ID).Desc;
        public float Weight => ItemDatabase.FetchItemByID(ID).Weight;
        public int Limit => ItemDatabase.FetchItemByID(ID).Limit;

        public InventoryItem(int _ID)
        {
            ID = _ID;
            Quantity = 1;
        }

        public InventoryItem(int _ID, int _Qty)
        {
            ID = _ID;
            Quantity = _Qty;
        }

        public InventoryItem(Item item, int _Qty)
        {
            ID = item.ID;
            Quantity = _Qty;
        }

        public InventoryItem(InventoryItem inventoryItem)
        {
            if (inventoryItem == null) return;
            ID = inventoryItem.ID;
            Quantity = inventoryItem.Quantity;
        }
    }
}

