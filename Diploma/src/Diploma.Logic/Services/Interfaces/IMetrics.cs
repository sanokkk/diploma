using Diploma.DTO.Metrics;

namespace Diploma.Logic.Services.Interfaces;

public interface IMetrics
{
    /// <summary>
    /// Установить значение метрики
    /// </summary>
    /// <param name="metricName">Название метрики</param>
    /// <param name="value"></param>
    /// <param name="tags"></param>
    Task Set(string metricName, double value, params MetricTag[] tags);
}
