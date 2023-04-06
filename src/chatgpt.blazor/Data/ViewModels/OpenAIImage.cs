//-----------------------------------------------------------------------
// <copyright file="OpenAIImage.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Application Settings
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// Create OpenAI Image Request
/// </summary>
public class CreateImageRequest
{
    /// <summary>
    /// Image Prompt
    /// </summary>
    [Required]
    [StringLength(1000)]
    public string Prompt { get; set; }
    
    /// <summary>
    /// Number of images to generate
    /// </summary>
    public int ImageCount = 1;

    /// <summary>
    /// Size of image
    /// </summary>
    public string ImageSize { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CreateImageRequest()
    {
        ImageSize = Constants.OpenAIImageSize.Size256;
        ImageCount = 1;
    }
    /// <summary>
    /// Constructor
    /// </summary>
    public CreateImageRequest(string prompt)
    {
        Prompt = prompt;
        ImageSize = Constants.OpenAIImageSize.Size256;
        ImageCount = 1;
    }
    /// <summary>
    /// Constructor
    /// </summary>
    public CreateImageRequest(string prompt, string imageSize)
    {
        Prompt = prompt;
        ImageSize = imageSize;
        ImageCount = 1;
    }
}

/// <summary>
/// Create OpenAI Image Response
/// </summary>
public class CreateImageResponse
{
    /// <summary>
    /// Image was created?
    /// </summary>
    public int Created { get; set; }

    /// <summary>
    /// List of images
    /// </summary>
    public List<OpenAIImage> Data { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CreateImageResponse()
    {

    }
    /// <summary>
    /// Constructor
    /// </summary>
    public CreateImageResponse(string messageTxt)
    {
        Message = messageTxt;
    }
}

/// <summary>
/// Edit Image Request
/// </summary>
public class EditImageRequest
{
    /// <summary>
    /// File
    /// </summary>
    public IBrowserFile File { get; set; }

    /// <summary>
    /// Image Prompt
    /// </summary>
    [Required]
    [StringLength(1000)]
    public string Prompt { get; set; }
}

/// <summary>
/// OpenAI Image
/// </summary>
public class OpenAIImage
{
    /// <summary>
    /// URL of Image
    /// </summary>
    public string Url { get; set; }
}
