using Godot;
using System;

public partial class CloseCombatEnemy : CharacterBody2D
{
    CharacterBody2D player;
    [Export] private int health;
    [Export] private int damage;
    [Export] private float speed = 2f;
    private Random random = new Random();

    public override void _Ready()
    {
        health = 100;
        damage = 10;
        speed += random.Next(-1, 1) * random.NextSingle();
    }

    public override void _Process(double delta)
    {
        if (player != null && IsInstanceValid(player))
        {
            FollowPlayer();
        }
        else
        {
            // Attempt to find the player node globally
            player = GetTree().Root.GetNodeOrNull<CharacterBody2D>("/root/Node2D/player");
            if (player == null)
            {
                GD.PrintErr("Player node not found in the scene tree.");
            }
        }
    }

    private void FollowPlayer()
    {
        if (player == null || !IsInstanceValid(player))
        {
            return; // Exit if the player is invalid or null
        }

        Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
        Vector2 velocity = direction * speed * (float)GetProcessDeltaTime() * 1000;

        Velocity = velocity;
        MoveAndSlide();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            QueueFree();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void IncreaseDamage(int amount)
    {
        damage *= amount;
    }

    public void IncreaseHealth(int amount)
    {
        health *= amount;
    }

}