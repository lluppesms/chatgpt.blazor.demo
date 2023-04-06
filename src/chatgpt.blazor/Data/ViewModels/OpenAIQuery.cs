//-----------------------------------------------------------------------
// <copyright file="OpenAIQuery.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// OpenAI Query
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// OpenAI Query
/// </summary>
public class OpenAIQuery
{
    /// <summary>
    /// Model to use
    /// </summary>
    [JsonProperty("model")]
    public string Model { get; set; }

    /// <summary>
    /// Temperature of the query
    /// </summary>
    [JsonProperty("temperature")]
    public decimal Temperature { get; set; }

    /// <summary>
    /// Maximum number of tokens
    /// </summary>
    [JsonProperty("max_tokens")]
    public int Max_tokens { get; set; }

    /// <summary>
    /// Prompt
    /// </summary>
    [JsonProperty("prompt")]
    public string Prompt { get; set; }

    /// <summary>
    /// Instructions
    /// </summary>
    [JsonProperty("instructions", NullValueHandling=NullValueHandling.Ignore)]
    public string Instructions { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIQuery()
    {
        Model = Constants.LanguageModelType.textDavinci003;
        Temperature = 1;
        Max_tokens = 100;
        Prompt = string.Empty;
        Instructions = string.Empty;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIQuery(string prompt)
    {
        Model = Constants.LanguageModelType.textDavinci003;
        Temperature = 1;
        Max_tokens = 100;
        this.Prompt = prompt;
        Instructions = string.Empty;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIQuery(string model, decimal temperature, int max_tokens, string prompt)
    {
        this.Model = model;
        this.Temperature = temperature;
        this.Max_tokens = max_tokens;
        this.Prompt = prompt;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIQuery(string model, decimal temperature, int max_tokens, string prompt, string instructions)
    {
        this.Model = model;
        this.Temperature = temperature;
        this.Max_tokens = max_tokens;
        this.Prompt = prompt;
        this.Instructions = instructions;
    }
}
