using System;

namespace hscogamejam25.scripts.map
{
    public class Room
    {
        public enum Side
        {
            Right, Left
        }
        
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Door[] Doors { get; private set; }

        public Room(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Doors = new Door[2];
        }

        public bool HasDoor()
        {
            if (Doors == null) return false;
            foreach (Door door in Doors)
            {
                if (door != null) return true;
            }

            return true;
        }

        public void AddDoor(Side side)
        {
            int nextDoorIndex = 0;
            for (int i = 0; i < Doors.Length; i++)
            {
                if (Doors[i] == null) nextDoorIndex = i;
            }
            
            int doorX = 0;
            if (side == Side.Left)
                doorX = 0;
            else
                doorX = Width - 1;
            
            int doorY = 2; // Always y = 2

            Doors[nextDoorIndex] = new Door(doorX + X, doorY + Y, side);
        }
    }
}