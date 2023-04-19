//-----------------------------------------------------------------------
// <copyright file="SimpleImage.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Simple Image Page Code-Behind
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Web.Pages;

/// <summary>
/// Simple Image Page Code-Behind
/// </summary>
public partial class SimpleImage : ComponentBase
{
    [Inject] IJSRuntime JsInterop { get; set; }
    [Inject] ISimpleImageService ImageService { get; set; }

    private string imagePromptToGenerate = string.Empty;
    private string imageGeneratorStatus = string.Empty;
    private bool showPicture = false;
    private string imageUrl;
    private ImagePrompt AppData = new();
    private LoadingIndicator imageLoadingIndicator;
    private LoadingIndicator imagePromptLoadingIndicator;
    private SnackbarStack snackbarstack;

    /// <summary>
    /// Initialization
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsInterop.InvokeVoidAsync("syncHeaderTitle");
            StateHasChanged();
        }
    }

    /// <summary>
    /// Generate an Image
    /// </summary>
    private async void SubmitForm()
    {
        imagePromptToGenerate = AppData.PromptText;
        showPicture = false;
        try
        {
            await imageLoadingIndicator.Show();
            await imagePromptLoadingIndicator.Show();
            imageGeneratorStatus = Data.Constants.OpenAIMessages.SendingRequest;
            StateHasChanged();
            var createImageRequest = new CreateImageRequest(imagePromptToGenerate);
            var timer = Stopwatch.StartNew();
            var response = await ImageService.CreateImage(createImageRequest);
            var elaspsedMS = timer.ElapsedMilliseconds;
            if (response != null && response.Data != null)
            {
                imageUrl = response.Data[0].Url;
                imageGeneratorStatus = Data.Constants.OpenAIMessages.Finished;
                showPicture = true;
            }
            else
            {
                imageGeneratorStatus = $"{Data.Constants.OpenAIMessages.Error} ({response.Message})";
            }
            await snackbarstack.PushAsync($"Image Elapsed: {(decimal)elaspsedMS / 1000m:0.0} seconds", SnackbarColor.Info);
        }
        catch (Exception ex)
        {
            imageGeneratorStatus = $"{Data.Constants.OpenAIMessages.Error} ({ex.Message})"; ;
        }
        await imageLoadingIndicator.Hide();
        await imagePromptLoadingIndicator.Hide();
        StateHasChanged();
    }
}
