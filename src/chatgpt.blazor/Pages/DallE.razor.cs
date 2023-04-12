//-----------------------------------------------------------------------
// <copyright file="DallE.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// DallE Code-Behind
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Pages;

/// <summary>
/// DallE Page
/// </summary>
public partial class DallE : ComponentBase
{
    [Inject] IImageService ImageService { get; set; }

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
        // --> this also seems to fire twice for Server apps, but there is a flag, so only do this once...
        var turnOffAsyncWarning = await Task.FromResult(true);
        if (firstRender)
        {
            //await ExecuteRandom();
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
            imageGeneratorStatus = data.Constants.OpenAIMessages.SendingRequest;
            StateHasChanged();
            var createImageRequest = new CreateImageRequest(imagePromptToGenerate);
            var timer = Stopwatch.StartNew();
            var response = await ImageService.CreateImage(createImageRequest);
            var elaspsedMS = timer.ElapsedMilliseconds;
            if (response != null && response.Data != null)
            {
                imageUrl = response.Data[0].Url;
                imageGeneratorStatus = data.Constants.OpenAIMessages.Finished;
                showPicture = true;
            }
            else
            {
                imageGeneratorStatus = $"{data.Constants.OpenAIMessages.Error} ({response.Message})";
            }
            await snackbarstack.PushAsync($"Image Elapsed: {(decimal)elaspsedMS / 1000m:0.0} seconds", SnackbarColor.Info);
        }
        catch (Exception ex)
        {
            imageGeneratorStatus = $"{data.Constants.OpenAIMessages.Error} ({ex.Message})"; ;
        }
        await imageLoadingIndicator.Hide();
        await imagePromptLoadingIndicator.Hide();
        StateHasChanged();
    }
}
