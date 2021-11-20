using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Console
{
    public static ConsoleLogHandler logHandler = new ConsoleLogHandler();

    public static Dictionary<string, bool> enabledClasses = new Dictionary<string, bool>();

    public static void ShowInDebugConsole() 
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        if(!enabledClasses.TryGetValue(source, out bool value))
            enabledClasses.Add(source, true);
        else
            enabledClasses[source] = true;
    }
    public static void HideInDebugConsole() 
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        if (!enabledClasses.TryGetValue(source, out bool value))
            enabledClasses.Add(source, false);
        else
            enabledClasses[source] = false;
    }

    private static bool LookUpClass(string source) 
    {
        if (enabledClasses.TryGetValue(source, out bool value))
        {
            return value;
        }
        else
        {
            enabledClasses.Add(source, false);
            return false;
        }
    }


    public static void LogDebug(object message) 
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(LogLevel.Debug, source, message, null, true);
    }
    public static void LogDebug(object message, UnityEngine.Object context)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(LogLevel.Debug, source, message, context, true);
    }

    public static void Log(LogLevel logLevel, object message)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(logLevel, source, message, null);
    }

    public static void Log(LogLevel logLevel, object message, UnityEngine.Object context)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(logLevel, source, message, context);
    }

    public static void Log(LogLevel logLevel, string tag, object message)
    {
        Log(logLevel, tag, message, null);
    }

    public static void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context)
    {
        Log(logLevel, tag, message, context, LookUpClass(tag)) ;
    }

    public static void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context, bool ShowInDebugConsole)
    {
        logHandler.Log(logLevel, ShowInDebugConsole, tag, context, message.ToString(), null);
    }

    public static void Log(object message)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(LogLevel.Information, source, message, null);
    }

    public static void Log(string tag, object message)
    {
        Log(LogLevel.Information, tag, message, null);
    }

    public static void Log(string tag, object message, UnityEngine.Object context)
    {
        Log(LogLevel.Information, tag, message, context);
    }

    public static void LogError(object message)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(LogLevel.Error, source, message, null);
    }

    public static void LogError(string tag, object message)
    {
        Log(LogLevel.Error, tag, message, null);
    }

    public static void LogError(string tag, object message, UnityEngine.Object context)
    {
        Log(LogLevel.Error, tag, message, context);
    }

    public static void LogException(Exception exception)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(LogLevel.Exception, source, exception, null);
    }

    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(LogLevel.Exception, source, exception, context);
    }

    public static void LogFormat(LogLevel logLevel, string format, params object[] args)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        string f = String.Format(format, args);
        Log(logLevel, source, f, null);
    }

    public static void LogFormat(LogLevel logLevel, UnityEngine.Object context, string format, params object[] args)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        string f = String.Format(format, args);
        Log(logLevel, source, f, context);
    }

    public static void LogWarning(object message)
    {
        string source = (new System.Diagnostics.StackTrace()).GetFrame(1).GetType().Name;
        Log(LogLevel.Warning, source, message, null);
    }

    public static void LogWarning(string tag, object message)
    {
        Log(LogLevel.Warning, tag, message, null);
    }

    public static void LogWarning(string tag, object message, UnityEngine.Object context)
    {
        Log(LogLevel.Warning, tag, message, context);
    }
}
