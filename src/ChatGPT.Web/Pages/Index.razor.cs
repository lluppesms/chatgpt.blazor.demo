//-----------------------------------------------------------------------
// <copyright file="Index.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Index Page Code Behind
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Web.Pages;

/// <summary>
/// Index Page Code Behind
/// </summary>
public partial class Index : ComponentBase
{
    [Inject] NavigationManager NavManager { get; set; }
    [Inject] IJSRuntime JsInterop { get; set; }

    /// <summary>
    /// Initialization
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await JsInterop.InvokeVoidAsync("syncHeaderTitle");
        }
    }

    /// <summary>
    /// GoTo Page
    /// </summary>
    private void GoToPage(string pageName)
    {
        switch (pageName.Trim().ToLower())
        {
            case "chat":
                NavManager.NavigateTo("/Chat");
                break;
            case "simplechat":
                NavManager.NavigateTo("/SimpleChat");
                break;
            case "simpleimage":
                NavManager.NavigateTo("/SimpleImage");
                break;
            default:
                NavManager.NavigateTo($"/{pageName}");
                break;
        }
    }
}