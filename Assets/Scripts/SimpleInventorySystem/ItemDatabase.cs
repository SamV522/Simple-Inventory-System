using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleInventorySystem
{
    public static class ItemDatabase
    {
        public static List<Item> Items = new List<Item>();
        public static int DefaultItemLimit = 10;
        public static bool IsGenerated { get; private set; } = false;

        public static void GenerateItems()
        {
            /// TODO: generate list of items from JSON here.
            Items = JsonMapper.ToObject<List<Item>>(File.ReadAllText(Application.dataPath + "/Resources/ItemDB.json"));
            int i = 0;
            foreach(Item item in Items)
            {
                item.ID = i;
                Debug.Log($"{item.ID}: {item.Name}");
                i++;
            }
            IsGenerated = true;
        }

        public static Item FetchItemByID(int ID)
        {
            return Items[ID];
        }
    }
}

