using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCplayer.convertor.FileTypeWork
{
    public interface IFileHelper
    {
        List<T> ReadFile<T>(string path, List<T> DataList) where T : DataClass;

        void WriteFile<T>(string path, List<T> DataList) where T : DataClass;
    }
}
