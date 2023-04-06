//-----------------------------------------------------------------------
// <copyright file="MessageBubble.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Message Bubble
// </summary>
//-----------------------------------------------------------------------
using Azure.Core;

namespace chatgpt.blazor.data;

/// <summary>
/// Message Bubble
/// </summary>
public class MessageBubble
{
    /// <summary>
    /// User Posting Message
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Body of Message
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Is this message mine?
    /// </summary>
    public bool Mine { get; set; }

    /// <summary>
    /// Time this message was posted    
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Time this message was posted    
    /// </summary>
    public string DisplayTime { 
        get {
            return $"{Time:h:mm:ss}";
        }
    }

    /// <summary>
    /// Detailed Query Info
    /// </summary>
    public string QueryInfo { get; set; }

    /// <summary>
    /// Detailed Query Info
    /// </summary>
    public string QueryInfoPlusTime
    {
        get
        {
            if (Max_tokens > 0)
            {
                return $"Time: {Time:h:mm:ss}\n" +
                    $"Request Temp: {Temperature}\n" +
                    $"Request Tokens: {Max_tokens}\n" +
                    $"{QueryInfo}";
            }
            else
            {
                return $"Time: {Time:h:mm:ss}\n{QueryInfo}";
            }
        }
    }

    /// <summary>
    /// Is this message a notice?
    /// </summary>
    public bool IsNotice => Body == null ? false : Body.StartsWith("[Notice]");

    /// <summary>
    /// Set CSS based on if this message is mine
    /// </summary>
    public string CSS => Mine ? "sent" : "received";

    /// <summary>
    /// Temperature of the query
    /// </summary>
    public decimal Temperature { get; set; }

    /// <summary>
    /// Maximum number of tokens
    /// </summary>
    public int Max_tokens { get; set; }

    /// <summary>
    /// Prompt
    /// </summary>
    public string Prompt { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public MessageBubble(string username, string body, bool mine)
    {
        Username = username;
        Body = body;
        Mine = mine;
        Time = DateTime.Now;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public MessageBubble(string username, string body, bool mine, string queryInfo)
    {
        Username = username;
        Body = body;
        Mine = mine;
        Time = DateTime.Now;
        QueryInfo = queryInfo;
        Temperature = 0;
        Max_tokens = 0;
        Prompt = string.Empty;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public MessageBubble(string username, string body, bool mine, string queryInfo, string prompt, decimal temperature, int tokens)
    {
        Username = username;
        Body = body;
        Mine = mine;
        Time = DateTime.Now;
        QueryInfo = queryInfo;
        Temperature = temperature;
        Max_tokens = tokens;
        Prompt = prompt;
    }
}
