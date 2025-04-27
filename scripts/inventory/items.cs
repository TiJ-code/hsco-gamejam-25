using Godot;
using System;


public enum ItemType{
    Consumable, Artefact, QuestItem
}

public class Item{
    public string Name {get; set;}
    public int Quantity {get; set;}
    public Texture2D Icon {get; set;}
    public string Description {get; set;}
    public ItemType Type {get; set;}

    public Item(string name, int quantity, Texture2D icon, string description, ItemType type){
        Name = name;
        Quantity = quantity;
        Icon = icon;
        Description = description;
        Type = type;
    }
}
