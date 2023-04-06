//-----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Application Settings
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// Application Settings
/// </summary>
[ExcludeFromCodeCoverage]
public class AppSettings
{
    /// <summary>
    /// App Version
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// App Title
    /// </summary>
    public string AppTitle { get; set; }

    /// <summary>
    /// App Description
    /// </summary>
    public string AppDescription { get; set; }

    /// <summary>
    /// Environment Name
    /// </summary>
    public string EnvironmentName { get; set; }

    /// <summary>
    /// User Name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// OpenAI API Key
    /// </summary>
    public string OpenAIApiKey { get; set; }
    
    /// <summary>
    /// Name of Azure OpenAI Resource
    /// </summary>
    public string OpenAIResourceName { get; set; }

    ///// <summary>
    ///// OpenAI API URL for Chat
    ///// </summary>
    //public string OpenAIChatUrl { get; set; }

    /// <summary>
    /// OpenAI API URL for creating an image
    /// </summary>
    public string OpenAIImageGenerateUrl { get; set; }

    /// <summary>
    /// OpenAI API URL for editing an image
    /// </summary>
    public string OpenAIImageEditUrl { get; set; }

    /// <summary>
    /// OpenAI API Image Size
    /// </summary>
    public string OpenAIImageSize { get; set; }

    /// <summary>
    /// Application Settings
    /// </summary>
    public AppSettings()
    {
        UserName = string.Empty;
    }
}
