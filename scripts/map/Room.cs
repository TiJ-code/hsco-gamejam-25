using System;

public class Room
{
    public enum Side
    {
        Right, Left
    }

    public enum RoomType
    {
        Start, Normal, End
    }

    public int Index { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public RoomType Type { get; private set; }
    
    public Room(int index, int x, int y, int width, int height, RoomType type)
    {
        Index = index;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Type = type;
    }
}