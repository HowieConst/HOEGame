using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using Unity.Plastic.Newtonsoft.Json.Bson;
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

        public Dictionary<string, List<string>> PackAllAssets()
        {
            if (PackItems.Count == 0)
                return null;
            var result = new Dictionary<string, List<string>>();
            for (int i = 0; i < PackItems.Count; i++)
            {
                var packItem = PackItems[i];
                PackAsset(packItem,ref result);
            }

            return result;
        }

        private void PackAsset(HOEPackItem packItem,ref Dictionary<string,List<string>> result)
        {
            var fileList = packItem.BuildSrcFileList(SrcDir);
            if(fileList.Count == 0)
                return;
            var bundleName = packItem.BundleName;
            switch (packItem.PackType)
            {
                case EPackType.One:
                    PackOneBundle(bundleName,fileList,ref result);
                    break;
                case EPackType.PerFile:
                    PackPerFileBundle(bundleName,fileList, ref result);
                    break;
                case EPackType.PerDir:
                    PackPerDirBundle(bundleName, fileList, ref result);
                    break;
                case EPackType.Scene:
                    break;
                case EPackType.Shader:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PackOneBundle(string BundleName,List<string> fileList,ref Dictionary<string,List<string>> result)
        {
            if (!string.IsNullOrEmpty(BundleName))
            {
                if (!result.ContainsKey(BundleName))
                {
                    result.Add(BundleName,fileList.ToList());
                }
            }
        }

        private void PackPerFileBundle(string bundleName,List<string> fileList, ref Dictionary<string, List<string>> result)
        {
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
                }
                else
                {
                    if (result.ContainsKey(fileName))
                    {
                        result.Add(fileName, new List<string>() {file});
                    }
                }
            }
        }

        private void PackPerDirBundle(string bundleName,List<string> fileList, ref Dictionary<string, List<string>> result)
        {
            var dirInfo = new Dictionary<string, List<string>>();

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
            }
        }
    }
}