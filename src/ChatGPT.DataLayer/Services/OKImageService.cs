//-----------------------------------------------------------------------
// <copyright file="OKImageService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// OpenAI Image Service using OkGoDoIt/OpenAI-API-dotnet library
// See https://github.com/OkGoDoIt/OpenAI-API-dotnet
//
// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
// NOTE: This is not working yet...!  Not sure if Azure OpenAI supports this or not yet...!
// https://learn.microsoft.com/en-us/azure/cognitive-services/openai/faq
// "Do the GPT-4 preview models currently support image input? No, GPT-4 is designed by OpenAI to be multimodal, but currently only text input and output is supported."
// 
// I need to change the URL to call OpenAI directly instead of using the Azure OpenAI service...
//
// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
//
// </summary>
//-----------------------------------------------------------------------
using OpenAI_API;
using OpenAI_API.Images;

namespace ChatGPT.Data;

/// <summary>
/// OpenAI Image Service using OkGoDoIt/OpenAI-API-dotnet library Implementation
/// </summary>
public class OKImageService : IOKImageService
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
    public OKImageService(AppSettings settings)
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
    /// Get an Image Response by supplying a fully detailed query model
    /// </summary>
    public async Task<CreateImageResponse> CreateImage(CreateImageRequest createImageRequest)
    {
        if (api == null) { return new CreateImageResponse($"An error occurred while initializing ChatGPT!"); }
        try
        {
            var okResult = await api.ImageGenerations.CreateImageAsync(
                new ImageGenerationRequest(createImageRequest.Prompt, 1, createImageRequest.ReturnOKImageSize(createImageRequest.ImageSize))
            );
            if (okResult.Data != null) 
            {
                var responseValue = new CreateImageResponse(okResult.Created, okResult.Data[0].Url);
                return responseValue;
            }
            return new CreateImageResponse("An image was not generated!");
        }
        catch (Exception ex)
        {
            return new CreateImageResponse($"An error occurred while talking to ChatGPT: {ex.Message}");
        }
    }
}
