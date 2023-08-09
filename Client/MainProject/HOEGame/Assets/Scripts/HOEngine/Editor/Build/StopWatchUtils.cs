using System.Collections.Generic;
using System.Diagnostics;

namespace HOEngine.Editor
{
    public static class StopWatchUtils
    {
        private static Dictionary<string, StopWatcher> StopWatchers = new Dictionary<string, StopWatcher>();
        public static void StartWatch(string typeFlag)
        {
           var watcher = ReferencePool.Acquire<StopWatcher>();
           watcher.SetTypeFlag(typeFlag);
           watcher.Start();
           if (StopWatchers.TryGetValue(typeFlag, out var stopWatcher))
           {
               Stop(typeFlag);
               StopWatchers[typeFlag] = watcher;
           }
           else
           {
               StopWatchers.Add(typeFlag,watcher);
           }
        }

        public static string Stop(string typeFlag)
        {
            if (StopWatchers.TryGetValue(typeFlag, out var stopWatcher))
            {
                stopWatcher.Stop();
                var watcherInfo = stopWatcher.GetWatcherInfo();
                ReferencePool.Release(stopWatcher);
                return watcherInfo;
            }

            return "";
        }
     
    }

    public class StopWatcher :IReference
    {
        private string TypeFlag;
        private Stopwatch Stopwatch;

        public StopWatcher()
        {
            
        }

        public void SetTypeFlag(string typeFlag)
        {
            TypeFlag = typeFlag;
        }
        public void Start()
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public void Stop()
        {
            Stopwatch.Stop();
        }

        public string GetWatcherInfo()
        {
            var time = Stopwatch.ElapsedMilliseconds;
            return $"{TypeFlag} Used Milliseconds : {time}";
        }

        public void Clear()
        {
            TypeFlag = string.Empty;
            Stopwatch = null;
        }
    }
}