using Godot;
using System;

public partial class DoorInteractor : Area2D
{
    private const int InteractorPixelOffset = 7;
    private const int TileSize = 32;

    [Export] private CollisionShape2D _collisionShape;
    public Door DoorContainer { get; set; }
        
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is PlayerController player)
        {
            GD.Print("Teleporting to new room!");

            if (DoorContainer != null)
            {
                const int yOffset = TileSize / 2;
                
                int xOffset;
                if (DoorContainer.Side == Room.Side.Left)
                {
                    xOffset = -InteractorPixelOffset;
                }
                else
                {
                    xOffset = TileSize + InteractorPixelOffset;
                }
                    
                player.Position = new Vector2(
                    DoorContainer.TargetDoor.X * TileSize + xOffset,
                    DoorContainer.TargetDoor.Y * TileSize + yOffset
                );
            }
        }
    }

    public CollisionShape2D GetCollisionShape()
    {
        return _collisionShape;
    }
}
