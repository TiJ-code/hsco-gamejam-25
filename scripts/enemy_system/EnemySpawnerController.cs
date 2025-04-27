using Godot;
using System;

public partial class EnemySpawnerController : Node2D
{

    public override void _Ready()
    {
        CreateEnemySpawnerInRoom(100, 10, Vector2.One, 15 * 32, 6 * 32);
    }

    public override void _Process(double delta)
    {
    }

    public void CreateEnemySpawnerInRoom(int enderDragonBossBar, int roomIndex, Vector2 roomPosition, int roomWidth, int roomHeight)
    {
        PackedScene enemySpawnerScene = GD.Load<PackedScene>("res://scenes/prefabs/enemy_system/enemy_spawner.tscn");
        EnemySpawner enemySpawnerInstance = enemySpawnerScene.Instantiate<EnemySpawner>();
        int enemyCount = calculateEnemyCount(roomIndex);

        GD.Print($"Enemy count for room {roomIndex}: {enemyCount}");

        enemySpawnerInstance.InitiateEnemySpawner(roomIndex, roomWidth / 2, roomHeight, enemyCount);
        enemySpawnerInstance.GlobalPosition = roomPosition + new Vector2(3 * (roomWidth / 4), -(roomHeight / 2));
        AddChild(enemySpawnerInstance);

    }

    public int calculateEnemyCount(int roomIndex)
    {
        return roomIndex * 15;
    }
}