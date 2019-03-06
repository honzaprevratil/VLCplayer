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
                OnPropertyChanged("VideoLengthFormat");
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
                OnPropertyChanged("VideoTimeFormat");
            }
        }

        public string VideoTimeFormat
        {
            get { return FormatSeconds(videoTime); }
        }
        public string VideoLengthFormat
        {
            get { return FormatSeconds((long)videoLength); }
        }

        public string FormatSeconds(long miliseconds)
        {
            int seconds = (int)Math.Round((float)miliseconds / 1000);
            string format = "";
            if (seconds > 3599)
            {
                format += ((seconds - seconds % 3600) / 3600).ToString() + ":";
                seconds = seconds % 3600;
            }
            int minutes = ((seconds - seconds % 60) / 60);
            seconds = seconds % 60;
            format += ((minutes > 9) ? minutes.ToString() : ("0" + minutes.ToString())) + ":" + ((seconds > 9) ? seconds.ToString() : ("0" + seconds.ToString()));
            return format;
        }

        /* EVENT AND FUNCTION FOR EVENT */
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
