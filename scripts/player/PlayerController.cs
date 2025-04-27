using Godot;
using System;
using System.Collections.Generic; // <-- Damit wir Listen benutzen können

// Enum für Itemtypen
public enum ItemType
{
	Consumable,
	Artifact,
	QuestItem
}

// Item-Klasse
public class Item
{
	public string Name { get; set; }
	public int Menge { get; set; }
	public Texture2D Icon { get; set; }
	public string Beschreibung { get; set; }
	public ItemType Typ { get; set; }

	public Item(string name, int menge, Texture2D icon, string beschreibung, ItemType typ)
	{
		Name = name;
		Menge = menge;
		Icon = icon;
		Beschreibung = beschreibung;
		Typ = typ;
	}
}

// PlayerController Klasse
public partial class PlayerController : CharacterBody2D
{
	// Geschwindigkeit des Spielers
	private const float Speed = 50f;
	
	// Für Animationen
	[Export] private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback stateMachine;

	// Inventar des Spielers
	private List<Item> inventar = new List<Item>();

	public override void _Ready()
	{
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		direction = direction.Normalized();
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
			
			stateMachine.Travel("walk");
			animationTree.Set("parameters/idle/BlendSpace2D/blend_position", direction);
			animationTree.Set("parameters/walk/BlendSpace2D/blend_position", direction);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			
			stateMachine.Travel("idle");
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	// Methode zum Hinzufügen eines Items
	public void AddItem(Item neuesItem)
	{
		if (neuesItem.Typ == ItemType.Consumable)
		{
			// Consumables stacken
			foreach (Item item in inventar)
			{
				if (item.Name == neuesItem.Name)
				{
					item.Menge += neuesItem.Menge;
					return;
				}
			}
			// Wenn es noch nicht existiert, neu hinzufügen
			inventar.Add(neuesItem);
		}
		else
		{
			// Artefakte und Questitems dürfen nur einmal existieren
			foreach (Item item in inventar)
			{
				if (item.Name == neuesItem.Name)
				{
					GD.Print("Item existiert bereits im Inventar!");
					return;
				}
			}
			inventar.Add(neuesItem);
		}
	}

	// Methode zum Entfernen eines Items
	public void RemoveItem(Item itemToRemove)
	{
		if (inventar.Contains(itemToRemove))
		{
			inventar.Remove(itemToRemove);
		}
	}

	// Optional: Inventar ausgeben
	public void PrintInventar()
	{
		GD.Print("Inventar:");
		foreach (Item item in inventar)
		{
			GD.Print($"{item.Name} x{item.Menge}");
		}
	}
}
