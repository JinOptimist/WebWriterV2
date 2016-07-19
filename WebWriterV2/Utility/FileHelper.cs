using System.IO;

namespace WebWriterV2.Utility
{
    public class FileHelper
    {
        public static void CreateBackup()
        {
            
        }

        public static void SaveHtmlFile(string name, string content)
        {
            var path = string.Format("{0} {1}", "asd", name);
            using (var text = File.CreateText(path))
            {
                text.WriteLine(content);
            }
        }
    }
}