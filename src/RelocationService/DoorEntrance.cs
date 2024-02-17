public class DoorEntrance
{
	public DoorEntrance()
	{
		SceneId = (int)Enumerations.Scenes.Empty;
		X = 0;
		Y = 0;
	}

	public DoorEntrance(int x, int y)
	{
		SceneId = (int)Enumerations.Scenes.Empty;
		X = x;
		Y = y;
	}

	public DoorEntrance(Enumerations.Scenes scene, int x, int y)
	{
		SceneId = (int)scene;
		X = x;
		Y = y;
	}

	public int SceneId { get; set; }
	public int X { get; set; }
	public int Y { get; set; }
}
