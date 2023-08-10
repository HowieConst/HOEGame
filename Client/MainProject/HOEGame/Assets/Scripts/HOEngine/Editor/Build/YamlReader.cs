using System.IO;
using YamlDotNet.RepresentationModel;

namespace HOEngine.Editor
{
    public class YamlReader
    {
        private string FilePath;
        public YamlReader(string filePath)
        {
            FilePath = filePath;
        }

        public YamlMappingNode Read()
        {
            if(!File.Exists(FilePath))
                return null;
            var text = File.ReadAllText(FilePath);
            if(string.IsNullOrEmpty(text))
                return null;
            var input = new StringReader(text);
            var yaml = new YamlStream();
            yaml.Load(input);

            return (YamlMappingNode)yaml.Documents[0].RootNode;

        }
    }
}