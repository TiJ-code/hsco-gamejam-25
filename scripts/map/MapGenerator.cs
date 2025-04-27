using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

namespace hscogamejam25.scripts.map
{
    public partial class MapGenerator : TileMapLayer
    {
        private readonly Vector2I[] _atlasCoordinates =
        {
            new Vector2I(0, 0), // floor
            new Vector2I(1, 0), // wall
            new Vector2I(2, 0)  // door
        };
        
        [Export] private int roomCount = 3;

        private TileSet _tileSet;
        private int _atlasSourceId;
        private Vector2I _half_tile_size;
        private readonly Random _random;
        private List<Room> rooms;

        public MapGenerator()
        {
            _random = new Random();
            rooms = new List<Room>();
        }
        
        public override void _Ready()
        {
            _tileSet = TileSet;
            _atlasSourceId = _tileSet.GetSourceId(0);
            _half_tile_size = _tileSet.TileSize / 2;

            GenerateDungeon();
        }

        private void GenerateDungeon()
        {
            const int roomWidth = 15;
    
            // 1. Create all rooms first
            Room startRoom = new Room(0, 0, roomWidth, 10, Room.RoomType.Start);
            rooms.Add(startRoom);
            GenerateRoom(startRoom);

            int lastViableIndex = roomCount - 1;
            for (int i = 0; i < roomCount; i++)
            {
                int newRoomX = roomWidth + (roomWidth + 6) * i + 5;

                Room newRoom = (i == lastViableIndex)
                    ? new Room(newRoomX, 0, roomWidth, 10, Room.RoomType.End)
                    : new Room(newRoomX, 0, roomWidth, _random.Next(12, 22), Room.RoomType.Normal);

                rooms.Add(newRoom);
                GenerateRoom(newRoom);
            }

            List<Door> doors = new List<Door>();
            foreach (Room room in rooms)
            {
                if (room.Type == Room.RoomType.End || room.Type == Room.RoomType.Normal)
                {
                    Door door = new Door(room.X, 2, Room.Side.Left); 
                    doors.Add(door);
                    PlaceDoor(door);
                }
                
                if (room.Type == Room.RoomType.Start || room.Type == Room.RoomType.Normal)
                {
                    Door door = new Door(room.X + room.Width - 1, 2, Room.Side.Right);
                    doors.Add(door);
                    PlaceDoor(door);
                }
            }

            for (int i = 0; i < doors.Count - 1; i += 2)
            {
                Door currentDoor = doors[i];
                Door nextDoor = doors[i + 1];

                currentDoor.TargetDoor = nextDoor;
                nextDoor.TargetDoor = currentDoor;
                
                CreateDoorCollider(currentDoor);
                CreateDoorCollider(nextDoor);
            }
        }

        private void PlaceDoor(Door door)
        {
            SetCell(new Vector2I(door.X, door.Y), _atlasSourceId, _atlasCoordinates[2], (int)door.Side);
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
            
                    if (x == room.X || x == roomEndXIdx || y == room.Y || y == roomEndYIdx)
                        SetCell(position, _atlasSourceId, _atlasCoordinates[1]); // Wall
                    else
                        SetCell(position, _atlasSourceId, _atlasCoordinates[0]); // Floor
                }
            }
        }

        private void CreateDoorCollider(Door door)
        {
            if (door == null) return;

            PackedScene doorScene = GD.Load<PackedScene>("res://scenes/prefabs/map/door_interactor.tscn");
            DoorInteractor node = doorScene.Instantiate<DoorInteractor>();
            node.DoorContainer = door;
            
            Vector2 pixelPosition = MapToLocal(new Vector2I(door.X, door.Y)) - _half_tile_size;

            node.Position = pixelPosition;

            AddChild(node);
        }
    }
}
