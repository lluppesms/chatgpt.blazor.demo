//-----------------------------------------------------------------------
// <copyright file="OpenAIImage.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Application Settings
// </summary>
//-----------------------------------------------------------------------
using OpenAI_API.Images;

namespace ChatGPT.Data;

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

    /// <summary>
    /// Return OK Image Size
    /// </summary>
    public ImageSize ReturnOKImageSize(string imageSizeString)
    {
        switch (imageSizeString)
        {
            case Constants.OpenAIImageSize.Size512:
                return OpenAI_API.Images.ImageSize._512;
            case Constants.OpenAIImageSize.Size256:
                return OpenAI_API.Images.ImageSize._256;
            default:
                return OpenAI_API.Images.ImageSize._1024;
        }
    }
}

/// <summary>
/// Create OpenAI Image Response
/// </summary>
public class CreateImageResponse
{
    /// <summary>
    /// List of images
    /// </summary>
    public List<OpenAIImage> Data { get; set; }

    /// <summary>
    /// Date Image was created
    /// </summary>
    public DateTime CreateDateTime { get; set; }

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

    /// <summary>
    /// Constructor
    /// </summary>
    public CreateImageResponse(DateTime? created, string url)
    {
        CreateDateTime = (DateTime)created;
        Data = new List<OpenAIImage>
        {
            new OpenAIImage() { Url = url }
        };
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
