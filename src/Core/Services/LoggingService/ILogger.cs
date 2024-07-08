public interface ILogger
{
    public void Log(Enumerations.LogLevels logLevel, string message);
    public void PublishLog(string message);
}