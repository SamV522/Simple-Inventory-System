using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SimpleInventorySystem
{
    public enum ItemLimitType
    {
        Item,
        Inventory,
        Global,
        None
    }

    public class Item
    {

        public int ID { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; } = "There is no description";
        public float Weight { get; private set; } = 0.0f;
        public int Limit { get; private set; } = ItemDatabase.DefaultItemLimit;
        public ItemLimitType LimitType { get; private set; } = ItemLimitType.Global;
        public string SpritePath { get; private set; }
        public string RequiredFactory { get; private set; }
        public Dictionary<string, int> Ingredients { get; protected set; }
        public int ProductionOutput = ItemDatabase.DefaultProductionOutput;
        public bool Craftable => Ingredients != null;

        public Item()
        {
            ID = ItemDatabase.Items.Count;
            Name = "Base Item";
        }

        public Item(int _ID, string _Name)
        {
            ID = _ID;
            Name = _Name;
        }

        public Item(int _ID, string _Name, string _Desc)
        {
            ID = _ID;
            Name = _Name;
            Description = _Desc;
        }

        public Item(int _ID, string _Name, string _Desc, float _Weight)
        {
            ID = _ID;
            Name = _Name;
            Description = _Desc;
            Weight = _Weight;
        }

        public Item(int _ID, string _Name, string _Desc, float _Weight, int _Limit)
        {
            ID = _ID;
            Name = _Name;
            Description = _Desc;
            Weight = _Weight;
            Limit = _Limit;
        }

        public Item(int _ID, string _Name, string _Desc, float _Weight, int _Limit, int _ProductionOutput)
        {
            ID = _ID;
            Name = _Name;
            Description = _Desc;
            Weight = _Weight;
            Limit = _Limit;
            ProductionOutput = _ProductionOutput;
        }
    }
}