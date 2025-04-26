using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

namespace hscogamejam25.scripts.map
{
    public partial class MapGenerator : TileMapLayer
    {
        private readonly Vector2I[] atlasCoordinates =
        {
            new Vector2I(0, 0), // floor
            new Vector2I(1, 0), // wall
            new Vector2I(2, 0) // door
        };
        
        [Export] private int roomCount = 2;
        [Export] private int minRoomSize = 4;
        [Export] private int maxRoomSize = 8;

        private TileSet tileSet;
        private int atlasSourceId;
        private Random random = new Random();
        private List<Room> rooms = new List<Room>();

        public override void _Ready()
        {
            tileSet = GetTileSet();
            atlasSourceId = tileSet.GetSourceId(0);

            GenerateDungeon();
        }

        private void GenerateDungeon()
        {
            const int roomWidth = 10;
            
            Room spawnRoom = new Room(0, 0, roomWidth, 7);
            spawnRoom.AddDoor(Room.Side.Right);
            GenerateRoom(spawnRoom);

            for (int i = 0; i < roomCount; i++)
            {
                int newRoomX = roomWidth + (roomWidth + 1) * i + 1;

                Room newRoom = new Room(newRoomX, 0, roomWidth, random.Next(7, 15));
                newRoom.AddDoor(Room.Side.Left);
                newRoom.AddDoor(Room.Side.Right);
                
                GenerateRoom(newRoom);
            }
        }

        private void GenerateRoom(Room room)
        {
            int roomEndX = room.X + room.Width;
            int roomEndY = room.Y + room.Height;
            int roomEndXIdx = roomEndX - 1;
            int roomEndYIdx = roomEndY - 1;

            for (int x = room.X; x < roomEndX; x++)
            {
                for (int y = room.Y; y < roomEndY; y++)
                {
                    Vector2I position = new Vector2I(x, y);
            
                    // Correct grouping of border conditions
                    if (x == room.X || x == roomEndXIdx || y == room.Y || y == roomEndYIdx)
                    {
                        SetCell(position, atlasSourceId, atlasCoordinates[1]); // Border tile
                    }
                    else
                    {
                        SetCell(position, atlasSourceId, atlasCoordinates[0]); // Floor tile
                    }
                }
            }

            if (!room.HasDoor()) return;

            foreach (Door door in room.Doors)
            {
                if (door == null) continue;

                Vector2I doorPosition = new Vector2I(door.X, door.Y);
                SetCell(doorPosition, atlasSourceId, atlasCoordinates[2], (int)door.Side);
            }
        }

    }
}
