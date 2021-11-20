using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleLogHandler : ILogHandler
{
    private FileStream mainFS;
    private StreamWriter mainSW;
    private FileStream lateFS;
    private StreamWriter lateSW;
    private ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;
    public static LogLevel filterLogLevel = LogLevel.Console;
    public static LogLevel filterFileLogLevel = LogLevel.Information;

    static object locker = new object();

    private bool sentFirstMessege;

    public int MaxLogSize = 50;
    private Queue<string> _logs;
    public string[] Logs => _logs.ToArray();

    public ConsoleLogHandler() 
    {
        string filePath = SanitizePath(Application.persistentDataPath + "/Logs");
        if (!Directory.Exists(filePath)) 
            Directory.CreateDirectory(filePath);
        string mainPath = SanitizePath(Path.Combine(filePath, $"{DateTime.Now.ToString("yyyy-MM-dd")}.log"));
        int i = 0;
        while (File.Exists(mainPath))
            mainPath = SanitizePath(Path.Combine(filePath, $"{DateTime.Now.ToString("yyyy-MM-dd")}-{++i}.log"));

        string latestPath = SanitizePath(Path.Combine(filePath, $"latest.log"));

        mainFS = new FileStream(mainPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        mainSW = new StreamWriter(mainFS);

        if(File.Exists(latestPath))
            File.WriteAllText(latestPath, String.Empty);
        lateFS = new FileStream(latestPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        lateSW = new StreamWriter(lateFS);

        _logs = new Queue<string>();

        Debug.Log($"Console has been changed to the Console Class. Log files can be found at:\n{filePath}");
        Debug.unityLogger.logHandler = this;

        sentFirstMessege = false;
    }

    private void BeforeFirst()
    {
        if (!sentFirstMessege)
        {
            sentFirstMessege = true;
            Console.ShowInDebugConsole();
            Console.Log(LogLevel.Console, $"{Application.productName} v{Application.version} by {Application.companyName} is now running!");
            Console.HideInDebugConsole();
            Console.Log(LogLevel.Information, $"{{ \"version\": \"v{Application.version}\", \"buildGuid\": \"{Application.buildGUID}\"," +
                $" \"genuine\": \"{Application.genuine}\", \"genuineCheckAvailable\": \"{Application.genuineCheckAvailable}\"," +
                $"\"installerName\": \"{Application.installerName}\", \"isEditor\": \"{Application.isEditor}\", \"platform\": \"{Application.platform}\", \"unityVersion\": \"{Application.unityVersion}\" }}");
        }
    }

    ~ConsoleLogHandler() 
    {
        Console.Log(LogLevel.All, $"Console has been Unloaded. End of Log.");
        lateSW.Close();
        lateFS.Close();
        mainSW.Close();
        mainFS.Close();
    }

    public static bool IsLogAllowedOnConsole(LogLevel logLevel)
    {
        return (int)filterLogLevel >= (int)logLevel;
    }

    public static bool IsLogAllowedOnFile(LogLevel logLevel)
    {
        return (int)filterFileLogLevel >= (int)logLevel;
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        BeforeFirst();
        LogToFile(LogLevel.Error, exception.ToString());
        defaultLogHandler.LogException(exception, context);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        BeforeFirst();
        if (args == null)
            args = new object[0];

        if (args.Length == 0)
            format = CleanFormat(format);
        string postformat = String.Format(format, args);

        LogToFile(LogChange(logType), postformat);
        if(IsLogAllowedOnConsole(LogChange(logType)))
            defaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void Log(LogLevel logLevel, bool allowInDebugConsole, string Source, UnityEngine.Object context, string format, bool logToFile = true, params object[] args) 
    {
        BeforeFirst();
        if (args == null)
            args = new object[0];
        if (args.Length == 0)
            format = CleanFormat(format);
        string postformat = String.Format(format, args);

        LogToFile(logLevel, postformat, Source);

        if(allowInDebugConsole || IsLogAllowedOnConsole(logLevel))
            defaultLogHandler.LogFormat(LogChange(logLevel), context, format, args);
    }

    private void LogToFile(LogLevel logLevel, string str, string source = "UnityEngine.Debug") 
    {
        BeforeFirst();
        string toWrite = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] [{LogLevelToString(logLevel)}] [{source}]: {str}";
        lock (locker)
        {
            if (IsLogAllowedOnConsole(logLevel))
            {
                UpdateLogs(toWrite);
            }
            if (IsLogAllowedOnFile(logLevel))
            {
                mainSW.WriteLine(toWrite);
                mainSW.Flush();
                lateSW.WriteLine(toWrite);
                lateSW.Flush();
            }
        }
    }

    private void UpdateLogs(string log) 
    {
        if (_logs.Count > MaxLogSize)
            _logs.Dequeue(); 
        _logs.Enqueue(log);
    }

    private LogLevel LogChange(LogType type) 
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

    private LogType LogChange(LogLevel logLevel)
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
            return LogType.Log;
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
        if (level <= (int)LogLevel.Information)
            return "INFO";
        return "DEBG";
    }

    private static string CleanFormat(string s) 
    {
        s = s.Replace("{", "{{");
        s = s.Replace("}", "}}");
        return s;
    }

    private static string SanitizePath(string s)
    {
        return s.Replace('/', '\\');
    }
}
