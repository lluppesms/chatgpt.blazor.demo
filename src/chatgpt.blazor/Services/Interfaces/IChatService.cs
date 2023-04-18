//-----------------------------------------------------------------------
// <copyright file="IChatService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Interface for OpenAI Chat Service
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Data;

/// <summary>
/// Chat Service Interface
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Get a Chat Response
    /// </summary>
    Task<OpenAIResponse> GetResponse(OpenAIQuery queryModel);
}
