// Damit du auf Godot-spezifische Dinge wie Texture2D zugreifen kannst
using Godot;
// Für Listen und andere Sammlungstypen
using System.Collections.Generic;

// Definiert die möglichen Typen von Items
public enum ItemTyp
{
	Consumable, // Verbrauchsgegenstand (z.B. Heiltrank)
	Artefakt,   // Artefakt (z.B. besondere Waffen, Ausrüstung)
	QuestItem   // Quest-Gegenstand (z.B. Schlüssel, Brief)
}

// Beschreibt ein Item im Spiel
public class Item
{
	// Name des Items (z.B. "Heiltrank")
	public string Name { get; set; }

	// Menge des Items (wieviele Exemplare, z.B. 5 Heiltränke)
	public int Menge { get; set; }

	// Bild des Items (z.B. Icon für UI)
	public Texture2D Icon { get; set; }

	// Beschreibung des Items (z.B. "Stellt 50 HP wieder her.")
	public string Beschreibung { get; set; }

	// Der Typ des Items (Consumable, Artefakt oder QuestItem)
	public ItemTyp Typ { get; set; }

	// Konstruktor: damit kann man ein Item erstellen
	public Item(string name, int menge, Texture2D icon, string beschreibung, ItemTyp typ)
	{
		Name = name;
		Menge = menge;
		Icon = icon;
		Beschreibung = beschreibung;
		Typ = typ;
	}
}

// Das Inventar des Spielers, enthält alle Items
public class Inventar
{
	// Liste aller Items, die der Spieler besitzt
	public List<Item> Items { get; private set; } = new List<Item>();

	// Methode um ein Item hinzuzufügen
	public void Add(Item neuesItem)
	{
		// Wenn das Item ein Consumable ist, dann stacken
		if (neuesItem.Typ == ItemTyp.Consumable)
		{
			// Suchen ob es schon ein Item mit gleichem Namen gibt
			Item vorhandenesItem = Items.Find(item => item.Name == neuesItem.Name);

			if (vorhandenesItem != null)
			{
				// Wenn gefunden, Menge erhöhen
				vorhandenesItem.Menge += neuesItem.Menge;
			}
			else
			{
				// Wenn nicht gefunden, neues Item zur Liste hinzufügen
				Items.Add(neuesItem);
			}
		}
		else
		{
			// Für Artefakte und Questitems: nur hinzufügen, wenn es noch nicht existiert
			bool existiertSchon = Items.Exists(item => item.Name == neuesItem.Name);

			if (!existiertSchon)
			{
				Items.Add(neuesItem);
			}
		}
	}

	// Methode um ein Item zu entfernen
	public void Remove(Item itemToRemove)
	{
		Items.Remove(itemToRemove);
	}
}
