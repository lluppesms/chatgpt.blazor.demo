namespace chatgpt.blazor.Pages;

/// <summary>
/// Index Page
/// </summary>
public partial class Index : ComponentBase
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IChatService chatService { get; set; }
    [Inject] IJSRuntime JsInterop { get; set; }
    [Inject] SweetAlertService Sweet { get; set; }
    [Inject] AppSettings settings { get; set; }
    [Inject] ILocalStorageService LocalStorageSvc { get; set; }
    [Inject] IToaster Toaster { get; set; }

    //private AppDataService AppData = new();
    private SessionStorageService SessionStorage = null;
    private SessionState AppData = new();

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
            await HideQueryForm(false);
        }
    }

    private async void HandleFormSubmit()
    {
        await HideQueryForm(true);
        try
        {
            // get rid of extra return characters, which seem to break the model...?
            AppData.ChatCurrentMessage.LastRequest = AppData.ChatCurrentMessage.Request.Replace("\n", " "); ;
            // reset form for next time
            AppData.ChatCurrentMessage.Request = string.Empty;
            // post the current message on the screen
            AppData.ChatMessageHistory.Add(new MessageBubble("Me", AppData.ChatCurrentMessage.LastRequest, true));
            // go get the API response
            AppData.ChatCurrentMessage.Response = await chatService.GetResponse(
              new OpenAIQuery(AppData.ChatSelectedModel, AppData.ChatTemperatureDec, AppData.ChatTokenValue, AppData.ChatCurrentMessage.LastRequest)
            );
            // post the repsonse to the screen
            AppData.ChatMessageHistory.Add(new MessageBubble("GPT", AppData.ChatCurrentMessage.Response.Choices[0].Text, false, AppData.ChatCurrentMessage.Response.QueryInfo, AppData.ChatCurrentMessage.LastRequest, AppData.ChatTemperatureDec, AppData.ChatTokenValue));
            await SaveSession();
        }
        catch (Exception ex)
        {
            AppData.ChatMessageHistory.Add(new MessageBubble("Error", ex.Message, false));
        }
        await HideQueryForm(false);
    }

    private async Task HideQueryForm(bool hideForm)
    {
        AppData.ChatShowQueryForm = !hideForm;
        StateHasChanged();
        if (AppData.ChatShowQueryForm)
        {
            await JsInterop.InvokeVoidAsync("focusOnInputField", "inputText");
            await JsInterop.InvokeVoidAsync("scrollToBottomOfDiv", "conversation");
            StateHasChanged();
        }
    }
    private void HandleEnterKey(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            HandleFormSubmit();
        }
    }
    private async void ResetChat()
    {
        if (await Utilities.ShowSweetPrompt(Sweet, "Clear Entries?", "This will remove your chat history", "Yes", "No", false))
        {
            AppData.ChatMessageHistory = new();
            AppData.ChatCurrentMessage = new();
            AppData.ChatMessageHistory.Add(new MessageBubble("GPT", "Hello!", false));
            await SaveSession();
            StateHasChanged();
            await JsInterop.InvokeVoidAsync("focusOnInputField", "inputText");
        }
    }
    private async Task<bool> GetSession()
    {
        AppData = new SessionState();
        SessionStorage ??= new SessionStorageService(LocalStorageSvc);
        var previousState = await SessionStorage.GetState();
        if (previousState != null && previousState.ChatMessageHistory.Count > 1)
        {
            if (await Utilities.ShowSweetPrompt(Sweet, "Load previous session?", "A previous session was found in your local storage", "Yes", "No", false))
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
            AppData.ChatMessageHistory.Add(new MessageBubble("GPT", "Hello!", false));
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