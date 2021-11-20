using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Console
{
    public static ConsoleLogHandler logHandler;
    public static bool DefaultEnable;

    public static Dictionary<string, bool> EnabledClasses;

    static Console() 
    {
        EnabledClasses = new Dictionary<string, bool>();
        DefaultEnable = true;
        logHandler = new ConsoleLogHandler();
    }

    public static void ShowInDebugConsole() 
    {
        string source = NameOfCallingClass();
        if(!EnabledClasses.TryGetValue(source, out bool value))
            EnabledClasses.Add(source, true);
        else
            EnabledClasses[source] = true;
    }
    public static void HideInDebugConsole() 
    {
        string source = NameOfCallingClass();
        if (!EnabledClasses.TryGetValue(source, out bool value))
            EnabledClasses.Add(source, false);
        else
            EnabledClasses[source] = false;
    }

    private static bool LookUpClass(string source) 
    {
        if (EnabledClasses.TryGetValue(source, out bool value))
        {
            return value;
        }
        else
        {
            EnabledClasses.Add(source, DefaultEnable);
            return DefaultEnable;
        }
    }

    /// <summary>
    /// Will log to debug if class allows it. Will ignore logging to file
    /// </summary>
    /// <param name="message"></param>
    public static void DebugOnly(object message) 
    {
        string source = NameOfCallingClass();
        Log(LogLevel.Debug, source, message, null, LookUpClass(source), false);
    }
    /// <summary>
    /// Will log to debug if class allows it. Will ignore logging to file
    /// </summary>
    /// <param name="message"></param>
    public static void DebugOnly(object message, UnityEngine.Object context)
    {
        string source = NameOfCallingClass();
        Log(LogLevel.Debug, source, message, context, LookUpClass(source), false);
    }

    public static void Debug(object message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Debug, NameOfCallingClass(), message, context, true);
    }

    public static void LogDebug(object message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Debug, NameOfCallingClass(), message, context);
    }

    public static void LogConsole(object message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Console, NameOfCallingClass(), message, context);
    }

    public static void LogFatal(object message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Fatal, NameOfCallingClass(), message, context);
    }

    public static void Log(LogLevel logLevel, object message, UnityEngine.Object context = null)
    {
        Log(logLevel, NameOfCallingClass(), message, context);
    }

    public static void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context = null)
    {
        Log(logLevel, tag, message, context, LookUpClass(tag)) ;
    }

    public static void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context, bool ShowInDebugConsole, bool logToFile = true)
    {
        logHandler.Log(logLevel, ShowInDebugConsole, tag, context, message.ToString(), logToFile, null);
    }

    public static void Log(object message)
    {
        Log(LogLevel.Information, NameOfCallingClass(), message, null);
    }

    public static void Log(string tag, object message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Information, tag, message, context);
    }

    public static void LogError(object message)
    {
        Log(LogLevel.Error, NameOfCallingClass(), message, null);
    }

    public static void LogError(string tag, object message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Error, tag, message, context);
    }

    public static void LogException(Exception exception, UnityEngine.Object context = null)
    {
        Log(LogLevel.Exception, NameOfCallingClass(), exception, context);
    }

    public static void LogFormat(LogLevel logLevel, string format, params object[] args)
    {
        string f = String.Format(format, args);
        Log(logLevel, NameOfCallingClass(), f, null);
    }

    public static void LogFormat(LogLevel logLevel, UnityEngine.Object context, string format, params object[] args)
    {
        string f = String.Format(format, args);
        Log(logLevel, NameOfCallingClass(), f, context);
    }

    public static void LogWarning(object message)
    {
        Log(LogLevel.Warning, NameOfCallingClass(), message, null);
    }

    public static void LogWarning(string tag, object message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Warning, tag, message, context);
    }

    public static string NameOfCallingClass()
    {
        string fullName;
        Type declaringType;
        int skipFrames = 2;
        do
        {
            System.Reflection.MethodBase method = new System.Diagnostics.StackFrame(skipFrames, false).GetMethod();
            declaringType = method.DeclaringType;
            if (declaringType == null)
            {
                return method.Name;
            }
            skipFrames++;
            fullName = declaringType.FullName;
        }
        while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

        return fullName;
    }
}
