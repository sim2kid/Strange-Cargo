public enum LogLevel  : int
{
    Off = int.MinValue,
    Fatal = 1000,
    Exception = 2000,
    Error = 3000,
    Warning = 4000,
    Console = 4999,
    Information = 5000,
    Debug = 6000,
    All = int.MaxValue
}