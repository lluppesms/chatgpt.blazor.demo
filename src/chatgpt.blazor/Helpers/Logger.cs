namespace chatgpt.blazor.Helpers;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

/// <summary>
/// Logging Service
/// </summary>
public static class MyLogger
{
    /// <summary>
    /// Application Insights Key
    /// </summary>
    private static string _appInsightsInstrumentationKey = string.Empty;

    /// <summary>
    /// Application Insights Key
    /// </summary>
    public static string AppInsightsInstrumentationKey { get { return _appInsightsInstrumentationKey; } set { _appInsightsInstrumentationKey = value; } }

    /// <summary>
    /// System Logger
    /// </summary>
    private static ILogger log = null;

    /// <summary>
    /// System Logger
    /// </summary>
    public static ILogger logger { get { return log; } set { log = value; } }

    /// <summary>
    /// Initialize Logger
    /// </summary>
    public static void InitializeLogger(AppSettings appSettings)
    {
        AppInsightsInstrumentationKey = appSettings.AppInsights_InstrumentationKey;
        CreateAppInsightsClient();
    }

    /// <summary>
    /// Initialize Logger
    /// </summary>
    public static void InitializeLogger(ILogger iLog)
    {
        logger = iLog;
    }

    /// <summary>
    /// Initialize Logger
    /// </summary>
    public static void InitializeLogger(ILogger iLog, AppSettings appSettings)
    {
        logger = iLog;
        AppInsightsInstrumentationKey = appSettings.AppInsights_InstrumentationKey;
        CreateAppInsightsClient();
    }

    /// <summary>
    /// Log Info message
    /// </summary>
    public static void LogInfoOrError(bool isInfo, string infoMsg, string errorMsg, string source = "WebApp", StringBuilder sb = null)
    {
        if (isInfo)
        {
            LogInfo(infoMsg, source, sb);
        }
        else
        {
            LogError(errorMsg, source, sb);
        }
    }

    /// <summary>
    /// Log Info message
    /// </summary>
    public static void LogInfo(string msg, string source = "WebApp", StringBuilder sb = null)
    {
        if (logger != null)
        {
            logger.LogInformation(msg);
        }
        if (sb != null)
        {
            sb.Append($"{msg};\n");
        }
        //Trace.WriteLine(msg);
        Console.WriteLine(msg);
        //Debug.WriteLine(msg);
        AppInsightsTrace(msg, SeverityLevel.Information, source);
    }

    /// <summary>
    /// Log Warning message
    /// </summary>
    public static void LogWarning(string msg, string source = "WebApp", StringBuilder sb = null)
    {
        if (logger != null)
        {
            logger.LogWarning(msg);
        }
        if (sb != null)
        {
            sb.Append($"{msg};\n");
        }
        //Trace.WriteLine(msg);
        Console.WriteLine(msg);
        //Debug.WriteLine(msg);
        AppInsightsTrace(msg, SeverityLevel.Warning, source);
    }

    /// <summary>
    /// Log Error message
    /// </summary>
    public static void LogError(string msg, string source = "WebApp", StringBuilder sb = null)
    {
        if (logger != null)
        {
            logger.LogError(msg);
        }
        if (sb != null)
        {
            sb.Append($"{msg};\n");
        }
        //Trace.WriteLine(msg);
        Console.WriteLine(msg);
        //Debug.WriteLine(msg);
        AppInsightsTrace(msg, SeverityLevel.Error, source);
    }

    #region App Insights Functions
    /// <summary>
    /// App Insights Telemetry Client
    /// </summary>
    private static TelemetryClient appInsightsClient = null;

    /// <summary>
    /// Create Application Insights Telemetry Client
    /// </summary>
    public static bool CreateAppInsightsClient()
    {
        if (appInsightsClient == null)
        {
            if (!string.IsNullOrEmpty(AppInsightsInstrumentationKey))
            {
                var cfg = TelemetryConfiguration.CreateDefault();
                cfg.ConnectionString = $"InstrumentationKey={AppInsightsInstrumentationKey}";
                appInsightsClient = new TelemetryClient(cfg);
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Custom Telemetry Event
    /// </summary>
    private static void AppInsightsTrace(string eventMessage, SeverityLevel severity, string source)
    {
        try
        {
            if (CreateAppInsightsClient())
            {
                if (!string.IsNullOrEmpty(source))
                {
                    appInsightsClient.TrackTrace(eventMessage, severity,
                        new Dictionary<string, string> {
                            { "eventSource", source }
                        });
                }
                else
                {
                    if (!string.IsNullOrEmpty(source))
                    {
                        appInsightsClient.TrackTrace(eventMessage, severity,
                            new Dictionary<string, string> {
                            { "eventSource", source }
                            });
                    }
                    else
                    {
                        appInsightsClient.TrackTrace(eventMessage, severity);
                    }
                }
                appInsightsClient.Flush();
            }
            else
            {
                //Trace.WriteLine(eventMessage);
                Console.WriteLine(eventMessage);
                //Debug.WriteLine(eventMessage);
            }
        }
        catch
        {
            // move on...
            //Trace.WriteLine(eventMessage);
            Console.WriteLine(eventMessage);
            //Debug.WriteLine(eventMessage);
        }
    }

    ///// <summary>
    ///// Log Exception Event
    ///// </summary>
    //private static void AppInsightsLogException(string exceptionMessage, string userName, string machineName)
    //{
    //    try
    //    {
    //        var telemetry = new ExceptionTelemetry();
    //        telemetry.Properties.Add("Message", exceptionMessage);
    //        appInsightsClient.TrackException(telemetry);
    //        appInsightsClient.TrackTrace(exceptionMessage, SeverityLevel.Error,
    //            new Dictionary<string, string> {
    //                { "userName", userName },
    //                { "machineName", machineName }
    //            });
    //        appInsightsClient.Flush();
    //    }
    //    catch
    //    {
    //        // move on...
    //        //Trace.WriteLine(exceptionMessage);
    //        Console.WriteLine(exceptionMessage);
    //        //Debug.WriteLine(exceptionMessage);
    //    }
    //}

    ///// <summary>
    ///// Custom Telemetry Event
    ///// </summary>
    //private static void AppInsightsTrace(string eventMessage)
    //{
    //    AppInsightsTrace(eventMessage, SeverityLevel.Information, string.Empty);
    //}

    ///// <summary>
    ///// Custom Telemetry Event
    ///// </summary>
    //private static void AppInsightsTrace(string eventMessage, SeverityLevel severity)
    //{
    //    AppInsightsTrace(eventMessage, severity, string.Empty);
    //}

    ///// <summary>
    ///// Custom Telemetry Event
    ///// </summary>
    //private static void AppInsightsCustomLog(string eventName, string eventMessage)
    //{
    //    try
    //    {
    //        if (CreateAppInsightsClient())
    //        {
    //            var telemetry = new EventTelemetry
    //            {
    //                Name = eventName
    //            };
    //            telemetry.Properties.Add("Message", eventMessage);
    //            appInsightsClient.TrackEvent(telemetry);
    //            appInsightsClient.Flush();
    //        }
    //        else
    //        {
    //            //Trace.WriteLine(eventMessage);
    //            Console.WriteLine(eventMessage);
    //            //Debug.WriteLine(eventMessage);
    //        }
    //    }
    //    catch
    //    {
    //        // move on...
    //        //Trace.WriteLine(eventMessage);
    //        Console.WriteLine(eventMessage);
    //        //Debug.WriteLine(eventMessage);
    //    }
    //}
    #endregion
}