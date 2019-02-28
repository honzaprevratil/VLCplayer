using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCplayer.convertor.FileTypeWork;

namespace VLCplayer.convertor
{
    public static class Convertor
    {
        static IFileHelper CsvWorker = new CsvWork();
        static IFileHelper JsonWorker = new JsonWork();

        public static List<T> Read<T>(string input) where T : DataClass
        {
            List<T> DataList = new List<T>();

            string inputType = input.Substring(input.Length - 4, 4);

            switch (inputType)
            {
                case ".csv":
                    DataList = CsvWorker.ReadFile(input, DataList);
                    break;

                case "json":
                    JsonWorker.ReadFile(input, DataList);
                    break;
            }

            return DataList;
        }

        public static bool Write<T>(string output, List<T> DataList) where T : DataClass
        {
            string outputType = "";
            if (output.Length >= 4)
                outputType = output.Substring(output.Length - 4, 4);

            switch (outputType)
            {
                case ".csv":
                    CsvWorker.WriteFile(output, DataList);
                    return true;

                case "json":
                    JsonWorker.WriteFile(output, DataList);
                    return true;

                default:
                    return false;
            }
        }

        public static List<T> Convert<T>(string input, string output = "") where T : DataClass
        {
            List<T> DataList = Read<T>(input);
            Write<T>(output, DataList);
            return DataList;
        }
    }
}
