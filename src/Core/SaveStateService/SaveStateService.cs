using Godot;
using System;
using Newtonsoft.Json;

public partial class SaveStateService : Node, IDisposable
{
	private static readonly StringName _SAVE_STATE_FILE = new StringName("res://Core/SaveStateService/SaveState.json");
	private LoggingService _globalLogger = null;

	public SaveStateService()
	{
		//_globalLogger = GetNode<LoggingService>("/root/LoggingService");
	}

	public void Commit(SaveStateModel saveState)
	{
		//_globalLogger.LogDebug("Commit");
		try
		{
			string content = JsonConvert.SerializeObject(saveState, Formatting.Indented);
			using var file = FileAccess.Open(_SAVE_STATE_FILE, FileAccess.ModeFlags.Write);
			file.StoreString(content);
		}
		catch (Exception exception)
		{
			//_globalLogger.LogError($"Commit exception: {exception}");
			//_globalLogger.LogError($"Commit SaveState: {saveState}");
		}
	}

	public SaveStateModel Load()
	{
		try
		{
			//GD.Print("Load");
			//_globalLogger.LogDebug("Load");
			using var file = FileAccess.Open(_SAVE_STATE_FILE, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			var json = JsonConvert.DeserializeObject<SaveStateModel>(content);
			return json;
		}
		catch (Exception exception)
		{
			//_globalLogger.LogError($"Load exception: {exception}");
		}
		return new SaveStateModel();
	}

	public void Clear()
	{
		//_globalLogger.LogDebug("Clear");
		Commit(new SaveStateModel());
	}
}
