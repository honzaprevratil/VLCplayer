using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCplayer.convertor.FileTypeWork
{
    public class CsvWork : IFileHelper
    {
        public List<T> ReadFile<T>(string path, List<T> DataList) where T : DataClass
        {
            FileHelperEngine<T> engine = new FileHelperEngine<T>();
            DataClass[] res;

            if (File.Exists(path))
            {
                res = engine.ReadFile(path);

                DataList.Clear();
                foreach (T record in res)
                {
                    DataList.Add(record);
                }
            }
            return DataList;
        }

        public void WriteFile<T>(string path, List<T> DataList) where T : DataClass
        {
            FileHelperEngine<T> engine = new FileHelperEngine<T>();
            if (!File.Exists(path))
            {
                FileStream x = File.Create(path);
                x.Close();
            }
            engine.WriteFile(path, DataList);
        }
    }
}
