using Microsoft.UI.Xaml;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChartBase.utils;

public static class Logger
{
    const string LOGGER_FILE = "C:\\temp\\FinancialAnalysis.log";

    private static int counter = 0;
    private static object locker =  new object();
    private static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();


    private static DispatcherTimer UpdateTimer = new DispatcherTimer();
    static Logger()
    {
        UpdateTimer.Interval = TimeSpan.FromMilliseconds(2000);        // 60 UPS
        UpdateTimer.Tick += UpdateTimer_Tick;
        UpdateTimer.Start();
    }

    private static void UpdateTimer_Tick(object sender, object e)
    {
        string msg;
        while (queue.TryDequeue(out msg))
        {
            using (StreamWriter writer = new StreamWriter(LOGGER_FILE, true)) { 
                writer.WriteLine($"{IncrementAndGet(), -4}: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), -20} {msg}");
            }
        }
    }

    public static int IncrementAndGet()
    {
        int ri;
        lock (locker)
        {
            ri = ++counter;
        }
        return ri;

    }

    //public static void LogMessage(string message="LOGGING: ")
    //{
    //    TraceMessage(message);
    //}

    public static void TraceMessage(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
    {
        var sf = sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\")+1);
        var fullMessage = $">>>>>>>>>>>>>>>>>>>>\t{sf}.{memberName}#{sourceLineNumber}: {message}";
        Trace.WriteLine(fullMessage);
    }


    public static void WriteLine(string methodName, string message)
    {
        queue.Enqueue($"{methodName, -30} {message}");
    }

    public static void InitLogFile()
    {
        using (StreamWriter writer = new StreamWriter(LOGGER_FILE, false))
        {
            writer.WriteLine($"=========================  {DateTime.Now}  ==============================");
        }
    }

    internal static void WriteLine(Type type, StackTrace stackTrace, string message)
    {
        StackFrame sf = stackTrace.GetFrame(0);
        var methodInfo = $"{type}#{sf?.GetMethod().Name}:{sf?.GetFileLineNumber()}";
        WriteLine(methodInfo, message);
    }
}
