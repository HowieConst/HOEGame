using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HOEngine.Log
{
    public sealed class LogManager :Singlton<LogManager>,IEngineManager
    {

        public static bool EnableLog = true;
        public static int LogLevel = 0;
        public static bool EnableLogColor = true;

        private string FilePath = "log.txt";
        
        
        private readonly Dictionary<ELogChannel, Color> LogChannelColor = new Dictionary<ELogChannel, Color>()
        {
            { ELogChannel.UI,Color.green},
            { ELogChannel.Resource,Color.blue},
            { ELogChannel.Battle,Color.magenta},
            { ELogChannel.Scene,Color.red},
            { ELogChannel.Message,Color.yellow},
        };

       

        public void Log(string log,ELogChannel logChannel)
        {
            if (!CheckLog(ELogLevel.Log)) return;
            var formateLog = GetLogFormate(ELogLevel.Log, logChannel, log);
            Debug.Log(formateLog);
        }

        public void LogFormate(string formate, string content,ELogChannel logChannel)
        {
            if (!CheckLog(ELogLevel.Log)) return;
            var log = string.Format(formate, content);
            var formateLog = GetLogFormate(ELogLevel.Log, logChannel, log);
            Debug.Log(formateLog);
        }

        public void LogWarning(string log,ELogChannel logChannel)
        {
            if (!CheckLog(ELogLevel.Warning)) return;
            var formateLog = GetLogFormate(ELogLevel.Warning, logChannel, log);
            Debug.LogWarning(formateLog);
        }
        public void LogError(string log,ELogChannel logChannel)
        {
            if (!CheckLog(ELogLevel.Error)) return;
            var formateLog = GetLogFormate(ELogLevel.Error, logChannel, log);
            Debug.LogError(formateLog);
        }

        public void LogException(string log,ELogChannel logChannel)
        {
            if (!CheckLog(ELogLevel.Exception)) return;
            var formateLog = GetLogFormate(ELogLevel.Exception, logChannel, log);
            Debug.LogError(formateLog);
        }

        public void LogFatal(string log,ELogChannel logChannel)
        {
            if (!CheckLog(ELogLevel.Fatal)) return;
            var formateLog = GetLogFormate(ELogLevel.Fatal, logChannel, log);
            Debug.LogError(formateLog);
        }


        public void Init(params object[] param)
        {
            //todo:读取logChannel 配置
            Application.logMessageReceived += OnReceivedLog;
        }

        private void OnReceivedLog(string condition, string stackTrace, LogType logType)
        {
            //todo：输出log 时去掉颜色
            var filePath = Application.dataPath + "/" + FilePath;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllLines(filePath,new string[]{condition,stackTrace});
        }
        
        public void Update()
        {
        }

        public void Clear()
        {
        }
        public void Dispose()
        {
        }

        private bool CheckLog(ELogLevel logLevel)
        {
            if (!EnableLog)
                return false;

            //todo:log channel
            return (int)logLevel >= LogLevel ;
        }

        private string GetColorLog(ELogChannel logChannel,string log)
        {
            var color = LogChannelColor[logChannel];
            var colorHex = ColorUtility.ToHtmlStringRGB(color);
            return $"<color=#{colorHex}>{log}</color>";
        }

        private string GetLogFormate(ELogLevel logLevel,ELogChannel logChannel,string log)
        {
            var realLog = $"[{DateTime.Now:MM-dd HH:mm:ss}] [{logLevel.ToString()}] : [{logChannel.ToString()}] : {log}";
            if (EnableLogColor)
            {
                return GetColorLog(logChannel, realLog);
            }
            return realLog;
        }
    }
}