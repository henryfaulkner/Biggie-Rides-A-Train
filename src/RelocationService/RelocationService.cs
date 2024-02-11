using Godot;
using System;

public partial class RelocationService : Node
{
	public RelocationService() { }
	
	
	public IDoorEntrance GetOutsideStation_MainStationDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.OutsideStation_MainStationDoor);
	public IDoorEntrance GetMainStation_ClubDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.MainStation_ClubDoor);
	public IDoorEntrance GetMainStation_TherapistOfficeDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.MainStation_TherapistOfficeDoor);
	public IDoorEntrance GetTherapistOffice_MainStationDoor() => GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances.TherapistOffice_MainStationDoor);

	public void SetState_OutsideStation_MainStationDoor() 
	{
		IDoorEntrance doorEntrance = GetOutsideStation_MainStationDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}
	
	public void SetState_MainStation_ClubDoor() 
	{ 
		IDoorEntrance doorEntrance = GetMainStation_ClubDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}
	
	public void SetState_MainStation_TherapistOfficeDoor() 
	{ 
		IDoorEntrance doorEntrance = GetMainStation_TherapistOfficeDoor();
		SetTargetDoorEntranceState(doorEntrance);
	}
	
	public void SetState_TherapistOffice_MainStationDoor() 
	{
		IDoorEntrance doorEntrance = GetTherapistOffice_MainStationDoor();
		SetTargetDoorEntranceState(doorEntrance);
	} 

	private IDoorEntrance GetDoorEntrance(DoorEntrancesEnumeration.DoorEntrances enumRef)
	{
		IDoorEntrance result = null;

		switch (enumRef)
		{
			case DoorEntrancesEnumeration.DoorEntrances.OutsideStation_MainStationDoor:
				result = new DoorEntrances.OutsideStation_MainStationDoor();
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
	
	private void SetTargetDoorEntranceState(IDoorEntrance targetDoorEntrance)
	{
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			contextState.TargetDoorEntrance = targetDoorEntrance;
			context.Commit(contextState);
		}
	}
}
