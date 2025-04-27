using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	private const float Speed = 50f;
	private const int MaxHealth = 100;
	private int currentHealth = MaxHealth;
	
	[Export] private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback stateMachine;

	private float damageCooldown = 0.2f; // Cooldown in seconds
	private float lastDamageTime = -1.0f; // Tracks the last time damage was taken

	public override void _Ready()
	{
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		currentHealth = MaxHealth;
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

		CheckForEnemyCollision();
	}

	private void CheckForEnemyCollision()
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		var query = new PhysicsShapeQueryParameters2D
		{
			Shape = GetNode<CollisionShape2D>("CollisionShape2D").Shape,
			Transform = GlobalTransform,
			Motion = Velocity * (float)GetPhysicsProcessDeltaTime() // Account for player movement
		};

		var results = spaceState.IntersectShape(query);
		foreach (var result in results)
		{
			if (result.TryGetValue("collider", out var colliderVariant))
			{
				var colliderNode = colliderVariant.As<Node>();
				if (colliderNode is CloseCombatEnemy enemy)
				{
					if (CanTakeDamage())
					{
						TakeDamage(enemy.GetDamage());
						lastDamageTime = Time.GetTicksMsec() / 1000.0f; // Update the last damage time
					}
				}
			}
		}
	}

	private bool CanTakeDamage()
	{
		return (Time.GetTicksMsec() / 1000.0f) - lastDamageTime >= damageCooldown;
	}

	private void TakeDamage(int damage)
	{
		currentHealth -= damage;
		GD.Print($"Player took {damage} damage. Current health: {currentHealth}");

		if (currentHealth <= 0)
		{
			GD.Print("Player has died.");
			GetTree().Quit(); // Exit the tree and close the application
		}
	}
}
