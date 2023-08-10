using System.Collections.Generic;
namespace HOEngine.Editor
{
    /// <summary>
    /// BuildAsset 结果记录
    /// </summary>
    public static class BuildAssetResult
    {
        private static Dictionary<EBuildAssetStep, object[]> BuildAssetResutDic;

        public static void Start()
        {
            if (BuildAssetResutDic == null)
            {
                BuildAssetResutDic = new Dictionary<EBuildAssetStep, object[]>();
            }
            BuildAssetResutDic.Clear();
        }

        public static void PushBuildAssetResult(EBuildAssetStep buildAssetStep,params object[] result)
        {
            if (!BuildAssetResutDic.ContainsKey(buildAssetStep))
            {
                BuildAssetResutDic.Add(buildAssetStep,result);
            }
            else
            {
                BuildAssetResutDic[buildAssetStep] = result;
            }
        }

        public static object[] PopBuildAssetResult(EBuildAssetStep buildAssetStep,params object[] result)
        {
            if (BuildAssetResutDic.TryGetValue(buildAssetStep,out var datas))
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