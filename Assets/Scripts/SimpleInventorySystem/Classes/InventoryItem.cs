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
            int _limit = ItemDatabase.FetchItemByID(ID).Limit;
            switch (LimitType)
            {
                case ItemLimitType.Global:
                    _limit =  ItemDatabase.DefaultItemLimit;
                    break;
                case ItemLimitType.Item:
                    _limit = ItemDatabase.FetchItemByID(ID).Limit;
                    break;
                case ItemLimitType.None:
                    _limit = int.MaxValue;
                    break;
                case ItemLimitType.Inventory:
                    _limit = Inventory.ItemLimit;
                    break;
                default:
                    _limit = int.MaxValue;
                    break;
            }
            return _limit;
        }

        private bool CheckIfFull()
        {
            bool _full = false;
            switch (LimitType)
            {
                case ItemLimitType.Global:
                    _full = Quantity >= ItemDatabase.DefaultItemLimit;
                    break;
                case ItemLimitType.Item:
                    _full = Quantity >= Limit;
                    break;
                case ItemLimitType.None:
                    _full = false;
                    break;
                case ItemLimitType.Inventory:
                    _full = Quantity >= Inventory.ItemLimit;
                    break;
                default:
                    _full = true;
                    break;
            }
            return _full;
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

