using Godot;

namespace hscogamejam25.scripts.map;

public class Door
{
    public int X { get; set; }
    public int Y { get; set; }
    public Room.Side Side { get; private set; }

    public Door TargetDoor { get; set; }

    public Door(int x, int y, Room.Side side)
    {
        X = x;
        Y = y;
        Side = side;
    }

    public bool IsTeleportingDirectionLeft()
    {
        return X - TargetDoor.X < 0;
    }
}