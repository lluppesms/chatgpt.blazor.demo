//-----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Utilities
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Web.Helpers;

/// <summary>
/// Utilities
/// </summary>
public class Utilities
{
    /// <summary>
    /// Combines all the inner exception messages into one string
    /// </summary>
    public static string GetExceptionMessage(Exception ex)
    {
        var message = string.Empty;
        if (ex == null)
        {
            return message;
        }
        if (ex.Message != null)
        {
            message += ex.Message;
        }
        if (ex.InnerException == null)
        {
            return message;
        }
        if (ex.InnerException.Message != null)
        {
            message += " " + ex.InnerException.Message;
        }
        if (ex.InnerException.InnerException == null)
        {
            return message;
        }
        if (ex.InnerException.InnerException.Message != null)
        {
            message += " " + ex.InnerException.InnerException.Message;
        }
        if (ex.InnerException.InnerException.InnerException == null)
        {
            return message;
        }
        if (ex.InnerException.InnerException.InnerException.Message != null)
        {
            message += " " + ex.InnerException.InnerException.InnerException.Message;
        }
        return message;
    }

    /// <summary>
    /// Sanitize connection string
    /// </summary>
    public static string GetSanitizedConnectionString(string connection)
    {
        //// "DeviceConnectionString": "HostName=iothub123.azure-devices.net;DeviceId=test1;SharedAccessKey=E5Z6******=",
        //// "SQLConnectionString": "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword";
        string noKey;
        if (string.IsNullOrEmpty(connection)) return string.Empty;
        var keyPos = connection.IndexOf("key=", StringComparison.OrdinalIgnoreCase);
        if (keyPos > 0)
        {
            noKey = string.Concat(connection.AsSpan(0, keyPos + 4), "...");
            return noKey;
        }
        keyPos = connection.IndexOf("pwd=", StringComparison.OrdinalIgnoreCase);
        if (keyPos > 0)
        {
            noKey = string.Concat(connection.AsSpan(0, keyPos + 4), "...");
            return noKey;
        }
        keyPos = connection.IndexOf("password=", StringComparison.OrdinalIgnoreCase);
        if (keyPos > 0)
        {
            noKey = string.Concat(connection.AsSpan(0, keyPos + 9), "...");
            return noKey;
        }
        return connection;
    }

    /// <summary>
    /// Get an environment variable
    /// </summary>
    public static string GetEnvironmentVariable(string name)
    {
        //return name + ": " + Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }

    /// <summary>
    /// Put a date into the middle of a file name
    /// </summary>
    public static string DateifyFileName(string fileName)
    {
        var dateString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        var extLocation = fileName.IndexOf(".");
        if (extLocation > 0)
        {
            var fileNameWithDate = fileName[..extLocation] + "-" + dateString + fileName[extLocation..];
            return fileNameWithDate;
        }
        return fileName + dateString;
    }

    /// <summary>
    /// Returns digits - checks to see if string is all numbers, like isnumeric, but works better... commas and periods are ok
    /// </summary>
    public static int ReturnOnlyNumbers(string textToConvert)
    {
        const string Digits = "0123456789";
        var resultString = "0";
        var resultLength = 0;
        try
        {
            int x;
            for (x = 0; x <= textToConvert.Length - 1; x++)
            {
                var lowerCaseChar = textToConvert.Substring(x, 1);
                if (Digits.Contains(lowerCaseChar))
                {
                    resultString += lowerCaseChar;
                    resultLength += 1;
                    if (resultLength > 8)
                    {
                        break;
                    }
                }
            }
            return Convert.ToInt32(resultString);
        }
        catch (Exception ex)
        {
            var message = GetExceptionMessage(ex);
            Console.WriteLine("IsOnlyNumbers: " + message);
            return 9999;
        }
    }

    /// <summary>
    /// Validates that this string has only numbers
    /// </summary>
    public static string IsOnlyLetters(string input)
    {
        const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return IsOnlyTheseCharacters(input, 999, ValidChars);
    }

    /// <summary>
    /// Validates that this string has only number or letters
    /// </summary>
    public static string IsOnlyNumbersOrLetters(string input, int maxLength)
    {
        const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-.";
        return IsOnlyTheseCharacters(input, maxLength, ValidChars);
    }

    /// <summary>
    /// Validates that this string has only number or letters or a space
    /// </summary>
    public static string IsOnlyNumbersOrLettersOrSpace(string input, int maxLength)
    {
        const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-.! ";
        return IsOnlyTheseCharacters(input, maxLength, ValidChars);
    }

    /// <summary>
    /// Validates that this string has only allowed characters
    /// </summary>
    public static string IsOnlyTheseCharacters(string input, int maxLength, string validCharacters)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            if (sb.Length < maxLength)
            {
                if (validCharacters.Contains(input[i]))
                {
                    sb.Append(input[i]);
                }
            }
            else
            {
                break;
            }
        }
        var newString = sb.ToString();
        return newString;
    }

    /// <summary>
    /// Convert DateTimeOffset to DateTime
    /// </summary>
    public static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime)
    {
        if (dateTime.Offset.Equals(TimeSpan.Zero))
            return dateTime.UtcDateTime;
        else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
            return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
        else
            return dateTime.DateTime;
    }

    /// <summary>
    /// Show Sweet Prompt
    /// </summary>
    public static async Task<bool> QueryUserPrompt(SweetAlertService swal, SweetAlertOptions options)
    {
        var confirm = await swal.FireAsync(options).ConfigureAwait(false);
        return confirm.IsConfirmed;
    }

    /// <summary>
    /// Show Sweet Prompt
    /// </summary>
    public static async Task<bool> QueryUserPrompt(SweetAlertService swal, string title, string html, string confirmButtonTxt, string cancelButtonTxt = "Cancel", bool focusCancel = true)
    {
        return await QueryUserPrompt(swal, new SweetAlertOptions
        {
            Title = title,
            Html = html,
            ShowCancelButton = true,
            ConfirmButtonText = confirmButtonTxt,
            CancelButtonText = cancelButtonTxt,
            FocusCancel = focusCancel
            //Icon = SweetAlertIcon.Warning,
        });
    }

    /// <summary>
    /// Show Popup HTML Message
    /// </summary>
    public static async Task PromptUserHtml(SweetAlertService swal, string title, string html, string confirmButtonTxt = "OK")
    {
        await QueryUserPrompt(swal, new SweetAlertOptions
        {
            Title = title,
            Html = html,
            ShowCancelButton = false,
            ConfirmButtonText = confirmButtonTxt
        });
    }

}
