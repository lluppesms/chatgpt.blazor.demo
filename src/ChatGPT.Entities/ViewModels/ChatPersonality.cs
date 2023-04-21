//-----------------------------------------------------------------------
// <copyright file="SessionState.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Session Variables
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Data;

/// <summary>
/// Chat Personalities
/// </summary>
public static class ChatPersonality
{
    /// <summary>
    /// List of Chat Personalities
    /// </summary>
    public static List<ChatPersonalityDefinition> Personalities = new()
    {
        new ChatPersonalityDefinition("", "Normal", "", 10),
        new ChatPersonalityDefinition("pirate", "A Pirate", "use the voice of a pirate to respond. ", 20),
        new ChatPersonalityDefinition("grandma", "Your Grandma", "use the voice of a kindly grandma to respond. ", 30),
        new ChatPersonalityDefinition("tedlasso", "Ted Lasso", "use the voice of Ted Lasso to respond. ", 40),
        new ChatPersonalityDefinition("johnwayne", "John Wayne", "use the voice of John Wayne to respond. ", 50),
        new ChatPersonalityDefinition("spock", "Mr. Spock", "use the voice of Science Officer Spock to respond. ", 60),
        new ChatPersonalityDefinition("mobster", "A mobster", "use the voice of a prohibition mobster to respond. ", 70),
        new ChatPersonalityDefinition("bugsbunny", "Bugs Bunny", "use the voice of a Bugs Bunny to respond. ", 80),
        new ChatPersonalityDefinition("captainkirk", "Captain Kirk", "use the voice of Captain James T. Kirk to respond. ", 90),
        new ChatPersonalityDefinition("dundee", "Crocodile Dundee", "use the voice of Crocodile Dundee to respond. ", 100),
        new ChatPersonalityDefinition("elmerfudd", "Elmer Fudd", "use the voice of a Elmer Fudd to respond. ", 110),
        new ChatPersonalityDefinition("emmetbrown", "Dr. Emmet Brown", "use the voice of Dr. Emmet Brown from Back to the Future to respond. ", 120),
        new ChatPersonalityDefinition("mufasa", "Mufasa", "use the voice of Mufasa the Lion King to respond. ", 130),
        new ChatPersonalityDefinition("paulbunyan", "Paul Bunyan", "use the voice of Paul Bunyan to respond. ", 140),
        new ChatPersonalityDefinition("abelincoln", "Abraham Lincoln", "use the voice of Abraham Lincoln to respond. ", 150)
    };
}

/// <summary>
/// Chat Personalities
/// </summary>
public class ChatPersonalityDefinition
{
    /// <summary>
    /// Personality Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Personality Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Personality Prompt
    /// </summary>
    public string Prompt { get; set; }

    /// <summary>
    /// Personality Sort Order
    /// </summary>
    public int SortOrderNbr { get; set; }

    /// <summary>
    /// Is this item selected by default?
    /// </summary>
    public string SelectedByDefault
    {
        get
        {
           return Name == string.Empty ? "selected" : "";
        }
    }

    /// <summary>
    /// Initialization
    /// </summary>
    public ChatPersonalityDefinition()
    {
        Name = string.Empty;
        Description = string.Empty;
        Prompt = string.Empty;
        SortOrderNbr = 0;
    }

    /// <summary>
    /// Initialization
    /// </summary>
    public ChatPersonalityDefinition(string name, string description, string prompt, int sortOrderNbr)
    {
        Name = name;
        Description = description;
        Prompt = prompt;
        SortOrderNbr = sortOrderNbr;
    }
}
