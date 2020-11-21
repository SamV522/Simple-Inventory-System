using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SimpleInventorySystem
{
    public class Item : Collection<Item>
    {
        public int ID { get; set; }
        public string Name { get; private set; }
        public string Desc { get; private set; } = "There is no description";
        public float Weight { get; private set; } = 0.0f;
        public int Limit { get; private set; } = ItemDatabase.DefaultItemLimit;
        public string SpritePath { get; private set; }
        public string RequiredFactory { get; private set; }
        public Dictionary<string, int> Ingredients { get; protected set; }
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
            Desc = _Desc;
        }

        public Item(int _ID, string _Name, string _Desc, float _Weight)
        {
            ID = _ID;
            Name = _Name;
            Desc = _Desc;
            Weight = _Weight;
        }

        public Item(int _ID, string _Name, string _Desc, float _Weight, int _Limit)
        {
            ID = _ID;
            Name = _Name;
            Desc = _Desc;
            Weight = _Weight;
            Limit = _Limit;
        }
    }
}