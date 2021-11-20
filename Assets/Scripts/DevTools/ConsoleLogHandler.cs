using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleLogHandler : ILogHandler
{
    private FileStream fs;
    private StreamWriter sw;
    private ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;
    public static LogLevel filterLogLevel = LogLevel.Console;

    public int MaxLogSize = 50;
    private Queue<string> _logs;
    public string[] Logs => _logs.ToArray();

    public ConsoleLogHandler() 
    {
        string filePath = SanitizePath(Application.persistentDataPath + "/Logs");
        if (!Directory.Exists(filePath)) 
            Directory.CreateDirectory(filePath);
        filePath = SanitizePath(Path.Combine(filePath, $"{DateTime.Now.ToString("yyyy-MM-dd")}.log"));
        fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        sw = new StreamWriter(fs);

        _logs = new Queue<string>();

        Debug.unityLogger.logHandler = this;
    }
    public static bool IsLogTypeAllowed(LogLevel logLevel)
    {
        return (int)filterLogLevel >= (int)logLevel;
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        LogToFile(LogLevel.Error, exception.ToString());
        defaultLogHandler.LogException(exception, context);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        string formattedStr = String.Format(format, args);
        LogToFile(LogLevelToType(logType), formattedStr);
        defaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void Log(LogLevel logLevel, bool allowInDebugConsole, string Source, UnityEngine.Object context, string format, params object[] args) 
    {
        string formattedStr = String.Format(format, args);
        LogToFile(logLevel, formattedStr, Source);

        if(allowInDebugConsole)
            defaultLogHandler.LogFormat(LogLevelToType(logLevel), context, format, args);
    }

    private void LogToFile(LogLevel logLevel, string str, string source = "UnityEngine.Debug") 
    {
        string toWrite = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] [{LogLevelToString(logLevel)}] [{source}]: {str}";
        if (IsLogTypeAllowed(logLevel))
        {
            UpdateLogs(toWrite);
        }
        sw.WriteLine(toWrite);
        sw.Flush();
    }

    private void UpdateLogs(string log) 
    {
        if (_logs.Count > MaxLogSize)
            _logs.Dequeue(); 
        _logs.Enqueue(log);
    }

    private LogLevel LogLevelToType(LogType type) 
    {
        switch (type) 
        {
            case LogType.Log:
                return LogLevel.Information;
            case LogType.Error:
                return LogLevel.Error;
            case LogType.Warning:
                return LogLevel.Warning;
            case LogType.Exception:
                return LogLevel.Exception;
            case LogType.Assert:
                return LogLevel.Debug;
            default:
                return LogLevel.All;
        }
    }

    public LogType LogLevelToType(LogLevel logLevel)
    {
        int level = (int)logLevel;
        if (level <= (int)LogLevel.Exception)
            return LogType.Exception;
        if (level <= (int)LogLevel.Error)
            return LogType.Error;
        if (level <= (int)LogLevel.Warning)
            return LogType.Warning;
        if (level <= (int)LogLevel.Information)
            return LogType.Log;
        if (level <= (int)LogLevel.Debug)
            return LogType.Assert;
        return LogType.Log;
    }

    private string LogLevelToString(LogLevel logLevel) 
    {
        int level = (int)logLevel;
        if (level <= (int)LogLevel.Fatal)
            return "FATL";
        if (level <= (int)LogLevel.Exception)
            return "EXCP";
        if (level <= (int)LogLevel.Error)
            return "EROR";
        if (level <= (int)LogLevel.Warning)
            return "WARN";
        if (level <= (int)LogLevel.Console)
            return "CONS";
        if (level <= (int)LogLevel.Information)
            return "INFO";
        return "DEBG";
    }

    private static string SanitizePath(string s)
    {
        return s.Replace('/', '\\');
    }
}
