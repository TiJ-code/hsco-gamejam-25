using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class MapGenerator : TileMapLayer
{
    private readonly Vector2I[] _atlasCoordinates =
    {
        new Vector2I(0, 0), // floor
        new Vector2I(1, 0), // wall
        new Vector2I(2, 0) // door
    };

    [Export] private int _roomCount = 6;

    public List<Room> Rooms { get; private set; } = new List<Room>();

    public List<DoorInteractor> Doors { get; private set; } = new List<DoorInteractor>();
    private readonly Random _random = new Random();

    private TileSet _tileSet;
    private int _atlasSourceId;
    private Vector2I _halfTileSize;

    public override void _Ready()
    {
        _tileSet = TileSet;
        _atlasSourceId = _tileSet.GetSourceId(0);
        _halfTileSize = _tileSet.TileSize / 2;
    }

    public void GenerateDungeon()
    {
        const int roomWidth = 15;

        // 1. Create all rooms first
        Room startRoom = new Room(0, 0, 0, roomWidth, 10, Room.RoomType.Start);
        Rooms.Add(startRoom);
        GenerateRoom(startRoom);

        int lastViableIndex = _roomCount - 1;
        for (int i = 0; i < _roomCount; i++)
        {
            int newRoomX = roomWidth + (roomWidth + 6) * i + 5;

            Room newRoom = (i == lastViableIndex)
                ? new Room(Rooms.Count, newRoomX, 0, roomWidth, 10, Room.RoomType.End)
                : new Room(Rooms.Count, newRoomX, 0, roomWidth, _random.Next(12, 22), Room.RoomType.Normal);

            Rooms.Add(newRoom);
            GenerateRoom(newRoom);
        }

        List<Door> doors = new List<Door>();
        foreach (Room room in Rooms)
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

        Vector2 pixelPosition = MapToLocal(new Vector2I(door.X, door.Y)) - _halfTileSize;

        node.Position = pixelPosition;

        Doors.Add(node);

        AddChild(node);
    }
}