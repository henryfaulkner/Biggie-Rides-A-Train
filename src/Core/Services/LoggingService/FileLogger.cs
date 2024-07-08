using Godot;
using System;

public class FileLogger : ILogger
{
	public FileLogger(string filePath, Enumerations.LogLevels logLevel)
	{
		FilePath = filePath;
		LogLevel = logLevel;
	}

	private string FilePath { get; set; }
	private Enumerations.LogLevels LogLevel { get; set; }

	public void Log(Enumerations.LogLevels logLevel, string message)
	{
		if ((int)LogLevel >= (int)logLevel)
		{
			PublishLog(message);
		}
	}

	public void PublishLog(string message)
	{
		try
		{
			using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
			file.StoreString(message);
		}
		catch (Exception exception)
		{
			////GD.Print($"Commit exception: {exception}");
		}
	}
}
