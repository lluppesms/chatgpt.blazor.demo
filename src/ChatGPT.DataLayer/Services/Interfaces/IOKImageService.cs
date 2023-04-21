//-----------------------------------------------------------------------
// <copyright file="IOKImageService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Interface for OpenAI Image Service
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Data;

/// <summary>
/// Image Service Interface
/// </summary>
public interface IOKImageService
{
    /// <summary>
    /// Create an Image
    /// </summary>
    Task<CreateImageResponse> CreateImage(CreateImageRequest createImageRequest);
}
