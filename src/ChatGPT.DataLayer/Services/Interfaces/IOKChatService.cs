//-----------------------------------------------------------------------
// <copyright file="IOKChatService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Interface for OpenAI Chat Service using OkGoDoIt/OpenAI-API-dotnet library
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Data;

/// <summary>
/// OpenAI Chat Service using OkGoDoIt/OpenAI-API-dotnet library Interface
/// </summary>
public interface IOKChatService
{
    /// <summary>
    /// Get a Chat Response
    /// </summary>
    Task<OpenAIResponse> GetResponse(OpenAIQuery queryModel);
}
