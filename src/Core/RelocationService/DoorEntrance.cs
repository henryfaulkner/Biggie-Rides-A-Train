public class DoorEntrance
{
	public int SceneId { get; set; }
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }

	public DoorEntrance()
	{
		SceneId = (int)Enumerations.Scenes.Empty;
		X = 0;
		Y = 0;
		Z = 0;
	}

	public DoorEntrance(float x, float y, float z = 0)
	{
		SceneId = (int)Enumerations.Scenes.Empty;
		X = x;
		Y = y;
		Z = z;
	}

	public DoorEntrance(Enumerations.Scenes scene, float x, float y, float z = 0)
	{
		SceneId = (int)scene;
		X = x;
		Y = y;
		Z = z;
	}
}
