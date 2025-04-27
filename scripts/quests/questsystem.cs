using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

public partial class questsystem : Control
{
    [Export] private Sprite2D questlabel;
    [Export] private VBoxContainer questList;
    [Export] private Panel questPanel;

    private string[] quests = new string[] 
    {
        "Bringe das Schwert zurück...", 
        "Schalte die Maschine an",
        "Überlebe den Raum"
    };
    private Node child;


    public override void _Ready()
    {
        questPanel.Visible = false;
    }

    public void OnQuestSymbolMouseEntered(){
        ShowQuests();
    }

    private void ShowQuests(){
        questPanel.Visible = true;

        foreach (Node Child in questList.GetChildren()){
            questList.RemoveChild(child);
            child.QueueFree();
        }

        foreach (string quest in quests){
            Label questOption = new Label();
            questOption.Text = quest;
            questList.AddChild(questOption);
        }
    }

    private void HideQuests(){
        questPanel.Visible = false;
    }

    

}
