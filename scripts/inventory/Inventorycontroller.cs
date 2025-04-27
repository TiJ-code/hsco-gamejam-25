using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryController : Control
{
    [Export] private VBoxContainer itemContainer; 

    private List<InventoryItem> inventory = new List<InventoryItem>();

    public override void _Ready()
    {
        inventory.Add(new InventoryItem { Name = "Heiltrank", ImagePath = "res://assets/Healpotion.png", Quantity = 3 });
        
        DisplayInventory(); 
    }

    private void DisplayInventory()
    {
        foreach (Node child in itemContainer.GetChildren())
        {
            child.QueueFree();
        }


        foreach (InventoryItem item in inventory)
        {
            HBoxContainer itemRow = new HBoxContainer(); 
            
            TextureRect itemImage = new TextureRect();
            itemImage.Texture = (Texture2D)GD.Load<Texture2D>(item.ImagePath); 
            itemImage.CustomMinimumSize = new Vector2(16, 16); 

            Label itemLabel = new Label();
            itemLabel.Text = $"{item.Name} (x{item.Quantity})"; 

            
            itemRow.AddChild(itemImage);
            itemRow.AddChild(itemLabel);

            itemContainer.AddChild(itemRow);
        }
    }
}


public class InventoryItem
{
    public string Name { get; set; } 
    public string ImagePath { get; set; } 
    public int Quantity { get; set; } 
}
