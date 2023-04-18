//-----------------------------------------------------------------------
// <copyright file="IImageService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Interface for OpenAI Image Service
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Data;

/// <summary>
/// Image Service Interface
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Create an Image
    /// </summary>
    Task<CreateImageResponse> CreateImage(string basePrompt, CreateImageRequest createImageRequest);
    /// <summary>
    /// Create an Image
    /// </summary>
    Task<CreateImageResponse> CreateImage(CreateImageRequest createImageRequest);
    /// <summary>
    /// Edit an Image
    /// </summary>
    Task<CreateImageResponse> EditImage(EditImageRequest editImageRequest, string webRootPath);
}
