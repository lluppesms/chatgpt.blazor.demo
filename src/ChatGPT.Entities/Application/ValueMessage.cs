//-----------------------------------------------------------------------
// <copyright file="SuccessMessage.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Success Message
// </summary>
//-----------------------------------------------------------------------

namespace ChatGPT.Data;

/// <summary>
/// Value Message
/// </summary>
public class ValueMessage
{
    /// <summary>
    /// Integer
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Value Message
    /// </summary>
    public ValueMessage()
    {
        Message = string.Empty;
        Value = -1;
    }

    /// <summary>
    /// Value Message
    /// </summary>
    /// <param name="msg">Message</param>
    public ValueMessage(string msg)
    {
        Message = msg;
        Value = (!msg.ToUpper().StartsWith("ERROR") && !msg.ToUpper().StartsWith("TIMEOUT")) ? 0 : 1;
    }

    /// <summary>
    /// Value Message
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="value">Value</param>
    public ValueMessage(string msg, int value)
    {
        Message = msg;
        Value = value;
    }
}
