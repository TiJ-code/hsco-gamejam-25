using Godot;
using System;

namespace hscogamejam25.scripts.map
{
    public partial class DoorInteractor : Area2D
    {
        private const int TileSize = 32;
        
        public Door DoorContainer { get; set; }
        
        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node2D body)
        {
            if (body is PlayerMovement player)
            {
                GD.Print("Teleporting to new room!");

                if (DoorContainer != null)
                {
                    int multiplier = DoorContainer.Side == Room.Side.Right ? 1 : -1;
                    
                    player.Position = new Vector2(
                        DoorContainer.TargetDoor.X * TileSize + ((TileSize + 2)* multiplier),
                        DoorContainer.TargetDoor.Y * TileSize + TileSize / 2
                    );
                }
            }
        }
    }
}
