//-----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Application Constants
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.data;

/// <summary>
/// Application Constants
/// </summary>
public class Constants
{
    /// <summary>
    /// Supported Language Model Types
    /// </summary>
    public class LanguageModelType
    {
        /// <summary>
        /// Model: text-davinci-003
        /// </summary>
        public const string textDavinci003 = "text-davinci-003";
        /// <summary>
        /// Model: gpt-35-turbo
        /// </summary>
        public const string gpt35turbo = "gpt35";
    }

    /// <summary>
    /// Size of generated image 
    /// </summary>
    public class OpenAIMessages
    {
        /// <summary>
        /// Starting Up Message
        /// </summary>
        public const string StartingUp = "Starting up...";
        /// <summary>
        /// Sending Request Message
        /// </summary>
        public const string SendingRequest = "Getting Dall-E's 2 cents worth on this topic...";
        /// <summary>
        /// Error Message
        /// </summary>
        public const string Error = "Dall-E has gone silent :(";
        /// <summary>
        /// Finished Message
        /// </summary>
        public const string Finished = "Another masterpiece by Dall-E!";
        /// <summary>
        /// Disabled Message
        /// </summary>
        public const string Disabled = ""; // "Dall-E has left the building!";
    }

    /// <summary>
    /// Size of generated image 
    /// </summary>
    public class OpenAIImageSize
    {
        /// <summary>
        /// 256x256
        /// </summary>
        public const string Size256 = "256x256";
        /// <summary>
        /// 512x512
        /// </summary>
        public const string Size512 = "512x512";
        /// <summary>
        /// 1024x1024
        /// </summary>
        public const string Size1024 = "1024x1024";
    }

    /// <summary>
    /// Local Storage Constants
    /// </summary>
    public static class LocalStorage
    {
        /// <summary>
        /// Session Object
        /// </summary>
        public const string SessionObject = "Session";
    }


    /// <summary>
    /// Security Constants
    /// </summary>
    public static class Security
    {
        /// <summary>
        /// Admin Claim Type Name
        /// </summary>
        public const string AdminClaimType = "isAdmin";

        /// <summary>
        /// Admin Role Name
        /// </summary>
        public const string AdminRoleName = "Admin";

        // These constants are used to determine who the super admin is...
        // Yes... this is a hack, but it works for a simple example...
        // This should be replaced by actual Active Directory roles...

        /// <summary>
        /// Super User First Name
        /// </summary>
        public const string SuperUserFirstName = "lyle";
        /// <summary>
        /// Super User Last Name
        /// </summary>
        public const string SuperUserLastName = "luppes";
    }
}
