using Godot;
using System;

public partial class RelocationService : Node
{
	public RelocationService() { }

	public AbstractDoorEntrance GetStoredLocation()
	{
		AbstractDoorEntrance result = null;
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			result = contextState.StoredLocation;
		}
		return result;
	}

	public bool IsStoredLocationEmpty()
	{
		AbstractDoorEntrance result = null;
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

	public void SetState_EmptyLocation()
	{
		AbstractDoorEntrance doorEntrance = GetEmptyLocation();
		SetTargetDoorEntranceState(doorEntrance);
	}

	public void SetState_OutsideStation_MainStationDoor()
	{
		AbstractDoorEntrance doorEntrance = GetOutsideStation_MainStationDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}

	public void SetState_MainStation_MainEntranceDoor()
	{
		AbstractDoorEntrance doorEntrance = GetMainStation_MainEntranceDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}

	public void SetState_MainStation_ClubDoor()
	{
		AbstractDoorEntrance doorEntrance = GetMainStation_ClubDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}

	public void SetState_MainStation_TherapistOfficeDoor()
	{
		AbstractDoorEntrance doorEntrance = GetMainStation_TherapistOfficeDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}

	public void SetState_TherapistOffice_MainStationDoor()
	{
		AbstractDoorEntrance doorEntrance = GetTherapistOffice_MainStationDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}

	private AbstractDoorEntrance GetEmptyLocation() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.EmptyLocation);
	private AbstractDoorEntrance GetOutsideStation_MainStationDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.OutsideStation_MainStationDoor);
	private AbstractDoorEntrance GetMainStation_MainEntranceDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.MainStation_MainEntranceDoor);
	private AbstractDoorEntrance GetMainStation_ClubDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.MainStation_ClubDoor);
	private AbstractDoorEntrance GetMainStation_TherapistOfficeDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.MainStation_TherapistOfficeDoor);
	private AbstractDoorEntrance GetTherapistOffice_MainStationDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.TherapistOffice_MainStationDoor);

	private AbstractDoorEntrance GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances enumRef)
	{
		AbstractDoorEntrance result = null;
		switch (enumRef)
		{
			case DoorEntrancesEnumeration.DoorEntrances.EmptyLocation:
				result = new DoorEntrances.EmptyLocation();
				break;
			case DoorEntrancesEnumeration.DoorEntrances.OutsideStation_MainStationDoor:
				result = new DoorEntrances.OutsideStation_MainStationDoor();
				break;
			case DoorEntrancesEnumeration.DoorEntrances.MainStation_MainEntranceDoor:
				result = new DoorEntrances.MainStation_MainEntranceDoor();
				break;
			case DoorEntrancesEnumeration.DoorEntrances.MainStation_ClubDoor:
				result = new DoorEntrances.MainStation_ClubDoor();
				break;
			case DoorEntrancesEnumeration.DoorEntrances.MainStation_TherapistOfficeDoor:
				result = new DoorEntrances.MainStation_TherapistOfficeDoor();
				break;
			case DoorEntrancesEnumeration.DoorEntrances.TherapistOffice_MainStationDoor:
				result = new DoorEntrances.TherapistOffice_MainStationDoor();
				break;
			default:
				break;
		}
		return result;
	}

	private void SetTargetDoorEntranceState(AbstractDoorEntrance targetDoorEntrance)
	{
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			contextState.StoredLocation = (DoorEntrance)targetDoorEntrance;
			context.Commit(contextState);
		}
	}
}
