# Simple-Inventory-System

## About:
Simple Inventory framework for Unity, no UI included yet, allowing you to rig up your UI any way you like.

## Use:
1. Setup your ItemDB.Json in Resources/ItemDB.json as per the included examples.
2. Somewhere at the start of your gameloop initialize ItemDatabase.GenerateItems() to generate the list of items from the json file.
2. Add an Inventory component to a GameObject
3. Use the included functions to manipulate the inventory.
    * AddItem - add an item the inventory.
    * RemoveItem - remove quantity an item from the inventory.
    * ExchangeItem - Exchange the item in specified slot with an item from the specified slot of the target inventory.
    * SwapItem - swap items within the current inventory.
## Note:
There are a couple functions left in for testing the inventory/itemdatabase:
* Inventory.TestPrintInventory()   - as expected
* ItemDatabase.TestPrintDatabase() - as expected
* ItemDatabase.TestSaveDatabase()  - will attempt to save a new Json file in the resources folder as Test_ItemDB.json with all of the items in the database as they were generated.
