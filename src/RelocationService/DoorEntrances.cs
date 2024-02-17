//// naming convention is as follows: _LevelName_TargetDoorNodeName_[X | Y]
public static class DoorEntrances
{
	public class EmptyLocation : DoorEntrance
	{
		public int SceneId => (int)Enumerations.Scenes.Empty;
		public int X => 0;
		public int Y => 0;
	}
}
