using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCplayer
{
    class VideoVM : INotifyPropertyChanged
    {
        /* Video Length = Slider Maximum */
        private float videoLength;
        public float VideoLength
        {
            get { return videoLength; }
            set
            {
                videoLength = value;
                OnPropertyChanged("VideoLength");
            }
        }

        /* Video current Time = Slider Value */
        private long videoTime;
        public long VideoTime
        {
            get { return videoTime; }
            set
            {
                videoTime = value;
                OnPropertyChanged("VideoTime");
            }
        }

        /* EVENT AND FUNCTION FOR EVENT */
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
