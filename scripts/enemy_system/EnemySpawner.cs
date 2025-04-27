using Godot;
using System;

public partial class EnemySpawner : Area2D
{
    private Random random = new Random();
    [Export] private int pendingEnemiesToSpawn = 0;
    [Export] private CollisionShape2D spawnArea;
    private int roomIndex;

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        if (pendingEnemiesToSpawn > 0)
        {
            SpawnEnemies(pendingEnemiesToSpawn);
        }
    }

    // not tested yet
    public void InitiateEnemySpawner(int roomIndex2, int spawnAreaWidth, int spawnAreaHeight, int enemyCount)
    {
        GD.Print($"Initiating enemy spawner with width: {spawnAreaWidth}, height: {spawnAreaHeight}, enemy count: {enemyCount}");
        if (spawnArea.Shape is RectangleShape2D rectangleShape)
        {
            rectangleShape.Size = new Vector2(spawnAreaWidth, spawnAreaHeight);
        }
        else
        {
            GD.PrintErr("Spawn area is not a RectangleShape2D.");
            return;
        }

        roomIndex = roomIndex2;
        RequestSpawnEnemies(enemyCount);
    }

    public void RequestSpawnEnemies(int count)
    {
        pendingEnemiesToSpawn += count;
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPosition;
            bool positionFound = false;

            for (int attempts = 0; attempts < 10; attempts++) // Limit attempts to find a valid position
            {
                spawnPosition = new Vector2(
                    random.Next((int)GlobalPosition.X, (int)(GlobalPosition.X + GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.X)) - GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.X / 2,
                    random.Next((int)GlobalPosition.Y, (int)(GlobalPosition.Y + GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.Y)) - GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.Y / 2
                );
                GD.Print($"Attempting to spawn enemy at {spawnPosition}");
                if (!IsOverlapping(spawnPosition))
                {
                    positionFound = true;
                    SpawnEnemy(spawnPosition);
                    pendingEnemiesToSpawn--;
                    break;
                }
            }

            if (!positionFound)
            {
                GD.Print("No valid position found for enemy spawn.");
                break;
            }
        }

        GD.Print("Enemies spawned");

    }

    private bool IsOverlapping(Vector2 position)
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var result = spaceState.IntersectPoint(new PhysicsPointQueryParameters2D { Position = position });
        return result.Count > 0;
    }

    public void SpawnEnemy(Vector2 position)
    {
        // Load the enemy scene
        PackedScene enemyScene = GD.Load<PackedScene>("res://scenes/prefabs/enemy_system/enemy_types/close_combat_enemy.tscn");

        // Instance the enemy
        CloseCombatEnemy enemyInstance = enemyScene.Instantiate<CloseCombatEnemy>();

        // Set the position of the enemy
        enemyInstance.GlobalPosition = position;

        // Increase enemy attributes
        enemyInstance.IncreaseDamage(roomIndex / 2);
        enemyInstance.IncreaseHealth(roomIndex / 2);

        // Add the enemy to the scene tree
        GetTree().Root.AddChild(enemyInstance);
    }

}