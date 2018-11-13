using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using SE_cw1_maria;

namespace Data
{
    [Serializable()]
    public class JsonSave
    {
        public void outputFile(List<Message> message)
        {
            using (StreamWriter file = File.CreateText(@"../../../output.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, message);
            }
        }
    }
}