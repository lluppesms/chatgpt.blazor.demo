//-----------------------------------------------------------------------
// <copyright file="OKChatService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// OpenAI Chat Service using OkGoDoIt/OpenAI-API-dotnet library
// See https://github.com/OkGoDoIt/OpenAI-API-dotnet
// </summary>
//-----------------------------------------------------------------------
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace ChatGPT.Data;

/// <summary>
/// OpenAI Chat Service using OkGoDoIt/OpenAI-API-dotnet library Implementation
/// </summary>
public class OKChatService : IOKChatService
{
    #region Variables
    /// <summary>
    /// Application Settings
    /// </summary>
    private readonly AppSettings appSettings = new();

    /// <summary>
    /// API Client
    /// </summary>
    private OpenAIAPI api;

    /// <summary>
    /// Selected Language Model
    /// </summary>
    private string languageModel = Constants.LanguageModelType.gpt35turbo;

    /// <summary>
    /// Language Model to use
    /// </summary>
    public string LanguageModel { get => languageModel; set => languageModel = value; }
    #endregion

    #region Initialization
    /// <summary>
    /// Constructor
    /// </summary>
    public OKChatService(AppSettings settings)
    {
        appSettings = settings;
        SetupAPIClient();
    }

    /// <summary>
    /// Set up API Client
    /// </summary>
    private void SetupAPIClient()
    {
        if (string.IsNullOrEmpty(appSettings.OpenAIResourceName))
        {
            api = null;
        }
        else
        {
            api = OpenAIAPI.ForAzure(appSettings.OpenAIResourceName, languageModel, appSettings.OpenAIApiKey);
            api.ApiVersion = "2023-03-15-preview"; // needed to access chat endpoint on Azure
        }
    }
    #endregion

    /// <summary>
    /// Get a Chat Response by supplying a fully detailed query model
    /// </summary>
    public async Task<OpenAIResponse> GetResponse(OpenAIQuery queryModel)
    {
        if (api == null) { return new OpenAIResponse($"An error occurred while initializing ChatGPT!"); }
        try
        {
            if (languageModel != queryModel.Model)
            {
                // if the language model is changed, then the URL has changed so modify API Client 
                languageModel = queryModel.Model;
                SetupAPIClient();
            }

            var okResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Temperature = (double)queryModel.Temperature,
                MaxTokens = queryModel.Max_tokens,
                Messages = new OpenAI_API.Chat.ChatMessage[] {
                new OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.User, queryModel.Prompt)
            }
            });
            var response = new OpenAIResponse(okResult);
            return response;
        }
        catch (Exception ex)
        {
            return new OpenAIResponse($"An error occurred while talking to ChatGPT: {ex.Message}");
        }
    }
}
