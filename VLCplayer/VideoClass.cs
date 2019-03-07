using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCplayer.convertor;

namespace VLCplayer
{
    public class VideoClass : DataClass
    {
        public string Path;

        public string GetName()
        {
            string[] nameArr = Path.Split('/');
            return nameArr[nameArr.Length-1];
        }
    }
}
