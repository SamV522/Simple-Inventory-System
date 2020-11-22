using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInventorySystem;

public class Testing : MonoBehaviour
{
    public Inventory inventory;
    public Inventory inventory2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Generating Items...");
            ItemDatabase.GenerateItems();
            Debug.Log($"Is Generated: {ItemDatabase.IsGenerated}");
            Debug.Log($"Generated {ItemDatabase.Items.Count} items");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log($"Adding {ItemDatabase.FetchItemByID(0).Name} to inventory slot {inventory2.NextEmpty}");
            inventory2.AddItem(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"Adding {ItemDatabase.FetchItemByID(1).Name} to inventory {inventory}");
            inventory.AddItem(1, 1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log($"Exchanging {inventory.Items[0]?.Name??"Nothing"} with {inventory2.Items[0]?.Name??"Nothing"} from {inventory2}");
            inventory.ExchangeItems(0, inventory2, 0);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Removing {ItemDatabase.FetchItemByID(1).Name} from inventory");
            inventory.RemoveItem(1, 1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(ItemDatabase.Items.Count);
            Debug.Log(ItemDatabase.Items[0].Name);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            inventory.TestPrintInventory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ItemDatabase.TestPrintDatabase();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ItemDatabase.TestSaveDatabase();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(JsonUtility.ToJson(ItemDatabase.Items[0]));
        }
    }
}
