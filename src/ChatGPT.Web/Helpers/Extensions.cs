//-----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Extensions
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Web.Helpers;

/// <summary>
/// Generic Extensions
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Converts object to string but doesn't crash if it's null
    /// </summary>
    public static string ToStringNullable(this object obj)
    {
        return (obj ?? "").ToString();
    }
    /// <summary>
    /// Converts object to string but doesn't crash if it's null
    /// </summary>
    public static string ToStringNullable(this object obj, string defaultValue)
    {
        return (obj ?? defaultValue).ToString();
    }
}
