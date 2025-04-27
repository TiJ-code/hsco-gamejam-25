using System;

namespace hscogamejam25.scripts.map
{
    public class Room
    {
        public enum Side
        {
            Right, Left
        }

        public enum RoomType
        {
            Start, Normal, Hub, End
        }
        
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public Door LeftDoor { get; protected set; }
        public Door RightDoor { get; protected set; }
        public RoomType Type { get; protected set; }

        public Room(int x, int y, int width, int height, RoomType type)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = type;
        }

        public bool HasDoor()
        {
            return LeftDoor != null || RightDoor != null;
        }

        public void AddDoor(Side side)
        {
            int doorY = 2;
            if (side == Side.Left)
                LeftDoor = new Door(X, doorY, side);
            else
                RightDoor = new Door(X + Width - 1, doorY, side);
        }
    }
}