//-----------------------------------------------------------------------
// <copyright file="About.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// About Code-Behind
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Pages;

/// <summary>
/// About Page
/// </summary>
public partial class About : ComponentBase
{
    [Inject] AppSettings Settings { get; set; }
    [Inject] HttpContextAccessor Context { get; set; }

    private string buildInfo = string.Empty;

    /// <summary>
    /// Initialization
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // --> this always seems to fire twice for Server apps, so move logic to OnAfterRender
        await base.OnInitializedAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// Initialization
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            var userName = Context.HttpContext.User.Identity?.Name;
            if (userName != null && userName.Contains("lyle", StringComparison.InvariantCultureIgnoreCase) && userName.Contains("luppes", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    var buildInfoFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "buildinfo.json");
                    if (System.IO.File.Exists(buildInfoFile))
                    {
                        using (var r = new StreamReader(buildInfoFile))
                        {
                            var buildInfoData = r.ReadToEnd();
                            var buildInfoObject = JsonConvert.DeserializeObject<BuildInfo>(buildInfoData);
                            buildInfo = $"Build: {buildInfoObject.BuildNumber}";
                        }
                    }
                    buildInfo += string.IsNullOrEmpty(Settings.OpenAIApiKey) ? string.Empty : " " + Settings.OpenAIApiKey.Substring(0, 4);
                }
                catch (Exception)
                {
                    buildInfo = "Build: Error reading buildinfo!";
                }
            }
            StateHasChanged();
        }
    }
}
