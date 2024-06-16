using Godot;
using System;
using Newtonsoft.Json;

public partial class SaveStateService : Node, IDisposable
{
	private static readonly StringName _SAVE_STATE_FILE = new StringName("res://Core/SaveStateService/SaveState.json");
	//private LoggingService _serviceLogger = null;

	public SaveStateService()
	{
		//_serviceLogger = GetNode<LoggingService>("/root/LoggingService");
	}

	public void Commit(SaveStateModel saveState)
	{
		//_serviceLogger.LogDebug("Commit");
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
			//_serviceLogger.LogError($"Commit exception: {exception}");
			//_serviceLogger.LogError($"Commit SaveState: {saveState}");
		}
	}

	public SaveStateModel Load()
	{
		try
		{
			GD.Print("Load");
			//_serviceLogger.LogDebug("Load");
			using var file = FileAccess.Open(_SAVE_STATE_FILE, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			var json = JsonConvert.DeserializeObject<SaveStateModel>(content);
			return json;
		}
		catch (Exception exception)
		{
			//_serviceLogger.LogError($"Load exception: {exception}");
		}
		return new SaveStateModel();
	}

	public void Clear()
	{
		//_serviceLogger.LogDebug("Clear");
		Commit(new SaveStateModel());
	}
}
