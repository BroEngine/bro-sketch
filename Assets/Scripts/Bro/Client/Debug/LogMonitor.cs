using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Bro.Client
{
    public static class LogMonitor
    {       
        private static readonly StringBuilder _logData = new StringBuilder ();
     
        public static void Initialize ()
        {            
            _logData.AppendLine ( string.Format ( "Version: {0}\r\n", Application.version ) );
            _logData.AppendLine ( string.Format ( "OS: {0}\r\n", SystemInfo.operatingSystem ) );
            _logData.AppendLine ( string.Format ( "Device: {0}\r\n", SystemInfo.deviceModel ) );
            _logData.AppendLine ( string.Format ( "Processor: {0}\r\n", SystemInfo.processorType ) );
            _logData.AppendLine ( string.Format ( "Memory: {0} Mb\r\n", SystemInfo.systemMemorySize ) );
            
            Application.logMessageReceived += OnLogCallBack;
        }

        public static string GetPrefix()
        {
            var text = new StringBuilder ();
            text.AppendLine ( string.Format ( "Version: {0}\n", Application.version ) );
            text.AppendLine ( string.Format ( "OS: {0}\n", SystemInfo.operatingSystem ) );
            text.AppendLine ( string.Format ( "Device: {0}\n", SystemInfo.deviceModel ) );
            text.AppendLine ( string.Format ( "Processor: {0}\n", SystemInfo.processorType ) );
            return text.ToString();
        }

        private static void OnLogCallBack (string condition, string stackTrace, UnityEngine.LogType type)
        {
            var msg = type + " " + DateTime.Now + " Message: " + condition;

            if (!string.IsNullOrEmpty(stackTrace))
            {
                msg = msg + Environment.NewLine + " Stack:" + stackTrace + Environment.NewLine;
            }

            LogHandler ( msg );
            
            if ( UnityEngine.LogType.Assert == type || UnityEngine.LogType.Exception == type || UnityEngine.LogType.Error == type )
            {
                LogHandler ( Environment.NewLine + Environment.StackTrace + Environment.NewLine  );
            } 
        }

        private static void LogHandler (string entry)
        {
            _logData.AppendLine(entry);
        }

        private static void WriteLogLineToStaticLog (string msg)
        {
            File.AppendAllText ( LogPath, msg );
        }

        private static void ResetLogFile()
        {            
            var filePath = LogPath;
            var dirPath = Path.GetDirectoryName(filePath);     
           
            if (!Directory.Exists(dirPath)) 
            {
                Directory.CreateDirectory(dirPath);
            }
            
            File.WriteAllText ( filePath, string.Empty );
        }
 
        private static string LogPath => Path.Combine ( Application.persistentDataPath, "staticlog.txt" );

        public static string Flush()
        {
            ResetLogFile();

            var path = LogPath;
            var text = _logData.ToString();
            
            File.WriteAllText ( path, text );

            return path;
        }
    }
}