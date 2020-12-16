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
        public static int DefaultProductionOutput = 1;
        public static bool IsGenerated { get; private set; } = false;
        public static bool GenerateIDs { get; private set; } = true;

        public static void GenerateItems()
        {
            TextAsset itemDb = Resources.Load("ItemDB") as TextAsset;
            Items = JsonMapper.ToObject<List<Item>>(itemDb.text);
            if(GenerateIDs)
            {
                int i = 0;
                foreach (Item item in Items)
                {
                    item.ID = i;
                    i++;
                }
            }
            IsGenerated = true;
        }

        #region Item Fetching
        public static Item FetchItemByID(int ID)
        {
            return Items[ID];
        }

        public static Item FetchItemByName(string Name)
        {
            Item retItem = null;
            foreach(Item item in Items)
            {
                if(item.Name == Name)
                {
                    return item;
                }
            }
            return retItem;
        }
        #endregion

        #region Fetching Recipes
        public static List<Item> FetchRecipesByFactory(string FactoryType)
        {
            List<Item> retItems = new List<Item>();
            foreach (Item _item in Items)
            {
                if (_item.Craftable && _item.RequiredFactory == FactoryType)
                {
                    //Item is craftable at the right factory.
                    retItems.Add(_item);
                }
            }
            return retItems;
        }

        public static List<Item> FetchRecipesByItem(Item item)
        {
            List<Item> retItems = new List<Item>();
            foreach (Item _item in retItems)
            {
                if (_item.Craftable)
                {
                    //Item is craftable at the right factory.
                    foreach (KeyValuePair<string, int> _ingredient in _item.Ingredients)
                    {
                        if (_ingredient.Key == item.Name)
                        {
                            // item is an ingredient of _item
                            retItems.Add(_item);
                        }
                    }
                }
            }
            return retItems;
        }

        public static List<Item> FetchRecipesByItem(Item item, string FactoryType)
        {
            List<Item> retItems = new List<Item>();
            foreach (Item _item in retItems)
            {
                if (_item.Craftable && _item.RequiredFactory == FactoryType)
                {
                    //Item is craftable at the right factory.
                    foreach (KeyValuePair<string, int> _ingredient in _item.Ingredients)
                    {
                        if (_ingredient.Key == item.Name)
                        {
                            // item is an ingredient of _item
                            retItems.Add(_item);
                        }
                    }
                }
            }
            return retItems;
        }
        #endregion

        #region Testing
        public static void TestSaveDatabase()
        {
            File.WriteAllText(Application.dataPath + "/Resources/Test_ItemDB.json", JsonMapper.ToJson(Items));
        }

        public static void TestPrintDatabase()
        {
            Debug.Log(Items[0].LimitType);
        }
        #endregion
    }
}

