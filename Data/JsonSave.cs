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

        public List<object> loadFile()
        {
            List<object> objects = new List<object>();

            using (StreamReader r = new StreamReader(@"../../../output.json"))
            {
                string json = r.ReadToEnd();
                objects = JsonConvert.DeserializeObject<List<object>>(json);
            }

            return objects;
        }
    }
}
