using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Vlc.DotNet;
using System.Diagnostics;

namespace VLCplayer
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VideoVM VideoVM { get; set; } = new VideoVM();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = VideoVM;

            var CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var vlcLibDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            MyControl.MediaPlayer.VlcLibDirectory = vlcLibDirectory;
            MyControl.MediaPlayer.EndInit();

            MyControl.MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            MyControl.MediaPlayer.LengthChanged += MediaPlayer_LengthChanged;

            PlayVideo(new Uri(@"D:\source\repos\VLCplayer\videos\big_buck_bunny_480p_h264.mov"));
        }

        private void MediaPlayer_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            VideoVM.VideoLength = e.NewLength;
            Debug.WriteLine(e.NewLength);
        }

        private void MediaPlayer_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
            VideoVM.VideoTime = e.NewTime;
        }

        public void PlayVideo(Uri UriOfVideo)
        {
            MyControl.MediaPlayer.Play(UriOfVideo);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyControl.MediaPlayer.IsPlaying)
            {
                MyControl.MediaPlayer.Pause();
                PauseButtonImage.Source = new BitmapImage(new Uri("pack://application:,,,/VLCplayer;component/pictures/play.png"));
            }
            else
            {
                MyControl.MediaPlayer.Play();
                PauseButtonImage.Source = new BitmapImage(new Uri("pack://application:,,,/VLCplayer;component/pictures/pause.png"));
            }

            Debug.WriteLine(TimeSlider.Value);
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            MyControl.MediaPlayer.Time += 5000;
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            MyControl.MediaPlayer.Time -= 5000;
        }

        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension 
                DefaultExt = ".mov",
                Filter = "MOV Files (*.mov)|*.mov|MP4 Files (*.mp4)|*.mp4"
            };

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                Console.WriteLine(dlg.FileName);
                PlayVideo(new Uri(dlg.FileName));
            }
        }
    }
}
