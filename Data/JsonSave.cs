using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Data
{
    [Serializable()]
    public class JsonSave
    {
        public void outputFile(List<object> message)
        {
            using (StreamWriter file = File.CreateText(@"../../../output.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, message);
            }
        }
    }
}
