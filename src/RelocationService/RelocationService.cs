using Godot;
using System;

public partial class RelocationService : Node
{
	public RelocationService() { }

	public DoorEntrance GetStoredLocation()
	{
		DoorEntrance result = null;
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			result = contextState.StoredLocation;
		}
		return result;
	}

	public bool IsStoredLocationEmpty()
	{
		DoorEntrance result = null;
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			result = contextState.StoredLocation;
		}
		return result.Id == GetEmptyLocation().Id;
	}

	public bool IsStoredLocationEmpty(DoorEntrance storedLocation)
	{
		return storedLocation.Id == GetEmptyLocation().Id;
	}

	public void ClearStoredLocation()
	{
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			contextState.StoredLocation = (DoorEntrance)GetEmptyLocation();
			context.Commit(contextState);
		}
	}

	public void SetState_StoredLocation(int x, int y)
	{
		GD.Print("SetState_StoredLocation");
		var location = new DoorEntrance(x, y);
		SetTargetDoorEntranceState(location);
	}
	
	public void SetState_EmptyLocation()
	{
		GD.Print("SetState_EmptyLocation");
		DoorEntrance doorEntrance = GetEmptyLocation();
		SetTargetDoorEntranceState(doorEntrance);
	}
	
	private DoorEntrance GetEmptyLocation() => GetDoorEntrance(Enumerations.Scenes.Empty);
	
	private DoorEntrance GetDoorEntrance(Enumerations.Scenes enumRef)
	{
		DoorEntrance result = null;
		switch (enumRef)
		{
			case Enumerations.Scenes.Empty:
				result = new DoorEntrances.EmptyLocation();
				break;
			default:
				break;
		}
		return result;
	}

	private void SetTargetDoorEntranceState(DoorEntrance targetDoorEntrance)
	{
		GD.Print("SetTargetDoorEntranceState");
		try
		{
			using (var context = new SaveStateContext())
			{
				var contextState = context.Load();
				contextState.StoredLocation = (DoorEntrance)targetDoorEntrance;
				context.Commit(contextState);
			}
		}
		catch (Exception exception)
		{
			GD.Print($"SetTargetDoorEntranceState exception: {exception}");
			GD.Print($"SetTargetDoorEntranceState exception.Message: {exception.Message}");
		}
	}
}
