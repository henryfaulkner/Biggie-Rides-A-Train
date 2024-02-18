using Godot;
using System;
using Newtonsoft.Json;

public partial class SaveStateContext : Node, IDisposable
{
	private static readonly string _SAVE_STATE_FILE = "res://SaveStateContext/SaveState.json";

	public void Commit(SaveStateModel saveState)
	{
		GD.Print("Commit");
		try
		{
			string content = JsonConvert.SerializeObject(saveState, Formatting.Indented);
			using var file = FileAccess.Open(_SAVE_STATE_FILE, FileAccess.ModeFlags.Write);
			file.StoreString(content);
		}
		catch (Exception exception)
		{
			GD.Print($"Commit exception: {exception}");
			GD.Print($"Commit SaveState: {saveState}");
		}
	}

	public SaveStateModel Load()
	{
		try
		{
			GD.Print("Load");
			using var file = FileAccess.Open(_SAVE_STATE_FILE, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			var json = JsonConvert.DeserializeObject<SaveStateModel>(content);
			return json;
		}
		catch (Exception exception)
		{
			GD.Print($"Load exception: {exception}");
		}
		return new SaveStateModel();
	}
	
	public void Clear()
	{
		GD.Print("Clear");
		Commit(new SaveStateModel());
	}
}
