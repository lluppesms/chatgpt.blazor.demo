//-----------------------------------------------------------------------
// <copyright file="SimpleChat.razor.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Simple Chat Page Code-Behind
// </summary>
//-----------------------------------------------------------------------
using System.Dynamic;
using System;

namespace ChatGPT.Web.Pages;

/// <summary>
/// Simple Chat Page Code-Behind
/// </summary>
public partial class SimpleChat : ComponentBase
{
    [Inject] ISimpleChatService chatService { get; set; }
    [Inject] IJSRuntime JsInterop { get; set; }
    [Inject] ILocalStorageService LocalStorageSvc { get; set; }
    [Inject] SweetAlertService SweetAlert { get; set; }

    private const string DefaultModel = Data.Constants.LanguageModelType.textDavinci003;
    private const string DefaultMessage = "This is a simple Chat-GPT example calling the API directly";
    private SessionStorageService SessionStorage = null;
    private SessionState SimpleAppData = new(DefaultModel);
    private LoadingIndicator loadingIndicator;
    private SnackbarStack snackbarstack;

    /// <summary>
    /// Initialization
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await JsInterop.InvokeVoidAsync("syncHeaderTitle");
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
            SimpleAppData.ChatCurrentMessage.LastRequest = SimpleAppData.ChatCurrentMessage.Request.Replace("\n", " "); ;
            // reset form for next time
            SimpleAppData.ChatCurrentMessage.Request = string.Empty;
            // post the current message on the screen
            DisplayNewMessage(SimpleAppData.ChatCurrentMessage.LastRequest, true);
            // go get the API response
            var timer = Stopwatch.StartNew();
            SimpleAppData.ChatCurrentMessage.Response = await chatService.GetResponse(
              new OpenAIQuery(SimpleAppData.ChatSelectedModel, SimpleAppData.ChatTemperatureDec, SimpleAppData.ChatTokenValue, SimpleAppData.ChatCurrentMessage.LastRequest)
            );
            var elaspsedMS = timer.ElapsedMilliseconds;
            // post the repsonse to the screen
            DisplayNewMessage(SimpleAppData.ChatCurrentMessage.Response.Choices[0].Text, SimpleAppData.ChatCurrentMessage.Response.QueryInfo, SimpleAppData.ChatCurrentMessage.LastRequest, SimpleAppData.ChatTemperatureDec, SimpleAppData.ChatTokenValue, elaspsedMS);
            await SaveSession();
            var tokenText = SimpleAppData.ChatCurrentMessage.Response.QueryInfo[SimpleAppData.ChatCurrentMessage.Response.QueryInfo.IndexOf("Prompt")..];
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
        DisplayNewMessage(new MessageBubble(userName, SimpleAppData.ChatCurrentMessage.LastRequest, isMine));
    }
    private void DisplayNewMessage(string messageText, string queryInfo, string prompt, decimal temperature, int tokens, long elapsed)
    {
        DisplayNewMessage(new MessageBubble("GPT", messageText, false, queryInfo, prompt, temperature, tokens, elapsed));
    }
    private async void DisplayNewMessage(MessageBubble message)
    {
        SimpleAppData.ChatMessageHistory.Add(message);
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
            SimpleAppData.ChatMessageHistory = new();
            SimpleAppData.ChatCurrentMessage = new();
            DisplayNewMessage(new MessageBubble("GPT", DefaultMessage));
            await SaveSession();
        }
    }
    private async Task<bool> GetSession()
    {
        SimpleAppData = new SessionState(DefaultModel);
        SessionStorage ??= new SessionStorageService(LocalStorageSvc);
        var previousState = await SessionStorage.GetState(Data.Constants.LocalStorage.SimpleChatSessionObject);
        if (previousState != null && previousState.ChatMessageHistory.Count > 1)
        {
            if (await Utilities.QueryUserPrompt(SweetAlert, "Load previous session?", "A previous session was found in your local storage", "Yes", "No", false))
            {
                SimpleAppData = previousState;
            }
            else
            {
                await SaveSession();
            }
        }
        if (SimpleAppData.ChatMessageHistory.Count == 0)
        {
            DisplayNewMessage(new MessageBubble("GPT", DefaultMessage));
        }
        return true;
    }
    private async Task<bool> SaveSession()
    {
        SessionStorage ??= new SessionStorageService(LocalStorageSvc);
        await SessionStorage.StoreState(Data.Constants.LocalStorage.SimpleChatSessionObject, SimpleAppData);
        return true;
    }
}