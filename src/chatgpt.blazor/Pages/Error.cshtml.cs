using Microsoft.AspNetCore.Mvc.RazorPages;

namespace chatgpt.blazor.Pages;

/// <summary>
/// Error Model
/// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    /// <summary>
    /// Request Id
    /// </summary>
    public string RequestId { get; set; }

    /// <summary>
    /// Show Request Id
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<ErrorModel> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// On Get
    /// </summary>
    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}
