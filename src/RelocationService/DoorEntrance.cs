public class DoorEntrance
{
	public DoorEntrance() 
	{ 
		Id = (int)Enumerations.Scenes.Empty;
		X = 0;
		Y = 0;
	}

	public DoorEntrance(int x, int y)
	{
		Id = (int)Enumerations.Scenes.Custom;
		X = x;
		Y = y;
	}

	public int Id { get; set; }
	public int X { get; set; }
	public int Y { get; set; }
}
