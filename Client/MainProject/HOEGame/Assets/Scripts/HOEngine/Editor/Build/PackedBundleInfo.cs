using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HOEngine.Editor
{
    public class PackedBundleItem
    {
        public string Name;
        public long Size;
        public string MD5;
        private string BundlePath;

        public PackedBundleItem(string bundlePath,string name)
        {
            Name = name;
            var bundleFile = Path.Combine(bundlePath, Name);
            var fileInfo = new FileInfo(bundleFile);
            Size = fileInfo.Length;
            MD5 = GetMD5(bundleFile);
        }
        private string GetMD5(string filePath)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            using (var fileStream = new FileStream(filePath,FileMode.Open,FileAccess.Read))
            {
                var buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                var bytes = md5.ComputeHash(buffer);
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }

    public class PackedBundleInfo
    {
        public string BundleName;
        public List<string> Assets;
        public List<string> Dependencies;

        public PackedBundleInfo(string bundleName,List<string> assets,List<string> denpendencies)
        {
            BundleName = bundleName;
            Assets = assets;
            Dependencies = denpendencies;
        }

        public override string ToString()
        {
            var stringWrite = new StringWriter();
            var memeryStram = new MemoryStream();
            var binaryWrite = new BinaryWriter(memeryStram);
            binaryWrite.Write(BundleName);
            binaryWrite.Write(Assets.Count);
            for (int i = 0; i < Assets.Count; i++)
            {
                var assetName = Assets[i];
                binaryWrite.Write(assetName);
            }
            binaryWrite.Write(Dependencies.Count);
            for (int i = 0; i < Dependencies.Count; i++)
            {
                var dependency = Dependencies[i];
                binaryWrite.Write(dependency);
            }

            var str = Convert.ToBase64String(memeryStram.GetBuffer(), 0, (int)memeryStram.Length);
            stringWrite.Write(str);

            return stringWrite.ToString();
        }
    }

}