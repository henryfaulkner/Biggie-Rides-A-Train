using Godot;
using System;
using Newtonsoft.Json;

public class SaveStateService
{
	private static readonly string _SAVE_STATE_FILE = "res://SaveStateService/SaveState.json";

	public void Save(SaveStateModel saveState)
	{
		GD.Print("Save");
		try
		{
			string content = JsonConvert.SerializeObject(saveState, Formatting.Indented);
			using var file = FileAccess.Open(_SAVE_STATE_FILE, FileAccess.ModeFlags.Write);
			file.StoreString(content);
		}
		catch (Exception exception)
		{
			GD.Print($"Save exception: {exception}");
		}
	}

	public SaveStateModel Load()
	{
		try
		{
			GD.Print("Load");
			using var file = FileAccess.Open(_SAVE_STATE_FILE, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			return JsonConvert.DeserializeObject<SaveStateModel>(content);
		}
		catch (Exception exception)
		{
			GD.Print($"Load exception: {exception}");
		}
		return new SaveStateModel();
	}
}