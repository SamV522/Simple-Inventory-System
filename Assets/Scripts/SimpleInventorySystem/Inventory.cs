using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleInventorySystem;

namespace SimpleInventorySystem
{
    public enum InventoryEventType
    {
        DELETE,
        UPDATE,
        CREATE
    }

    public class InventoryEventArgs : EventArgs
    {
        public InventoryEventType InventoryEventType;

        public InventoryEventArgs(InventoryEventType _InventoryEvent, int Slot)
        {
            InventoryEventType = _InventoryEvent;
        }
    }

    public class Inventory : MonoBehaviour
    {
        public int maxSize = 9;
        public InventoryItem[] Items { get; private set; }
        public int Size { get { return Items.Count(x => x != null); } }
        public bool IsFull => Size >= maxSize;
        public int NextEmpty => Array.IndexOf(Items, null);
        public int ItemLimit = 100;
        public bool ignoreItemLimit = false;

        public event EventHandler InventoryEvent;

        private void Awake()
        {
            Items = new InventoryItem[maxSize];
        }

        public int[] AllIndicesOf(int id)
        {
            List<int> Indices = new List<int>();
            for(int i = 0; i<Items.Length;i++)
            {
                if(Items[i] != null && Items[i].ID == id)
                {
                    Indices.Add(i);
                }
            }
            return Indices.ToArray();
        }

        public (bool,int[]) ItemExists(int id)
        {
            int[] _index = AllIndicesOf(id);
            return (_index.Length>0, _index);
        }

        public bool AddItem(int id, int qty)
        {
            bool _success = false;
            int _remaining = qty;
            if(!IsFull)
            {
                (bool _exists, int[] _indices) = ItemExists(id);
                if (_exists)
                {
                    foreach (int _index in _indices)
                    {
                        int _toAdd = ignoreItemLimit ? qty : Math.Min(qty, Items[_index].Limit);
                        Items[_index].Quantity += _toAdd;
                        _remaining -= _toAdd;
                        InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, _index));
                    }
                }
                else
                {
                    Items[NextEmpty] = new InventoryItem(ItemDatabase.FetchItemByID(id), ignoreItemLimit ? qty : Math.Min(qty, ItemDatabase.FetchItemByID(id).Limit));
                }
                _success = _remaining == 0;
            }
            return _success;
        }

        /// TODO: make a function that exchanges items between inventories
        public bool ExchangeItems(int _sourceSlot, Inventory _targetInventory, int _targetSlot)
        {
            bool _success = false;
            InventoryItem oldSource = new InventoryItem(Items[_sourceSlot]);
            InventoryItem oldDestination = new InventoryItem(_targetInventory.Items[_targetSlot]);
            _targetInventory.Items[_targetSlot] = oldSource;
            Items[_sourceSlot] = oldDestination;
            _targetInventory.InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, _targetSlot));
            InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, _sourceSlot));
            return _targetInventory.Items[_targetSlot] == oldSource && Items[_sourceSlot] == oldDestination;
        }

        /// TODO: Referencing items from different inventorys also won't allow to the chance to raise events, this will need to be event based. fuck.
        public bool SwapSlots(int SlotSource, int SlotDest)
        {
            bool _success = false;
            InventoryItem oldDestination = new InventoryItem(Items[SlotDest]);
            InventoryItem oldSource = new InventoryItem(Items[SlotSource]);
            if(oldDestination!=null && oldSource!=null)
            {
                Items[SlotSource] = oldDestination;
                Items[SlotDest] = oldSource;
                _success = Items[SlotSource] == oldDestination && Items[SlotDest] == oldSource;
                InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, SlotSource));
                InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, SlotDest));
            }
            return _success;
        }

        public bool CombineItems(int SlotSource, int SlotDest)
        {
            bool _success = false; 
            if(Items[SlotDest].ID == Items[SlotSource].ID)
            {
                int oldQty = Items[SlotDest].Quantity;
                int newQuantity = ignoreItemLimit ? Items[SlotSource].Quantity + Items[SlotDest].Quantity : Math.Min(Items[SlotDest].Limit, Items[SlotSource].Quantity + Items[SlotDest].Quantity);
                Items[SlotDest].Quantity = newQuantity;
                Items[SlotSource].Quantity = Items[SlotSource].Quantity - newQuantity;
                _success = Items[SlotDest].Quantity == newQuantity;
                InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, SlotSource));
                InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, SlotDest));
            }
            return _success;
        }

        public bool RemoveItem(int id, int qty)
        {
            bool _success = false;

            int _remaining = qty;
            (bool _exists, int[] _indices) = ItemExists(id);
            if (_exists)
            {
                foreach(int _index in _indices)
                {
                    int _toSubtract= Math.Min(qty, Items[_index].Quantity);
                    Items[_index].Quantity -= _toSubtract;
                    _remaining -= _toSubtract;
                    InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, _index));
                }
                _success = _remaining == 0;
            }
            else
            {
                _success = true;
            }

            return _success;
        }

        public void UpdateItemQuantity(int Slot)
        {
            if(Items[Slot].Quantity<=0)
            {
                Items[Slot] = null;
                InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.DELETE, Slot));
            }
        }

        public void TestPrintInventory()
        {
            Debug.Log(Items.Length);
            Debug.Log(NextEmpty);
            Debug.Log(Size);
            for(int i=0;i< Size;i++)
            {
                Debug.Log(i);
                if(Items[i]!=null)
                {
                    Debug.Log($"{Items[i].Name} x {Items[i].Quantity}");
                }
            }
        }
    }
}

