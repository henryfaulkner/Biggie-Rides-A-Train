// naming convention is as follows: _LevelName_TargetDoorNodeName_[X | Y]
public static class DoorEntrances
{
	#region OutsideStation

	public class OutsideStation_MainStationDoor : IDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.OutsideStation_MainStationDoor;
		public override DoorEntrancesEnumeration.DoorEntrances TargetDoorEntrance => _TARGET_DOOR_ENTRANCE;
		private const int _X = 69;
		public override int X => _X;
		private const int _Y = 420;
		public override int Y => _Y;
	}

	#endregion

	#region MainStation

	public class MainStation_ClubDoor : IDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.MainStation_ClubDoor;
		public override DoorEntrancesEnumeration.DoorEntrances TargetDoorEntrance => _TARGET_DOOR_ENTRANCE;
		private const int _X = 0;
		public override int X => _X;
		private const int _Y = 0;
		public override int Y => _Y;
	}

	public class MainStation_TherapistOfficeDoor : IDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.MainStation_TherapistOfficeDoor;
		public override DoorEntrancesEnumeration.DoorEntrances TargetDoorEntrance => _TARGET_DOOR_ENTRANCE;
		private const int _X = 0;
		public override int X => _X;
		private const int _Y = 0;
		public override int Y => _Y;
	}

	#endregion

	#region TherapistOffice

	public class TherapistOffice_MainStationDoor : IDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.TherapistOffice_MainStationDoor;
		public override DoorEntrancesEnumeration.DoorEntrances TargetDoorEntrance => _TARGET_DOOR_ENTRANCE;
		private const int _X = 0;
		public override int X => _X;
		private const int _Y = 0;
		public override int Y => _Y;
	}

	#endregion
}
