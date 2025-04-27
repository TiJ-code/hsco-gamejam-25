using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export] private float _speed = 50f;
	[Export] private AnimationTree _animationTree;
	private AnimationNodeStateMachinePlayback _stateMachine;

	public override void _Ready()
	{
		_stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		direction = direction.Normalized();
		if (direction != Vector2.Zero)
		{
			velocity = direction * _speed;
			
			_stateMachine.Travel("walk");
			_animationTree.Set("parameters/idle/BlendSpace2D/blend_position", direction);
			_animationTree.Set("parameters/walk/BlendSpace2D/blend_position", direction);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, _speed);
			
			_stateMachine.Travel("idle");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
