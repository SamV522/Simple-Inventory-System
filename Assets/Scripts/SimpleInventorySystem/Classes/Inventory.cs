﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleInventorySystem;

namespace SimpleInventorySystem
{
    /// TODO: This should have create/add/remove/update/delete
    public enum InventoryEventType
    {
        DELETE,
        UPDATE,
        CREATE
    }

    public class InventoryEventArgs : EventArgs
    {
        public InventoryEventType InventoryEventType;
        public int? Slot;

        public InventoryEventArgs(InventoryEventType _InventoryEvent, int? _Slot = null)
        {
            InventoryEventType = _InventoryEvent;
            Slot = _Slot; 
        }
    }

    public class Inventory : MonoBehaviour
    {
        public int maxSize = 9;
        public InventoryItem[] Items { get; private set; }
        public int Size { get { return Items.Count(x => x != null); } }
        public bool IsFull => Size >= maxSize;
        public int NextEmpty => Array.IndexOf(Items, null);
        public int CountEmptySlots => Items.Count(x => x == null);
        public int ItemLimit = 100;
        public bool ignoreItemLimit = false;

        public event EventHandler<InventoryEventArgs> InventoryEvent;

        private void Awake()
        {
            Items = new InventoryItem[maxSize];
            InventoryEvent += e_InventoryEventHandler;
        }

        private void e_InventoryEventHandler(object sender, InventoryEventArgs e)
        {
            /// TODO: sender may be from another inventory, this might need to have a check to make sure the sender is allowed to change this inventory.
            /// REMINDER: sender may be from another inventory 
            switch (e.InventoryEventType)
            {
                case InventoryEventType.CREATE:
                    break;
                case InventoryEventType.UPDATE:
                    if(e.Slot.HasValue && Items[e.Slot.Value].Quantity<=0)
                    {
                        SetItem(e.Slot.Value, null);
                    }
                    break;
                case InventoryEventType.DELETE:
                    break;
                default:
                    // ???
                    break;
            }
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

        public (bool,int) HasIngredients(Item item, int Qty)
        {
            bool retBool = false;
            int retInt = 0;
            foreach(KeyValuePair<string,int> ingredient in item.Ingredients)
            {
                Item _ingredientItem = ItemDatabase.FetchItemByName(ingredient.Key);
                (bool hasQty, int invQty) = HasItemQuantity(_ingredientItem.ID, ingredient.Value * Qty);
                if (hasQty)
                {
                    retBool = true;
                    retInt = retInt == 0 ? invQty / ingredient.Value : retInt;
                    int newInt = Math.Min(retInt, Math.Min(Qty, ingredient.Value * Qty));
                    retInt = retInt > newInt ? newInt : retInt;
                }
                else
                {
                    retBool = false;
                    retInt = 0;
                    return (retBool, retInt);
                }
            }
            return (retBool,retInt);
        }

        public (bool,int) HasItemQuantity(int id, int qty)
        {
            int _qty = 0;
            foreach(int idx in AllIndicesOf(id))
            {
                _qty += Items[idx].Quantity;
            }
            return (_qty >= qty, _qty);
        }

        public (bool,int[]) ItemExists(int id)
        {
            int[] _index = AllIndicesOf(id);
            return (_index.Length>0, _index);
        }

        public (bool, int[], int) ItemsWithSpaceFor(int id)
        {
            List<int> Indices = new List<int>();
            int spaces = 0;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null && Items[i].ID == id && !Items[i].IsFull)
                {
                    Indices.Add(i);
                    spaces += Items[i].RemainingSpace;
                }
            }
            return (Indices.Count>0,Indices.ToArray(), spaces);
        }

        public (bool, int) CanFit(int id, int qty)
        {
            (bool _hasItems, _, int _qty) = ItemsWithSpaceFor(id);
            if(!_hasItems || _qty < qty)
            {
                return (SlotsToFit(id, qty) <= CountEmptySlots, qty);
            }
            else
            {
                return (_hasItems || !IsFull, _qty);
            }
        }

        public int SlotsToFit(int id, int qty)
        {
            int _stackLimit = ItemDatabase.FetchItemByID(id).Limit;
            return Math.Min(1, (qty % _stackLimit));
        }

        public bool AddItem(int id, int qty)
        {
            bool _success = false;
            int _remaining = qty;
            (bool _exists, int[] _indices, _) = ItemsWithSpaceFor(id);
            if (_exists)
            {
                foreach (int _index in _indices)
                {
                    // Quantity DIFFERENCE Limit 
                    int _toAdd = ignoreItemLimit ? qty : Math.Min(_remaining, Items[_index].RemainingSpace);
                    Items[_index].Quantity += _toAdd;
                    _remaining -= _toAdd;
                    InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, _index));
                }
                _success = _remaining == 0;
            }else if (!IsFull)
            {
                int _requiredSlots = SlotsToFit(id, qty);
                if(_requiredSlots <= CountEmptySlots)
                {
                    for (int i = 0; i < SlotsToFit(id, qty); i++)
                    {

                        int targetSlot = NextEmpty;
                        SetItem(targetSlot, new InventoryItem(ItemDatabase.FetchItemByID(id), 
                                                            ignoreItemLimit ? qty : Math.Min(qty, ItemDatabase.FetchItemByID(id).Limit)));
                        InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, targetSlot));
                    }
                    _success = true;
                }
            }
            return _success;
        }

        public void SetItem(int index, InventoryItem inventoryItem)
        {
            Items[index] = inventoryItem;
            if (inventoryItem == null) return;
            inventoryItem.Inventory = this;
        }

        public bool ExchangeItems(int _sourceSlot, Inventory _targetInventory, int _targetSlot)
        {
            InventoryItem oldSource = new InventoryItem(Items[_sourceSlot])??null;
            InventoryItem oldDestination = new InventoryItem(_targetInventory.Items[_targetSlot])??null;
            _targetInventory.SetItem(_targetSlot, oldSource);
            SetItem(_sourceSlot, oldDestination);
            _targetInventory.InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, _targetSlot));
            InventoryEvent?.Invoke(this, new InventoryEventArgs(InventoryEventType.UPDATE, _sourceSlot));
            return _targetInventory.Items[_targetSlot] == oldSource && Items[_sourceSlot] == oldDestination;
        }

        public bool SwapSlots(int SlotSource, int SlotDest)
        {
            bool _success = false;

            // This is prone to errors, since the constructor 
            InventoryItem oldDestination = new InventoryItem(Items[SlotDest]);
            InventoryItem oldSource = new InventoryItem(Items[SlotSource]);
            
            if(oldDestination!=null && oldSource!=null)
            {
                SetItem(SlotSource, oldDestination);
                SetItem(SlotDest, oldSource);
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
                    int _toSubtract= Math.Min(_remaining, Items[_index].Quantity);
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

        public void TestPrintInventory()
        {
            for(int i=0;i< Size;i++)
            {
                if(Items[i]!=null)
                {
                    Debug.Log($"{Items[i].Name} x {Items[i].Quantity}");
                }
            }
        }
    }
}

