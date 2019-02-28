using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCplayer.convertor.FileTypeWork
{
    public class JsonWork : IFileHelper
    {
        public List<T> ReadFile<T>(string path, List<T> DataList) where T : DataClass
        {
            DataList.Clear();
            string[] JsonText = System.IO.File.ReadAllLines(path);
            foreach (string line in JsonText)
            {
                DataList.Add(JsonConvert.DeserializeObject<T>(line));
            }
            return DataList;
        }

        public void WriteFile<T>(string path, List<T> DataList) where T : DataClass
        {
            List<string> lines = new List<string>();
            foreach (DataClass line in DataList)
            {
                string json = JsonConvert.SerializeObject(line);
                lines.Add(json);
            }

            int len = lines.Count();
            string[] jsonLines = new string[len];

            for (int i = 0; i < jsonLines.Length; i++)
            {
                jsonLines[i] = lines[i];
            }
            System.IO.File.WriteAllLines(path, jsonLines);
        }
    }
}
