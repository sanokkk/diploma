using System.Diagnostics.Metrics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using Diploma.Logic.Metrics;
using Diploma.Logic.Services.Interfaces;
using LibreTranslate.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Diploma.Instance.Controllers;

[ApiController]
[Route("v1/tcp")]
public class TcpController : ControllerBase
{
    private readonly ILogger<TcpController> _logger;
    private readonly MetricsCollector _metricsCollector;
    
    public TcpController(ILogger<TcpController> logger, MetricsCollector metricsCollector)
    {
        _logger = logger;
        _metricsCollector = metricsCollector;
    }
    //works
    [HttpGet("localfive/{word}")]
    public async Task<ActionResult> GetLocalAsync([FromRoute]string word)
    {
        var libreTranslate = new LibreTranslate.Net.LibreTranslate("http://host.docker.internal:5005");
        var newWord = await libreTranslate.TranslateAsync(new Translate()
        {
            Source = LanguageCode.Russian,
            Target = LanguageCode.English,
            Text = word
        });

        return Ok(newWord);
    }
}
