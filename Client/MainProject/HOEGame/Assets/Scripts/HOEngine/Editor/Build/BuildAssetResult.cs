using System.Collections.Generic;
namespace HOEngine.Editor
{
    public static class BuildAssetResult
    {
        private static Dictionary<string, object[]> BuildAssetResutDic;

        public static void Start()
        {
            if (BuildAssetResutDic == null)
            {
                BuildAssetResutDic = new Dictionary<string, object[]>();
            }
            BuildAssetResutDic.Clear();
        }

        public static void PushBuildAssetResult(string buildAssetStepName,params object[] result)
        {
            if (!BuildAssetResutDic.ContainsKey(buildAssetStepName))
            {
                BuildAssetResutDic.Add(buildAssetStepName,result);
            }
        }

        public static object[] PopBuildAssetResult(string buildAssetStepName,params object[] result)
        {
            if (BuildAssetResutDic.TryGetValue(buildAssetStepName,out var datas))
            {
               return datas;
            }

            return null;
        }

        public static void Clear()
        {
            BuildAssetResutDic?.Clear();
        }
    }
}