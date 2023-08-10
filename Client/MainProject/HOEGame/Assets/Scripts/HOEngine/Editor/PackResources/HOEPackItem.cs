using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;

namespace HOEngine.Editor.PackResources
{
    [Serializable]
    public class HOEPackItem 
    {
        public string SubFolder;
        //BundleName
        public string BundleName;

        //pack 方式
        public EPackType PackType;
        //打包内容
        public EPackContent PackContent;
        //搜索路径
        public string[] SearchFilters;

        public string[] NecessaryFilters;

        public string[] ExcludeFilters;
        
        public bool SearchSubDir;

        public bool CheckDependency;

        
        [ListDrawerSettings(IsReadOnly = true)]
        public List<string> viewAssetsList;


        public void ViewAssets(string srcPath)
        {
            viewAssetsList = BuildSrcFileList(srcPath);
        }
        
        private  bool IsFileValid(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            return !extension.Contains(".dll") && !extension.Contains(".cs") && !extension.Contains(".js") &&
                   !extension.Contains(".psd") 
                   && !extension.Contains(".meta");
        }

     
        bool IsMatchNecessary(string srcPath)
        {
            if (NecessaryFilters.Length == 0)
                return true;
            foreach (var filter in NecessaryFilters)
            {
                if (!srcPath.Contains(filter))
                {
                    return false;
                }
            }
            return true;
        }
        bool IsMatchExclude(string srcPath)
        {
            if (ExcludeFilters.Length == 0)
                return true;
            foreach (var filter in ExcludeFilters)
            {
                if (srcPath.Contains(filter))
                {
                    return false;
                }
            }
            return true;
        }
        public List<string> BuildSrcFileList(string srcPath)
        {
            var fileList = new HashSet<string>();
            var dir = Path.Combine(srcPath, SubFolder);
            //没有通配符
            var option = SearchSubDir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            if (SearchFilters.Length == 0)
            {
                var files = Directory.GetFiles(dir,"*.*" ,option);
                foreach (var file in files)
                {
                    var path = file.Replace("\\", "/");
                    if (IsFileValid(path) && IsMatchNecessary(path) && IsMatchExclude(path))
                    {
                        fileList.Add(path);
                    }
                }
            }
            else
            {
                for (int i = 0; i < SearchFilters.Length; i++)
                {
                    var filter = SearchFilters[i];
                    if(string.IsNullOrEmpty(filter))
                        continue;

                    if (filter.StartsWith("*."))
                    {
                        var files = Directory.GetFiles(dir, filter, option);

                        foreach (var file in files)
                        {
                            var path = file.Replace("\\","/");
                            if (IsFileValid(path) && IsMatchNecessary(path) && IsMatchExclude(path))
                            {
                                fileList.Add(path);
                            }
                        }
                    }
                    else
                    {
                        var filePath = Path.Combine(dir, filter);
                        if (File.Exists(filePath))
                        {
                            filePath = filePath.Replace("\\","/");
                            if (IsFileValid(filePath) && IsMatchNecessary(filePath) && IsMatchExclude(filePath))
                            {
                                fileList.Add(filePath);
                            }
                        }
                    }
                }
            }
            return fileList.ToList();
        }
    }
}