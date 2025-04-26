using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

namespace hscogamejam25.scripts.map
{
    public partial class MapGenerator : TileMapLayer
    {
        [Export] private int roomCount = 1;
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
            const int roomWidth = 10, roomHeight = 7;
            
            Room spawnRoom = new Room(0, 0, roomWidth, roomHeight);
            spawnRoom.AddDoor(Room.Side.Right);
            GenerateRoom(spawnRoom);

            for (int i = 0; i < roomCount; i++)
            {
                int newRoomX = roomWidth * i + 1;

                Room newRoom = new Room(newRoomX, 0, roomWidth, roomHeight);
                newRoom.AddDoor(Room.Side.Left);
                newRoom.AddDoor(Room.Side.Right);
                
                GenerateRoom(newRoom);
            }
        }

        private void GenerateRoom(Room room)
        {
            Array<Vector2I> cells = new Array<Vector2I>();
            
            for (int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    cells.Add(new Vector2I(x, y));
                }
            }
            
            SetCellsTerrainConnect(cells, 0, 0, false);

            if (!room.HasDoor()) return;
            
            Vector2I doorPosition = new Vector2I(room.Door.X, room.Door.Y);
            SetCell(doorPosition, atlasSourceId, new Vector2I(1, 3));
        }
    }
}
