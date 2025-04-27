using Godot;
using System;

public partial class BarControl : Control
{
	
	[Export] private uint maxprogressbarvalue = 100;
	[Export] private Label label; 
	[Export] private ProgressBar progressBar;
	[Export] private string write;



	public override void _Ready()
	{
		UpdateDreamstabilityUI();
		progressBar.MaxValue = maxprogressbarvalue;
		progressBar.Value = maxprogressbarvalue;
		label.Text = write;
	}


	private void UpdateDreamstabilityUI()
	{
		SetDream_Label();
		SetDream_Stability();
	}

	private void SetDream_Label()
	{
		label.Text = $"Dreamstability: {progressBar.Value}"; 
	}

	private void SetDream_Stability()
	{
		progressBar.Value = progressBar.Value;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			Damage();
		}
	}

	private void Damage()
	{
		progressBar.Value -= 1;
		if (progressBar.Value < 0)
		{
			progressBar.Value = maxprogressbarvalue;
		}
		UpdateDreamstabilityUI();
	}
}
