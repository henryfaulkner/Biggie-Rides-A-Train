using Godot;
using System;

public partial class RelocationService : Node
{
	public RelocationService() { }

	public int GetStoredLocationSceneId()
	{
		return GetStoredLocation().SceneId;
	}

	public DoorEntrance GetStoredLocation()
	{
		DoorEntrance result = null;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			result = contextState.StoredLocation;
		}
		return result;
	}

	public bool IsStoredLocationEmpty()
	{
		DoorEntrance result = null;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			result = contextState.StoredLocation;
		}
		return result.SceneId == (int)Enumerations.Scenes.Empty;
	}

	public bool IsStoredLocationEmpty(DoorEntrance storedLocation)
	{
		return storedLocation.SceneId == (int)Enumerations.Scenes.Empty;
	}

	public void Clear()
	{
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			contextState.StoredLocation = null;
			context.Commit(contextState);
		}
	}

	#region SETTERS

	public void SetState_EmptyLocation()
	{
		//////GD.Print("SetState_EmptyLocation");
		DoorEntrance doorEntrance = new DoorEntrance();
		SetTargetDoorEntranceState(doorEntrance);
	}

	public void SetState_EmptyLocation(int x, int y)
	{
		//////GD.Print("SetState_StoredLocation");
		var location = new DoorEntrance(x, y);
		SetTargetDoorEntranceState(location);
	}

	public void SetLocation(int x, int y, int z = 0)
	{
		//GD.Print($"SetLocation x:{x} y:{y} z:{z}");
		var location = new DoorEntrance(Enumerations.Scenes.Empty, x, y, z);
		//GD.Print($"SetLocation location.x:{location.X} location.y:{location.Y} location.z:{location.Z}");
		SetTargetDoorEntranceState(location);
	}

	public void SetLocation(Enumerations.Scenes scene, int x, int y, int z = 0)
	{
		//GD.Print($"SetLocation x:{x} y:{y} z:{z}");
		var location = new DoorEntrance(scene, x, y, z);
		//GD.Print($"SetLocation location.x:{location.X} location.y:{location.Y} location.z:{location.Z}");
		SetTargetDoorEntranceState(location);
	}

	public void SetState_OutsideStation(int x, int y)
	{
		//////GD.Print("SetState_OutsideStation");
		var location = new DoorEntrance(Enumerations.Scenes.OutsideStation, x, y);
		SetTargetDoorEntranceState(location);
	}

	public void SetState_MainStation(int x, int y)
	{
		//////GD.Print("SetState_MainStation");
		var location = new DoorEntrance(Enumerations.Scenes.MainStation, x, y);
		SetTargetDoorEntranceState(location);
	}

	public void SetState_TherapistOffice(int x, int y)
	{
		//////GD.Print("SetState_TherapistOffice");
		var location = new DoorEntrance(Enumerations.Scenes.TherapistOffice, x, y);
		SetTargetDoorEntranceState(location);
	}

	public void SetState_Club(int x, int y)
	{
		//////GD.Print("SetState_Club");
		var location = new DoorEntrance(Enumerations.Scenes.Club, x, y);
		SetTargetDoorEntranceState(location);
	}

	#endregion

	private void SetTargetDoorEntranceState(DoorEntrance targetDoorEntrance)
	{
		//////GD.Print("SetTargetDoorEntranceState");
		try
		{
			using (var context = new SaveStateService())
			{
				//GD.Print($"SetTargetDoorEntranceState targetDoorEntrance.x:{targetDoorEntrance.X} targetDoorEntrance.y:{targetDoorEntrance.Y} targetDoorEntrance.z:{targetDoorEntrance.Z}");
				var contextState = context.Load();
				contextState.StoredLocation = targetDoorEntrance;
				context.Commit(contextState);
			}
		}
		catch (Exception exception)
		{
			//////GD.Print($"SetTargetDoorEntranceState exception: {exception}");
			//////GD.Print($"SetTargetDoorEntranceState exception.Message: {exception.Message}");
		}
	}
}
