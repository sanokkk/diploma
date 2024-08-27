using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using Diploma.DTO.Metrics;
using Diploma.Logic.Services.Interfaces;

namespace Diploma.Logic.Services.Implementations;

public class DiagnosticMetrics : IMetrics
{
    public static string MeterName = "telemetry";
    private readonly Meter _meter = new (MeterName, "1.0.0");
    private readonly ConcurrentDictionary<string, Gauge<double>> _gaugeCache = new();


    public async Task Set(string metricName, double value, params MetricTag[] tags)
    {
        var gauge = GetGauge(metricName);
        gauge.Record(value, tags);
    }
    
    private Gauge<double> GetGauge(string metricName)
    {
        if (!_gaugeCache.ContainsKey(metricName))
        {
            var gauge = new Gauge<double>(_meter, metricName);
            _gaugeCache.TryAdd(metricName, gauge);
        }

        return _gaugeCache[metricName];
    }
}
