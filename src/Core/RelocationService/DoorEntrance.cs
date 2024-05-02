public class DoorEntrance
{
	public int SceneId { get; set; }
	public int X { get; set; }
	public int Y { get; set; }
	public int Z { get; set; }

	public DoorEntrance()
	{
		SceneId = (int)Enumerations.Scenes.Empty;
		X = 0;
		Y = 0;
		Z = 0;
	}

	public DoorEntrance(int x, int y, int z = 0)
	{
		SceneId = (int)Enumerations.Scenes.Empty;
		X = x;
		Y = y;
		Z = z;
	}

	public DoorEntrance(Enumerations.Scenes scene, int x, int y, int z = 0)
	{
		SceneId = (int)scene;
		X = x;
		Y = y;
		Z = z;
	}
}
