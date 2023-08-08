using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace HOEngine.Editor.PackResources
{
    
    [CreateAssetMenu(menuName ="PackResources/Create Pack Mainfest",fileName ="PackMainfest")]
    public class HOEPackMainfest :ScriptableObject 
    {
        //源目录
        public string SrcDir;
        
        public List<HOEPackItem> PackItems;

        private List<string> BuildLog;

        [Button("View Pack Asset")]
        public void ViewPackAssets()
        {
            if (PackItems.Count > 0)
            {
                for (int i = 0; i < PackItems.Count; i++)
                {
                    PackItems[i].ViewAssets(SrcDir);
                }
            }
        }

        public void PackAllAssets(ref Dictionary<string,List<string>> result,ref Dictionary<string,HashSet<string>> denpendencies)
        {
            if (PackItems.Count == 0)
                return ;
            for (int i = 0; i < PackItems.Count; i++)
            {
                var packItem = PackItems[i];
                PackAsset(packItem,ref result,ref denpendencies);
            }
        }
        
        private void PackAsset(HOEPackItem packItem,ref Dictionary<string,List<string>> result,ref Dictionary<string,HashSet<string>> denpendencies)
        {
            switch (packItem.PackType)
            {
                case EPackType.One:
                    PackOneBundle(packItem,ref result,ref denpendencies);
                    break;
                case EPackType.PerFile:
                    PackPerFileBundle(packItem,ref result,ref denpendencies);
                    break;
                case EPackType.PerDir:
                    PackPerDirBundle(packItem,ref result,ref denpendencies);
                    break;
                case EPackType.Scene:
                    break;
                case EPackType.Shader:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PackOneBundle(HOEPackItem packItem,ref Dictionary<string,List<string>> result,ref Dictionary<string,HashSet<string>> dependencies)
        {
            var bundleName = packItem.BundleName;
            var fileList = packItem.BuildSrcFileList(SrcDir);
            if (!string.IsNullOrEmpty(bundleName))
            {
                if (!result.ContainsKey(bundleName))
                {
                    bool isCheckDependency = packItem.CheckDependency;
                    foreach (var item in fileList)
                    {
                        if (isCheckDependency)
                        {
                            var dependenciesArray = AssetDatabase.GetDependencies(item);
                            foreach (var dependecy in dependenciesArray)
                            {
                                if (!dependencies.ContainsKey(dependecy))
                                {
                                    dependencies.Add(dependecy,new HashSet<string>());
                                }
                                dependencies[dependecy].Add(bundleName);
                            }
                        }
                    }
                    result.Add(bundleName,fileList.ToList());
                }
            }
        }
        
        private void PackPerFileBundle(HOEPackItem packItem,ref Dictionary<string, List<string>> result,ref Dictionary<string,HashSet<string>> dependencies)
        {
            var bundleName = packItem.BundleName;
            var fileList = packItem.BuildSrcFileList(SrcDir);
            bool isCustomBundleName = !string.IsNullOrEmpty(bundleName) && bundleName.StartsWith("{0}");
            foreach (var file in fileList)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                if (isCustomBundleName)
                {
                    bundleName = string.Format(bundleName, fileName);
                    if (result.ContainsKey(bundleName))
                    {
                        result.Add(bundleName, new List<string>() {file});
                    }
        
                    if (packItem.CheckDependency)
                    {
                        var dependenciesArray = AssetDatabase.GetDependencies(file);
                        foreach (var dependecy in dependenciesArray)
                        {
                            if (!dependencies.ContainsKey(dependecy))
                            {
                                dependencies.Add(dependecy,new HashSet<string>());
                            }
                            dependencies[dependecy].Add(bundleName);
                        }
                    }
                }
                else
                {
                    if (result.ContainsKey(fileName))
                    {
                        result.Add(fileName, new List<string>() {file});
                    }
                    if (packItem.CheckDependency)
                    {
                        var dependenciesArray = AssetDatabase.GetDependencies(file);
                        foreach (var dependecy in dependenciesArray)
                        {
                            if (!dependencies.ContainsKey(dependecy))
                            {
                                dependencies.Add(dependecy,new HashSet<string>());
                            }
                            dependencies[dependecy].Add(fileName);
                        }
                    }
                }
            }
        }
        
        private void PackPerDirBundle(HOEPackItem packItem,ref Dictionary<string, List<string>> result,ref Dictionary<string,HashSet<string>> dependencies)
        {
            var dirInfo = new Dictionary<string, List<string>>();
            var fileList = packItem.BuildSrcFileList(SrcDir);
            var bundleName = packItem.BundleName;
            foreach (var file in fileList)
            {
                var topdir = Path.GetDirectoryName(file);
                if (!string.IsNullOrEmpty(topdir))
                {
                    if (!dirInfo.ContainsKey(topdir))
                    {
                        dirInfo.Add(topdir,new List<string>());
                    }
                    dirInfo[topdir].Add(file);
                }
            }
            bool isCustomBundleName = !string.IsNullOrEmpty(bundleName) && bundleName.StartsWith("{0}");
            
            foreach (var item in dirInfo)
            {
                var name = item.Key;
                if (isCustomBundleName)
                {
                    name = string.Format(bundleName, name);
                }
        
                if (!result.ContainsKey(name))
                {
                    result.Add(name,item.Value);
                }

                foreach (var file in item.Value)
                {
                    if (packItem.CheckDependency)
                    {
                        var dependenciesArray = AssetDatabase.GetDependencies(file);
                        foreach (var dependecy in dependenciesArray)
                        {
                            if (!dependencies.ContainsKey(dependecy))
                            {
                                dependencies.Add(dependecy,new HashSet<string>());
                            }
                            dependencies[dependecy].Add(name);
                        }
                    }
                }
            }
        }
    }
}