//-----------------------------------------------------------------------
// <copyright file="ImageService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// OpenAI Image Service
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// Image Service Implementation
/// </summary>
public class ImageService : IImageService
{
    private readonly string apiKey;
    private readonly string createImageRequestUri;
    private readonly string imageSize;
    private readonly string editImageRequestUri;

    /// <summary>
    /// Constructor
    /// </summary>
    public ImageService(AppSettings settings)
    {
        apiKey = settings.OpenAIApiKey;
        imageSize = string.IsNullOrEmpty(settings.OpenAIImageSize) ? settings.OpenAIImageSize : "512x512";
        createImageRequestUri = string.IsNullOrEmpty(settings.OpenAIImageGenerateUrl) ? settings.OpenAIImageGenerateUrl : "https://api.openai.com/v1/images/generations";
        editImageRequestUri = string.IsNullOrEmpty(settings.OpenAIImageEditUrl) ? settings.OpenAIImageEditUrl : "https://api.openai.com/v1/images/edits";
    }

    /// <summary>
    /// Create an Image
    /// </summary>
    public async Task<CreateImageResponse> CreateImage(string basePrompt, CreateImageRequest createImageRequest)
    {
        createImageRequest.Prompt = basePrompt + " " + createImageRequest.Prompt;
        return await CreateImage(createImageRequest);
    }
    
    /// <summary>
    /// Create an Image
    /// </summary>
    public async Task<CreateImageResponse> CreateImage(CreateImageRequest createImageRequest)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        var request = new CreateImageRequest(createImageRequest.Prompt);
        //using StringContent jsonContent = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        using StringContent jsonContent = new(
            System.Text.Json.JsonSerializer.Serialize(new
            {
                prompt = createImageRequest.Prompt,
                size = imageSize
            }), Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await client.PostAsync(createImageRequestUri, jsonContent);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CreateImageResponse>();
        }
        return new CreateImageResponse(response.StatusCode.ToString());
    }

    /// <summary>
    /// Edit an Image
    /// </summary>
    public async Task<CreateImageResponse> EditImage(EditImageRequest editImageRequestDTO, string webRootPath)
    {
        var imagePath = await SaveFile(webRootPath, editImageRequestDTO!.File!);
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        using var formData = new MultipartFormDataContent();
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.Add("Content-Type", "image/png");
        formData.Add(imageContent, "image", "otter.png");
        formData.Add(new StringContent(editImageRequestDTO.Prompt!), "prompt");
        formData.Add(new StringContent(imageSize), "size");
        var response = client.PostAsync(editImageRequestUri, formData).Result;
        File.Delete(imagePath);
        return await response.Content.ReadFromJsonAsync<CreateImageResponse>();
    }

    /// <summary>
    /// Save a File to the File System
    /// </summary>
    private static async Task<string> SaveFile(string webRootPath, IBrowserFile file)
    {
        //string imageUploadPath = Path.Combine(webHostEnvironment.WebRootPath, "img");
        var imageUploadPath = Path.Combine(webRootPath, "img");
        Directory.CreateDirectory(imageUploadPath);
        var fullFilePath = Path.Combine(imageUploadPath, file.Name);
        await using FileStream fs = new(fullFilePath, FileMode.Create);
        await file.OpenReadStream().CopyToAsync(fs);
        fs.Close();
        return fullFilePath;
    }
}
