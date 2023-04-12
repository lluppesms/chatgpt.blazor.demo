//-----------------------------------------------------------------------
// <copyright file="ChatService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// OpenAI Chat Service
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// Chat Service Implementation
/// </summary>
public class ChatService : IChatService
{
    #region Variables
    /// <summary>
    /// Application Settings
    /// </summary>
    private readonly AppSettings appSettings = new();

    /// <summary>
    /// HTTP Client
    /// </summary>
    private HttpClient _httpClient;

    /// <summary>
    /// Selected Language Model
    /// </summary>
    private string languageModel = Constants.LanguageModelType.textDavinci003;

    /// <summary>
    /// Language Model to use
    /// </summary>
    public string LanguageModel { get => languageModel; set => languageModel = value; }
    #endregion

    #region Initialization
    /// <summary>
    /// Constructor
    /// </summary>
    public ChatService(AppSettings settings)
    {
        appSettings = settings;
        SetupHttpClient();
    }

    /// <summary>
    /// Set up HTTP Client
    /// </summary>
    private void SetupHttpClient()
    {
        if (string.IsNullOrEmpty(appSettings.OpenAIResourceName)) { 
            _httpClient = null;
        }
        else
        {
            var chatUrl = $"https://{appSettings.OpenAIResourceName}.openai.azure.com/openai/deployments/{languageModel}/completions?api-version=2022-12-01";
            _httpClient = new()
            {
                BaseAddress = new Uri(chatUrl)
            };
            _httpClient.DefaultRequestHeaders.Add("api-key", $"{appSettings.OpenAIApiKey}");
        }
    }
    #endregion

    /// <summary>
    /// Get a Chat Response by supplying a fully detailed query model
    /// </summary>
    public async Task<OpenAIResponse> GetResponse(OpenAIQuery queryModel)
    {
        if (_httpClient == null) { return new OpenAIResponse($"An error occurred while initializing ChatGPT!"); }
        try
        {
            if (languageModel != queryModel.Model)
            {
                // if the language model is changed, then the URL has changed so modify HTTP Client 
                languageModel = queryModel.Model;
                SetupHttpClient();
            }

            //// How do I handle model instructions -- these seem to be ignored... ???
            //if (!string.IsNullOrEmpty(queryModel.Instructions))
            //{
            //    queryModel.Prompt = queryModel.Instructions + " \n" + queryModel.Prompt;
            //}
            //queryModel.Instructions = null;

            var modelData = JsonConvert.SerializeObject(queryModel);
            var request = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress)
            {
                Content = new StringContent(modelData, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var structuredResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseContent);
            return structuredResponse;
        }
        catch (Exception ex)
        {
            return new OpenAIResponse($"An error occurred while talking to ChatGPT: {ex.Message}");
        }
    }
}
