//-----------------------------------------------------------------------
// <copyright file="ImagePrompt.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Image Prompt
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// Image prompt
/// </summary>

public class ImagePrompt
{
    /// <summary>
    /// Image prompt
    /// </summary>
    public string PromptText { get; set; }

    /// <summary>
    /// Initialization
    /// </summary>
    public ImagePrompt()
    {
        PromptText = string.Empty;
    }
    /// <summary>
    /// Initialization
    /// </summary>

    public ImagePrompt(string prompt)
    {
        PromptText = prompt;
    }
}