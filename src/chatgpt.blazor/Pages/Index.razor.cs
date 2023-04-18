//-----------------------------------------------------------------------
// <copyright file="Chat.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Chat Code-Behind
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Pages;

/// <summary>
/// Index page
/// </summary>
public partial class Index : ComponentBase
{
    [Inject] NavigationManager NavManager { get; set; }

    /// <summary>
    /// GoTo Page
    /// </summary>
    private void GoToPage(string pageName)
    {
        switch (pageName.Trim().ToLower())
        {
            case "simplechat":
                NavManager.NavigateTo("/SimpleChat");
                break;
            case "dalle":
                NavManager.NavigateTo("/DallE");
                break;
            default:
                NavManager.NavigateTo($"/{pageName}");
                break;
        }
    }
}