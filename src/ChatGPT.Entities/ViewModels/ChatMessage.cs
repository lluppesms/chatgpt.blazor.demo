//-----------------------------------------------------------------------
// <copyright file="ChatMessage.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Chat Message
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Data;

/// <summary>
/// Chat Message
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Request
    /// </summary>
    public string Request { get; set; }

    /// <summary>
    /// Last Request
    /// </summary>
    public string LastRequest { get; set; }

    /// <summary>
    /// Response
    /// </summary>
    public OpenAIResponse Response { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public ChatMessage()
    {
        Request = string.Empty;
        Response = new();
        LastRequest = string.Empty;
    }
    /// <summary>
    /// Constructor
    /// </summary>
    public ChatMessage(string request)
    {
        Request = request;
        Response = new();
        LastRequest = string.Empty;
    }
}