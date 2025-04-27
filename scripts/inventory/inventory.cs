using Godot;
using System;
using System.Collections.Generic;

public class Inventory
{
    public List<Item> Items {get; set;}
    public void Add(Item newItem){
        if (newItem.Type == ItemType.Consumable){
            Item existingItem = Items.Find(item => item.Name == newItem.Name);
            
            if (existingItem != null){
                existingItem.Quantity += newItem.Quantity;
            }
            else {
                Items.Add(newItem);
            }
        }
        else {
            bool AlreadyExisting = Items.Exists(Item => Item.Name == newItem.Name);

            if (!AlreadyExisting){
                Items.Add(newItem)
            }
        }
    }
    public void Remove(Item itemToRemove){
        Items.Remove(itemToRemove);
    }
}


