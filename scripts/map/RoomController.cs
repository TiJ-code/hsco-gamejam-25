using Godot;
using System;
using System.Collections.Generic;

public partial class RoomController : Node
{
    public List<DoorInteractor> DoorInteractors { get; set; } 

    public void OpenRoom(int roomIndex)
    {
        int maxDoorIndex = roomIndex * 2 - 1;
        for (int i = 0; i < maxDoorIndex; i++)
        {
            DoorInteractors[i].GetCollisionShape().Disabled = false;
        }
    }

    public void LockRooms()
    {
        foreach (DoorInteractor interactor in DoorInteractors)
        {
            interactor.GetCollisionShape().Disabled = true;
        }
    }
}
