using System;

namespace GameFramework
{
    public class LogUtility
    {
        public enum LogLevel : byte
        {
            Debug,
            Info,
            Warning,
            Error,
            Fatal
        }

        public static void Debug(object message)
        {
            Print(LogLevel.Debug, message);
        }

        public static void Debug(string message)
        {

            Print(LogLevel.Debug, message);
        }

        public static void Debug(string format, object arg0)
        {
            Print(LogLevel.Debug, Utility.Text.Format(format, arg0));
        }

        public static void Debug(string format, object arg0, object arg1)
        {
            Print(LogLevel.Debug, Utility.Text.Format(format, arg0, arg1));
        }

        public static void Debug(string format, object arg0, object arg1, object arg2)
        {
            Print(LogLevel.Debug, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        public static void Debug(string format, params object[] args)
        {
            Print(LogLevel.Debug, Utility.Text.Format(format, args));
        }

        public static void Info(object message)
        {
            Print(LogLevel.Info, message);
        }

        public static void Info(string message)
        {
            Print(LogLevel.Info, message);
        }

        public static void Info(string format, object arg0)
        {
            Print(LogLevel.Info, Utility.Text.Format(format, arg0));
        }

        public static void Info(string format, object arg0, object arg1)
        {
            Print(LogLevel.Info, Utility.Text.Format(format, arg0, arg1));
        }

        public static void Info(string format, object arg0, object arg1, object arg2)
        {
            Print(LogLevel.Info, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        public static void Info(string format, params object[] args)
        {
            Print(LogLevel.Info, Utility.Text.Format(format, args));
        }

        public static void Warning(object message)
        {
            Print(LogLevel.Warning, message);
        }

        public static void Warning(string message)
        {
            Print(LogLevel.Warning, message);
        }

        public static void Warning(string format, object arg0)
        {
            Print(LogLevel.Warning, Utility.Text.Format(format, arg0));
        }

        public static void Warning(string format, object arg0, object arg1)
        {
            Print(LogLevel.Warning, Utility.Text.Format(format, arg0, arg1));
        }

        public static void Warning(string format, object arg0, object arg1, object arg2)
        {
            Print(LogLevel.Warning, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        public static void Warning(string format, params object[] args)
        {
            Print(LogLevel.Warning, Utility.Text.Format(format, args));
        }

        public static void Error(object message)
        {
            Print(LogLevel.Error, message);
        }

        public static void Error(string message)
        {
            Print(LogLevel.Error, message);
        }

        public static void Error(string format, object arg0)
        {
            Print(LogLevel.Error, Utility.Text.Format(format, arg0));
        }

        public static void Error(string format, object arg0, object arg1)
        {
            Print(LogLevel.Error, Utility.Text.Format(format, arg0, arg1));
        }

        public static void Error(string format, object arg0, object arg1, object arg2)
        {
            Print(LogLevel.Error, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        public static void Error(string format, params object[] args)
        {
            Print(LogLevel.Error, Utility.Text.Format(format, args));
        }

        public static void Fatal(object message)
        {
            Print(LogLevel.Fatal, message);
        }

        public static void Fatal(string message)
        {
            Print(LogLevel.Fatal, message);
        }

        public static void Fatal(string format, object arg0)
        {
            Print(LogLevel.Fatal, Utility.Text.Format(format, arg0));
        }

        public static void Fatal(string format, object arg0, object arg1)
        {
            Print(LogLevel.Fatal, Utility.Text.Format(format, arg0, arg1));
        }

        public static void Fatal(string format, object arg0, object arg1, object arg2)
        {
            Print(LogLevel.Fatal, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        public static void Fatal(string format, params object[] args)
        {
            Print(LogLevel.Fatal, Utility.Text.Format(format, args));
        }

        private static void Print(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log(Utility.Text.Format("<color=#00FF00>{0}</color>", message.ToString()));
                    break;

                case LogLevel.Info:
                    UnityEngine.Debug.Log(Utility.Text.Format("<color=#AED6F1>{0}</color>", message.ToString()));
                    break;

                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(Utility.Text.Format("<color=##F4D03F>{0}</color>", message.ToString()));
                    break;

                case LogLevel.Error:
                    UnityEngine.Debug.LogError(Utility.Text.Format("<color=#FF0000>{0}</color>", message.ToString()));
                    break;

                default:
                    throw new ArgumentException(message.ToString());
            }
        }
    }
}

