//-----------------------------------------------------------------------
// <copyright file="Admin.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Admin Code-Behind
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Pages;

/// <summary>
/// Admin Page
/// </summary>
public partial class Admin : ComponentBase
{
    [Inject] AppSettings Settings { get; set; }
    [Inject] HttpContextAccessor Context { get; set; }

    private string userName = string.Empty;
    private string buildInfo = string.Empty;
    private string openAIKeyInfo = string.Empty;
    private string dallEKeyInfo = string.Empty;

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
            var userIdentity = Context.HttpContext.User;
            userName = userIdentity != null ? userIdentity.Identity.Name : string.Empty;
            var isInAdminRole = userIdentity != null && userIdentity.IsInRole("Admin");
            // var isAdmin = userIdentity != null && userIdentity.HasClaim(claim => claim.Type == "isAdmin");
            if (isInAdminRole)
            {
                try
                {
                    var buildInfoFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "buildinfo.json");
                    if (File.Exists(buildInfoFile))
                    {
                        using (var r = new StreamReader(buildInfoFile))
                        {
                            var buildInfoData = r.ReadToEnd();
                            var buildInfoObject = JsonConvert.DeserializeObject<BuildInfo>(buildInfoData);
                            buildInfo = buildInfoObject.BuildNumber;
                        }
                    }
                    openAIKeyInfo = string.IsNullOrEmpty(Settings.OpenAIApiKey) ? string.Empty : Settings.OpenAIApiKey[..4] + "...";
                    dallEKeyInfo = string.IsNullOrEmpty(Settings.DallEApiKey) ? string.Empty : Settings.DallEApiKey[..4] + "...";
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
