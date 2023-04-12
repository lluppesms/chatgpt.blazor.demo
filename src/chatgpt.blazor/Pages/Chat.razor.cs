//-----------------------------------------------------------------------
// <copyright file="Index.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Index Code-Behind
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Pages;

/// <summary>
/// Index Page
/// </summary>
public partial class Chat : ComponentBase
{
    [Inject] IChatService chatService { get; set; }
    [Inject] IJSRuntime JsInterop { get; set; }
    [Inject] ILocalStorageService LocalStorageSvc { get; set; }
    [Inject] SweetAlertService SweetAlert { get; set; }

    private SessionStorageService SessionStorage = null;
    private SessionState AppData = new();
    private LoadingIndicator loadingIndicator;
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
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await GetSession();
            HideQueryForm(false);
        }
    }

    private async void SubmitForm()
    {
        HideQueryForm(true);
        try
        {
            // get rid of extra return characters, which seem to break the model...?
            AppData.ChatCurrentMessage.LastRequest = AppData.ChatCurrentMessage.Request.Replace("\n", " "); ;
            // reset form for next time
            AppData.ChatCurrentMessage.Request = string.Empty;
            // post the current message on the screen
            DisplayNewMessage(AppData.ChatCurrentMessage.LastRequest, true);
            // go get the API response
            var timer = Stopwatch.StartNew();
            AppData.ChatCurrentMessage.Response = await chatService.GetResponse(
              new OpenAIQuery(AppData.ChatSelectedModel, AppData.ChatTemperatureDec, AppData.ChatTokenValue, AppData.ChatCurrentMessage.LastRequest)
            );
            var elaspsedMS = timer.ElapsedMilliseconds;
            // post the repsonse to the screen
            DisplayNewMessage(AppData.ChatCurrentMessage.Response.Choices[0].Text, AppData.ChatCurrentMessage.Response.QueryInfo, AppData.ChatCurrentMessage.LastRequest, AppData.ChatTemperatureDec, AppData.ChatTokenValue, elaspsedMS);
            await SaveSession();
            var tokenText = AppData.ChatCurrentMessage.Response.QueryInfo[AppData.ChatCurrentMessage.Response.QueryInfo.IndexOf("Prompt")..];
            await snackbarstack.PushAsync($"Elapsed: {(decimal)elaspsedMS / 1000m:0.0} seconds; {tokenText}", SnackbarColor.Info);
        }
        catch (Exception ex)
        {
            DisplayNewMessage(new MessageBubble("Error", ex.Message));
        }
        HideQueryForm(false);
    }

    private void DisplayNewMessage(string messageText, bool isMine)
    {
        var userName = isMine ? "Me" : "GPT";
        DisplayNewMessage(new MessageBubble(userName, AppData.ChatCurrentMessage.LastRequest, isMine));
    }
    private void DisplayNewMessage(string messageText, string queryInfo, string prompt, decimal temperature, int tokens, long elapsed)
    {
        DisplayNewMessage(new MessageBubble("GPT", messageText, false, queryInfo, prompt, temperature, tokens, elapsed));
    }
    private async void DisplayNewMessage(MessageBubble message)
    {
        AppData.ChatMessageHistory.Add(message);
        ScrollDownAndRefocus();
        await SaveSession();
    }
    private async void HideQueryForm(bool hideForm)
    {
        if (hideForm)
        {
            await loadingIndicator.Show();
            StateHasChanged();
        }
        else
        {
            await loadingIndicator.Hide();
            ScrollDownAndRefocus();
        }
    }
    private async void ScrollDownAndRefocus()
    {
        StateHasChanged(); // we have to repaint here otherwise scroll to bottom doesn't quite go to the bottom
        await JsInterop.InvokeVoidAsync("scrollToBottomOfDiv", "conversation");
        await JsInterop.InvokeVoidAsync("focusOnInputField", "inputText");
    }

    private async void ResetChat()
    {
        if (await Utilities.QueryUserPrompt(SweetAlert, "Clear Entries?", "This will remove your chat history", "Yes", "No", false))
        {
            AppData.ChatMessageHistory = new();
            AppData.ChatCurrentMessage = new();
            DisplayNewMessage(new MessageBubble());
            await SaveSession();
        }
    }
    private async Task<bool> GetSession()
    {
        AppData = new SessionState();
        SessionStorage ??= new SessionStorageService(LocalStorageSvc);
        var previousState = await SessionStorage.GetState();
        if (previousState != null && previousState.ChatMessageHistory.Count > 1)
        {
            if (await Utilities.QueryUserPrompt(SweetAlert, "Load previous session?", "A previous session was found in your local storage", "Yes", "No", false))
            {
                AppData = previousState;
            }
            else
            {
                await SaveSession();
            }
        }
        if (AppData.ChatMessageHistory.Count == 0)
        {
            DisplayNewMessage(new MessageBubble());
        }
        return true;
    }
    private async Task<bool> SaveSession()
    {
        SessionStorage ??= new SessionStorageService(LocalStorageSvc);
        await SessionStorage.StoreState(AppData);
        return true;
    }
}