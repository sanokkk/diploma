using System.Collections.Concurrent;
using LibreTranslate.Net;
using Prometheus;

namespace Diploma.Logic.Metrics;

public sealed class MetricsCollector
{
    private ConcurrentBag<Gauge> Gauges { get; set; } = new();
    private ConcurrentDictionary<string, string> TranslatedMetricNames { get; set; } = new();

    private const string TranslateHost = "http://host.docker.internal:5005";

    public void Set(string metric, double value)
    {
        var metricName = GetMetricName(metric);

        metric = metricName.Replace(" ", "_");
        
        var gaugeFromCollection = Gauges.FirstOrDefault(g => g.Name == metric);

        if (gaugeFromCollection is null)
        {
            var newGauge = Prometheus.Metrics.CreateGauge(metric, "", new GaugeConfiguration{ SuppressInitialValue = true});
            Gauges.Add(newGauge);
            
            newGauge.Set(value);
            return;
        }
        
        gaugeFromCollection.Set(value);
    }

    private string GetMetricName(string russianName)
    {
        if (TranslatedMetricNames.TryGetValue(russianName, out string? metricName))
        {
            return metricName;
        }

        var translator = new LibreTranslate.Net.LibreTranslate(TranslateHost);

        var translatedWord = translator.TranslateAsync(new Translate()
        {
            Source = LanguageCode.Russian,
            Target = LanguageCode.English,
            Text = russianName
        }).Result;

        TranslatedMetricNames.TryAdd(russianName, translatedWord);

        return translatedWord;
    }
}