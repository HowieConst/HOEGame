using System.IO;
using UnityEngine;
using YamlDotNet.RepresentationModel;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using NUnit.Framework;

namespace HOEngine.Editor
{
    public class GenerateBuildMapStep : IBuildAssetStep
    {
        public EBuildAssetStep BuildStep => EBuildAssetStep.GenerateBuildMapStep;
        private const string ManifestSuffix = ".manifest";
        private List<string> AllBundlesList = new List<string>();
        private Dictionary<string, List<string>> BundleAssets = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> BundleDependencies = new Dictionary<string, List<string>>();
        private string BundleOutPutPath;
        private const string BundleMapName = "BundleMap.txt";
        private const string BundleInfo_Prefix = "&BundleInfo%;";

        public ReturnCode Run(IBuildAssetContent content)
        {
            var pathResult = BuildAssetResult.PopBuildAssetResult(EBuildAssetStep.BuildAssetPrepareStep);
            if (pathResult == null || pathResult.Length == 0)
                return ReturnCode.SuccessNotReturn;
            BundleOutPutPath= pathResult[0] as string;
            if (string.IsNullOrEmpty(BundleOutPutPath))
                return ReturnCode.SuccessNotReturn;

            var index = BundleOutPutPath.LastIndexOf('/');
            if (index < 0)
                return ReturnCode.SuccessNotReturn;
            var manifestName = BundleOutPutPath.Substring(index + 1);
            if (string.IsNullOrEmpty(manifestName))
                return ReturnCode.SuccessNotReturn;

            var fileName = $"{manifestName}{ManifestSuffix}";

            var filePath = BundleOutPutPath + "/" + fileName;

            if (!File.Exists(filePath))
                return ReturnCode.Error;

            var yamlReader = new YamlReader(filePath);
            var mappingNode = yamlReader.Read();
            if (mappingNode == null)
                return ReturnCode.Error;

            var bundleManifest = (YamlMappingNode)mappingNode.Children["AssetBundleManifest"];
            var bundleInfos = (YamlMappingNode)bundleManifest.Children["AssetBundleInfos"];

            foreach (var node in bundleInfos)
            {
                AllBundlesList.Add(node.Value["Name"].ToString());
            }

            for (int i = 0; i < AllBundlesList.Count; i++)
            {
                var bundleName = AllBundlesList[i];
                var manifestFilePath =  $"{BundleOutPutPath}/{bundleName}{ManifestSuffix}";
                ParseBundleManifest(bundleName,manifestFilePath,ref BundleAssets,ref BundleDependencies);
            }

            DeleteAllManifestFile(BundleOutPutPath,manifestName);


            GenerateBundleMap();
            
            return ReturnCode.Success;
        }

        private void ParseBundleManifest(string bundleName,string filePath,ref Dictionary<string,List<string>> assets,ref Dictionary<string,List<string>> dependencies)
        {
            var yamlReader = new YamlReader(filePath);
            var mappingNode = yamlReader.Read();
            if (mappingNode == null)
                return;
            var assetInfos = (YamlSequenceNode)mappingNode.Children["Assets"];
            var dependencyInfos = (YamlSequenceNode)mappingNode.Children["Dependencies"];
            if (assets.TryGetValue(bundleName, out var assetList))
            {
                assetList.Clear();
            }
            else
            {
                assets.Add(bundleName,new List<string>());
            }
            foreach (var asset in assetInfos)
            {
                assets[bundleName].Add(asset.ToString());
            }
            if (dependencies.TryGetValue(bundleName, out var dependencyList))
            {
                dependencyList.Clear();
            }
            else
            {
                dependencies.Add(bundleName,new List<string>());
            }
            foreach (var dependency in dependencyInfos)
            {
                dependencies[bundleName].Add(dependency.ToString());
            }
        }

        private void DeleteAllManifestFile(string BundleOutPath,string manifestpath)
        {
            var files = Directory.GetFiles(BundleOutPath, "*.manifest", SearchOption.AllDirectories);
            foreach (var filePath in files)
            {
                var fileInfo = new FileInfo(filePath);
                fileInfo.Delete();
            }

            var manifestFilePath = $"{BundleOutPath}/{manifestpath}";
            File.Delete(manifestFilePath);
        }

        private void GenerateBundleMap()
        {
            if(AllBundlesList.Count == 0)
                return;
            var bundleMapPath = Path.Combine(BundleOutPutPath, BundleMapName);
            if (File.Exists(bundleMapPath))
            {
                File.Delete(bundleMapPath);
            }

            var bundleMapBuilder = new StringBuilder();
            for (int i = 0; i < AllBundlesList.Count; i++)
            {
                var bundleName = AllBundlesList[i];
                var assetBundleInfo = new PackedBundleItem(BundleOutPutPath, bundleName);
                var bundleStr = $"{assetBundleInfo.Name}|{assetBundleInfo.Size}|{assetBundleInfo.MD5}";
                bundleMapBuilder.AppendLine(bundleStr);
            }

            bundleMapBuilder.AppendLine(BundleInfo_Prefix);
            
            for (int i = 0; i < AllBundlesList.Count; i++)
            {
                var bundleName = AllBundlesList[i];
                var assetList = new List<string>();
                var dependencies = new List<string>();
                if (BundleAssets.TryGetValue(bundleName, out var asset))
                {
                    assetList = asset;
                }

                if (BundleDependencies.TryGetValue(bundleName, out var dependency))
                {
                    if (dependency != null) dependencies = dependency;
                }

                var packedBundleInfo = new PackedBundleInfo(bundleName, assetList, dependencies);
                bundleMapBuilder.Append(packedBundleInfo);
            }
            
            File.WriteAllText(bundleMapPath,bundleMapBuilder.ToString());
        }
    }
}