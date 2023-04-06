//-----------------------------------------------------------------------
// <copyright file="BuildInfo.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Build Info ViewModel
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// Build Info
/// </summary>
public class BuildInfo
{
    /// <summary>
    /// Build Date
    /// </summary>
    [JsonProperty("buildDate")]
    public string BuildDate { get; set; }

    /// <summary>
    /// Build Number
    /// </summary>
    [JsonProperty("buildNumber")]
    public string BuildNumber { get; set; }

    /// <summary>
    /// Build Id
    /// </summary>
    [JsonProperty("buildId")]
    public string BuildId { get; set; }

    /// <summary>
    /// Branch Name
    /// </summary>
    [JsonProperty("branchName")]
    public string BranchName { get; set; }

    /// <summary>
    /// Commit Hash
    /// </summary>
    [JsonProperty("commitHash")]
    public string CommitHash { get; set; }
}
