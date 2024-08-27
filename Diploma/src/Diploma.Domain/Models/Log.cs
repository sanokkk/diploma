using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618

[Table("Logs")]

public class Log
{
    public Guid Id { get; set; }

    public string LogText { get; set; }

    public LogType LogType { get; set; }

    public DateTimeOffset WhenHappened { get; set; }
    
    [ForeignKey("sensor_data_id")]
    public SensorData SensorData { get; set; }    
}

public enum LogType
{
    None = 0,
    Info = 1,
    Error =2
}