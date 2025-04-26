namespace hscogamejam25.scripts.map;

public class Door
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Room.Side Side { get; private set; }

    public Door(int x, int y, Room.Side side)
    {
        X = x;
        Y = y;
        Side = side;
    }
}