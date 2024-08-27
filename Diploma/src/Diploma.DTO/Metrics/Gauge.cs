using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Diploma.DTO.Metrics;

public class Gauge<T> where T : struct
{
    private readonly ObservableGauge<T> _observableGauge;
    private Measurement<T> _currentMeasurement;

    public Gauge(Meter meter, string metricName)
    {
        _observableGauge = meter.CreateObservableGauge<T>(metricName, () => _currentMeasurement);
    }

    public void Record(T value, MetricTag[] tags)
    {
        _currentMeasurement = new (value, MakeTagList(tags));
    }
    
    public static TagList MakeTagList(IEnumerable<MetricTag> tags)
    {
        var tagKVs = tags
            .Select(x => new KeyValuePair<string, object?>(x.Name, x.Value))
            .ToArray();

        return new TagList(tagKVs);
    }
}
