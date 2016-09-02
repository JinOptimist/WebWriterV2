using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using WebWriterV2.VkUtility;

namespace WebWriterV2.GetUserFromJsonFile
{
    public class Magic
    {
        public static List<ExternalUserModel> DoMagic()
        {
            var result = new List<ExternalUserModel>();

            var files = Directory.GetFiles(@"c:\Users\Pavel_Lvou@epam.com\Documents\Visual Studio 2013\Projects\WebWriterV2\WebWriterV2\Json\");
            foreach (var filePath in files)
            {
                var json = File.ReadAllText(filePath);
                var user = SerializeHelper.Deserialize<ExternalUserModel>(json);
                result.Add(user);
            }

            return result;
        }
    }
}