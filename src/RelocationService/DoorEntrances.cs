// naming convention is as follows: _LevelName_TargetDoorNodeName_[X | Y]
public static class DoorEntrances
{
	#region Free

	public class EmptyLocation : AbstractDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.EmptyLocation;
		public override DoorEntrancesEnumeration.DoorEntrances Id => _TARGET_DOOR_ENTRANCE;
		private const int _X = 0;
		public override int X => _X;
		private const int _Y = 0;
		public override int Y => _Y;
	}

	#endregion

	#region OutsideStation

	public class OutsideStation_MainStationDoor : AbstractDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.OutsideStation_MainStationDoor;
		public override DoorEntrancesEnumeration.DoorEntrances Id => _TARGET_DOOR_ENTRANCE;
		private const int _X = 256;
		public override int X => _X;
		private const int _Y = 94;
		public override int Y => _Y;
	}

	#endregion

	#region MainStation

	public class MainStation_MainEntranceDoor : AbstractDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.MainStation_MainEntranceDoor;
		public override DoorEntrancesEnumeration.DoorEntrances Id => _TARGET_DOOR_ENTRANCE;
		private const int _X = 1050;
		public override int X => _X;
		private const int _Y = 523;
		public override int Y => _Y;
	}

	public class MainStation_ClubDoor : AbstractDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.MainStation_ClubDoor;
		public override DoorEntrancesEnumeration.DoorEntrances Id => _TARGET_DOOR_ENTRANCE;
		private const int _X = 3125;
		public override int X => _X;
		private const int _Y = 700;
		public override int Y => _Y;
	}

	public class MainStation_TherapistOfficeDoor : AbstractDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.MainStation_TherapistOfficeDoor;
		public override DoorEntrancesEnumeration.DoorEntrances Id => _TARGET_DOOR_ENTRANCE;
		private const int _X = 1806;
		public override int X => _X;
		private const int _Y = -996;
		public override int Y => _Y;
	}

	#endregion

	#region TherapistOffice

	public class TherapistOffice_MainStationDoor : AbstractDoorEntrance
	{
		private const DoorEntrancesEnumeration.DoorEntrances _TARGET_DOOR_ENTRANCE = DoorEntrancesEnumeration.DoorEntrances.TherapistOffice_MainStationDoor;
		public override DoorEntrancesEnumeration.DoorEntrances Id => _TARGET_DOOR_ENTRANCE;
		private const int _X = 1055;
		public override int X => _X;
		private const int _Y = 976;
		public override int Y => _Y;
	}

	#endregion
}
