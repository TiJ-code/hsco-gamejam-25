using Godot;
using System;

public partial class CloseCombatEnemy : CharacterBody2D
{
    CharacterBody2D player;
    [Export] private int health;
    [Export] private int damage;
    [Export] private float speed = 3.5f;

    public override void _Ready()
    {
        health = 100;
        damage = 10;
    }

    public override void _Process(double delta)
    {

        if (player != null)
        {
            FollowPlayer();
        }
        else
        {
            player = GetNode<CharacterBody2D>("../player");
        }

    }

    private void FollowPlayer()
    {
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

}