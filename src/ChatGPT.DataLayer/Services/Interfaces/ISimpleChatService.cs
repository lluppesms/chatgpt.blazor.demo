//-----------------------------------------------------------------------
// <copyright file="ISimpleChatService.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Interface for OpenAI Simple Chat Service
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Data;

/// <summary>
/// Simple Chat Service Interface
/// </summary>
public interface ISimpleChatService
{
    /// <summary>
    /// Get a Chat Response
    /// </summary>
    Task<OpenAIResponse> GetResponse(OpenAIQuery queryModel);
}
