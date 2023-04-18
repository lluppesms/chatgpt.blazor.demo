//-----------------------------------------------------------------------
// <copyright file="OpenAIResponse.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// OpenAI Response Model
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Data;

/// <summary>
/// OpenAI Response
/// </summary>
public class OpenAIResponse
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Object
    /// </summary>
    [JsonProperty("object")]
    public string Object { get; set; }

    /// <summary>
    /// Model 
    /// </summary>
    [JsonProperty("model")]
    public string Model { get; set; }

    /// <summary>
    /// Created
    /// </summary>
    [JsonProperty("created")]
    public long Created { get; set; }

    /// <summary>
    /// Choices
    /// </summary>
    [JsonProperty("choices")]
    public OpenAIResponseChoice[] Choices { get; set; }

    /// <summary>
    /// Usage
    /// </summary>
    [JsonProperty("usage")]
    public OpenAIResponseUsage Usage { get; set; }

    /// <summary>
    /// Detailed Query Info 
    /// </summary>
    public string QueryInfo
    {
        get
        {
            var reason = Choices != null && Choices.Length > 0 ? Choices[0]?.FinishReason : "";
            return
                $"Model: {Model}\n" +
                $"Finish Reason: {reason}\n" +
                $"Prompt Tokens: {Usage?.PromptTokens}\n" +
                $"Completion Tokens: {Usage?.CompletionTokens}\n" +
                $"Total Tokens: {Usage?.TotalTokens}\n";
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIResponse()
    {
        Id = string.Empty;
        Object = string.Empty;
        Model = string.Empty;
        Created = 0;
        Choices = Array.Empty<OpenAIResponseChoice>();
        Usage = new OpenAIResponseUsage();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIResponse(string message)
    {
        Id = string.Empty;
        Object = string.Empty;
        Model = string.Empty;
        Created = 0;
        Choices = new List<OpenAIResponseChoice> { new OpenAIResponseChoice(message) }.ToArray();
        Usage = new OpenAIResponseUsage();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIResponse(string id, string @object, string model, long created, OpenAIResponseChoice[] choices, OpenAIResponseUsage usage)
    {
        Id = id;
        Object = @object;
        Model = model;
        Created = created;
        Choices = choices;
        Usage = usage;
    }
}

/// <summary>
/// OpenAI Response Choice
/// </summary>
public class OpenAIResponseChoice
{
    /// <summary>
    /// Index
    /// </summary>
    [JsonProperty("index")]
    public int Index { get; set; }

    /// <summary>
    /// Text 
    /// </summary>
    [JsonProperty("text")]
    public string Text { get; set; }

    /// <summary>
    /// Finish Reason 
    /// </summary>
    [JsonProperty("finish_reason")]
    public string FinishReason { get; set; }

    /// <summary>
    /// Log Probs
    /// </summary>
    [JsonProperty("logprobs")]
    public float? Logprobs { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIResponseChoice()
    {
        Index = 0;
        Text = string.Empty;
        FinishReason = string.Empty;
        Logprobs = null;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIResponseChoice(string text)
    {
        Index = 0;
        Text = text;
        FinishReason = string.Empty;
        Logprobs = 1;
    }
}

/// <summary>
/// OpenAI Response Usage
/// </summary>
public class OpenAIResponseUsage
{
    /// <summary>
    /// Completion Tokens
    /// </summary>
    [JsonProperty("completion_tokens")]
    public int CompletionTokens { get; set; }

    /// <summary>
    /// Prompt Tokens
    /// </summary>
    [JsonProperty("prompt_tokens")]
    public int PromptTokens { get; set; }

    /// <summary>
    /// Total Tokens
    /// </summary>
    [JsonProperty("total_tokens")]
    public long TotalTokens { get; set; }
}
