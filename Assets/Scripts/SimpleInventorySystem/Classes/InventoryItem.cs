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
        public string Description => ItemDatabase.FetchItemByID(ID).Description;
        public float Weight => ItemDatabase.FetchItemByID(ID).Weight;
        public ItemLimitType LimitType => ItemDatabase.FetchItemByID(ID).LimitType;
        public int Limit => GetLimit();
        public bool IsFull => CheckIfFull();
        public int RemainingSpace => Limit - Quantity;
        public Inventory Inventory;

        private int GetLimit()
        {
            switch(LimitType)
            {
                case ItemLimitType.Global: return ItemDatabase.DefaultItemLimit;
                case ItemLimitType.Item: return ItemDatabase.FetchItemByID(ID).Limit;
                case ItemLimitType.None: return int.MaxValue;
                case ItemLimitType.Inventory: return Inventory.ItemLimit;
                default: return int.MaxValue;
            };
        }

        private bool CheckIfFull()
        {
            switch(LimitType)
            {
                case ItemLimitType.Global: return Quantity >= ItemDatabase.DefaultItemLimit;
                case ItemLimitType.Item: return Quantity >= Limit;
                case ItemLimitType.None: return false;
                case ItemLimitType.Inventory: return Quantity >= Inventory.ItemLimit;
                default: return true;
            };
        }

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

