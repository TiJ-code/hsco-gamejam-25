using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	private const int MaxHealth = 100;
	private int currentHealth = MaxHealth;
	
	[Export] private float _speed = 50f;
	[Export] private AnimationTree _animationTree;
	[Export] private Sprite2D _pointer;
	private AnimationNodeStateMachinePlayback _stateMachine;
	
	private float damageCooldown = 0.2f; // Cooldown in seconds
	private float lastDamageTime = -1.0f; // Tracks the last time damage was taken

	public override void _Ready()
	{
		_stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
		currentHealth = MaxHealth;
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateCursor();
		EvaluateMovement();
		EvaluateAttack();
		CheckForEnemyCollision();
	}

	private void EvaluateMovement()
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

	private void EvaluateAttack()
	{
		bool attackMeleePressed = Input.IsActionJustPressed("attack_melee");
		if (attackMeleePressed)
		{
			AttackMelee();	
		}

		bool attackDistancePressed = Input.IsActionJustPressed("attack_distance");
		if (attackDistancePressed && !attackMeleePressed)
		{
			AttackDistance();
		}
	}

	private void AttackMelee()
	{
		RigidBody2D meleeSprite = SpawnAttackSpriteRigid(true);
		GetParent().AddChild(meleeSprite);
		meleeSprite.GlobalPosition = GlobalPosition;
		
		// Get direction to mouse
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 toMouse = (mousePosition - GlobalPosition).Normalized();

		// Slightly offset the spawn position (e.g. 16 pixels forward)
		Vector2 spawnOffset = toMouse * 16;
		meleeSprite.GlobalPosition = GlobalPosition + spawnOffset;

		// Make sure no gravity affects it
		meleeSprite.GravityScale = 0;

		// Rotate the sprite to face the direction
		meleeSprite.Rotation = toMouse.Angle();

		// Set a constant velocity
		meleeSprite.LinearVelocity = toMouse * 100;

		// Timer to delete melee sprite after short time
		Timer timer = new Timer();
		timer.WaitTime = .75f;
		timer.OneShot = true;
		timer.Timeout += () => meleeSprite.QueueFree();
		meleeSprite.AddChild(timer);
		timer.Start();
	}

	private void AttackDistance()
	{
		RigidBody2D distanceSprite = SpawnAttackSpriteRigid(false);
		GetParent().AddChild(distanceSprite);

		// Get direction to mouse
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 toMouse = (mousePosition - GlobalPosition).Normalized();

		// Slightly offset the spawn position (e.g. 16 pixels forward)
		Vector2 spawnOffset = toMouse * 16;
		distanceSprite.GlobalPosition = GlobalPosition + spawnOffset;

		// Make sure no gravity affects it
		distanceSprite.GravityScale = 0;

		// Rotate the sprite to face the direction
		distanceSprite.Rotation = toMouse.Angle();

		// Set a constant velocity
		distanceSprite.LinearVelocity = toMouse * 400;
		
		// Timer to delete melee sprite after short time
		Timer timer = new Timer();
		timer.WaitTime = 2.5f;
		timer.OneShot = true;
		timer.Timeout += () => distanceSprite.QueueFree();
		distanceSprite.AddChild(timer);
		timer.Start();
	}

	public RigidBody2D SpawnAttackSpriteRigid(bool melee)
	{
		PackedScene attackScene;
		if (melee)
			attackScene = GD.Load<PackedScene>("res://scenes/prefabs/player/attack_sprite_melee.tscn");
		else
			attackScene = GD.Load<PackedScene>("res://scenes/prefabs/player/attack_sprite_distance.tscn");
		RigidBody2D rigid = attackScene.Instantiate<RigidBody2D>();
		return rigid;
	}


	private void UpdateCursor()
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 toMouse = mousePosition - GlobalPosition;
		_pointer.Rotation = toMouse.Angle();
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
