using Godot;

public partial class GameController : Node2D
{
    [ExportGroup("Game Controller")]
    [Export] private MapGenerator _mapGenerator;

    [Export] private RoomController _roomController;

    [ExportGroup("Player based")] 
    [Export] private PlayerController _player;

    public override void _Ready()
    {
        _mapGenerator.GenerateDungeon();
        _roomController.DoorInteractors = _mapGenerator.Doors;
        _roomController.OpenRoom(1);
    }

    public override void _Process(double delta) {}
}
