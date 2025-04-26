using System;

namespace hscogamejam25.scripts.map
{
    public class Room
    {
        public enum Side
        {
            Left, Right
        }
        
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Door Door { get; private set; }

        public Room(int x, int y, int width, int height)
            : this(x, y, width, height, null)
        {
        }

        private Room(int x, int y, int width, int height, Door door)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Door = door;
        }

        public bool HasDoor()
        {
            return Door != null;
        }

        public void AddDoor(Side side)
        {
            int doorX = (side == Side.Left) ? 0 : Width;
            int doorY = 2; // Always y = 2

            Door = new Door(doorX, doorY);
        }
    }
}