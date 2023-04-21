//-----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Application Constants
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Data;

/// <summary>
/// Application Constants
/// </summary>
public static class Constants
{
    /// <summary>
    /// Initialize a few of the application constants from the app settings
    /// </summary>
    /// <param name="appSettings"></param>
    public static void Initialize(AppSettings appSettings)
    {
        ApplicationTitle = appSettings.AppTitle;
        SuperUserFirstName = appSettings.SuperUserFirstName;
        SuperUserLastName = appSettings.SuperUserLastName;
    }

    /// <summary>
    /// Application Title
    /// </summary>
    public static string ApplicationTitle { get; private set; }

    // <hack> These constants are used to determine who the super admin user is.</hack>
    // Yes... this is a hack, but it works for this simple example. This should be replaced by actual Active Directory roles.
    // See MyClaimsTransformation.TransformAsync() for more details.

    /// <summary>
    /// Super User First Name
    /// </summary>
    public static string SuperUserFirstName { get; private set; }

    /// <summary>
    /// Super User Last Name
    /// </summary>
    public static string SuperUserLastName { get; private set; }

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
    }

    /// <summary>
    /// Supported Language Model Types
    /// </summary>
    public static class LanguageModelType
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
    public static class OpenAIMessages
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
    public static class OpenAIImageSize
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
        /// Chat Session Object
        /// </summary>
        public const string ChatSessionObject = "Chat";

        /// <summary>
        /// Simple Chat Session Object
        /// </summary>
        public const string SimpleChatSessionObject = "SimpleChat";

        /// <summary>
        /// New Session?
        /// </summary>
        public const string NewSessionObject = "NewSession";
    }
}
