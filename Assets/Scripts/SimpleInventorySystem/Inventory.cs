using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleInventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public int maxSize = 9;
        public InventoryItem[] Items { get; private set; }
        public int Size { get { return Items.Count(x => x != null); } }
        public bool IsFull => Size >= maxSize;
        public int NextEmpty => Array.IndexOf(Items, null);
        public int ItemLimit = 100;
        public bool ignoreItemLimit = false;

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

        /// TODO: Referencing items from different inventorys also won't allow to the chance to raise events, this will need to be event based. fuck.
        public bool SwapItems(ref InventoryItem SourceItem, ref InventoryItem DestinationItem)
        {
            InventoryItem oldDestination = new InventoryItem(DestinationItem);
            InventoryItem oldSource = new InventoryItem(SourceItem);
            DestinationItem = new InventoryItem(SourceItem);
            SourceItem = oldDestination;
            return SourceItem == oldDestination && DestinationItem == oldSource;
        }

        public bool CombineItems(ref InventoryItem SourceItem, ref InventoryItem DestinationItem)
        {
            bool _success = false;
            if(SourceItem.ID == DestinationItem.ID)
            {
                //DestinationItem.Quantity = ignoreItemLimit ? SourceItem.Quantity + 
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
                }
            }
            else
            {
                _success = true;
            }
            _success = _remaining == 0;

            return _success;
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

