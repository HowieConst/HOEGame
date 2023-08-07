using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HOEngine.Editor.PackResources
{
    public class HOEPackItem :SerializedScriptableObject
    {
        //子路径
        public string SubFolder;
        //BundleName
        public string BundleName;
        //搜索路径
        public string[] SearchFilters;

        public string[] NecessaryFilters;

        public string[] ExcludeFilters;
        
        public bool SearchSubDir;

        public bool CheckDependency;

        public HashSet<string> BuildSrcFileList(string srcPath)
        {
            return null;
        }
        bool IsMatchNecessary(string str)
        {
            if (NecessaryFilters.Length == 0)
                return true;
            for (int i = 0; i < NecessaryFilters.Length; i++)
            {
                var filter = NecessaryFilters[i];
                if (str.Contains(filter))
                {
                    return false;
                }
            }

            return false;
        }
    }
}