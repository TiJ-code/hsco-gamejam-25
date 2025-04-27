using Godot;
using System;

public partial class ProjectileCollider : RigidBody2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        for (int i = 0; i < state.GetContactCount(); i++)
        {
            Node collider = state.GetContactColliderObject(i) as Node;
            if (collider != null)
            {
                GD.Print("Hit something: " + collider.Name);

                // Optionally, you can check if it's a TileMap specifically
                if (collider is TileMapLayer)
                {
                    GD.Print("Hit a TileMap!");
                }

                QueueFree(); // Destroy the projectile
                break;
            }
        }
    }


    public void OnBodyEntered(Node body)
    {
        QueueFree();
    }
}
