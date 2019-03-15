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
using VLCplayer.dialog;

namespace VLCplayer
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VideoVM VideoVM { get; set; } = new VideoVM();
        PlaylistProvider PlaylistProvider { get; set; } = new PlaylistProvider();
        private List<MenuItem> DynamicGrid { get; set; }

        bool wasPlayingBeforeDrag = false;
        static string[] mediaExtensions = { ".mov", ".mp4" };

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

            //VideoVM.PlayingVideoPath = @"D:\source\repos\VLCplayer\videos\big_buck_bunny_480p_h264.mov";
            //PlayVideo(new Uri(VideoVM.PlayingVideoPath));
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            addVideoTItem.Items.Clear();
            openPlaylistItem.Items.Clear();

            foreach (string ListName in PlaylistProvider.PlayLists)
            {
                MenuItem MenuItem1 = new MenuItem
                {
                    Header = ListName
                };
                MenuItem1.Click += MenuItem_AddToPlayList;

                MenuItem MenuItem2 = new MenuItem
                {
                    Header = ListName
                };
                MenuItem2.Click += MenuItem_OpenPlayList;

                addVideoTItem.Items.Add(MenuItem1);
                openPlaylistItem.Items.Add(MenuItem2);
            }

            if (PlaylistProvider.PlayLists.Count > 1)
            {
                addVideoTItem.IsEnabled = true;
                openPlaylistItem.IsEnabled = true;
            }
            else
            {
                addVideoTItem.IsEnabled = false;
                openPlaylistItem.IsEnabled = false;
            }

        }

        private void MediaPlayer_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            VideoVM.VideoLength = e.NewLength / 10000;
        }

        private void MediaPlayer_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
            VideoVM.VideoTime = e.NewTime;
        }

        public void PlayVideo(Uri UriOfVideo)
        {
            VideoVM.PlayingVideoPath = UriOfVideo.OriginalString;
            if (IsMediaFile(UriOfVideo.OriginalString))
            {
                MyControl.MediaPlayer.Play(UriOfVideo);
            }
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

        private void TimeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            MyControl.MediaPlayer.Time = (long)((sender as Slider).Value);
            if (!MyControl.MediaPlayer.IsPlaying && wasPlayingBeforeDrag)
                PauseButton_Click(null, null);
            wasPlayingBeforeDrag = false;
        }

        private void TimeSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (MyControl.MediaPlayer.IsPlaying)
            {
                wasPlayingBeforeDrag = true;
                PauseButton_Click(null, null);
            }
            MyControl.MediaPlayer.Time = (long)((sender as Slider).Value);
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoVM.VideoLength > MyControl.MediaPlayer.Time + 30000)
            {
                MyControl.MediaPlayer.Time += 30000;
                VideoVM.VideoTime = MyControl.MediaPlayer.Time;
            }
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyControl.MediaPlayer.Time >= 5000)
            {
                MyControl.MediaPlayer.Time -= 5000;
                VideoVM.VideoTime = MyControl.MediaPlayer.Time;
            }
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

            // Get the selected file name and playvideo 
            if (result == true)
            {
                VideoVM.PlayingVideoPath = dlg.FileName;
                PlayVideo(new Uri(VideoVM.PlayingVideoPath));
                PlayingInfo.Header = "Playing: none video list";
            }
            RenderNextAndPrevious();
        }

        private void MenuItem_AddToPlayList(object sender, RoutedEventArgs e)
        {
            PlaylistProvider.AddToPlayList((sender as MenuItem).Header.ToString(), VideoVM.PlayingVideoPath);
            RenderNextAndPrevious();
        }

        private void MenuItem_OpenPlayList(object sender, RoutedEventArgs e)
        {
            string PlayListName = (sender as MenuItem).Header.ToString();
            VideoClass VideoToPlay = PlaylistProvider.Load(PlayListName);
            PlayVideo(new Uri(VideoToPlay.Path));
            RenderNextAndPrevious();
            PlayingInfo.Header = "Playing: " + PlayListName + " [1/" + PlaylistProvider.CurrentPlaylist.Count + "]";
        }

        private void MenuItem_AddToNewPlayList(object sender, RoutedEventArgs e)
        {
            var dialog = new MyDialog();

            dialog.Owner = this;
            MyControl.MediaPlayer.Pause();

            if (dialog.ShowDialog() == true)
            {
                PlaylistProvider.CreateNew(dialog.ResponseText, VideoVM.PlayingVideoPath);
                MyControl.MediaPlayer.Play();
            }
            else
            {
                MyControl.MediaPlayer.Play();
            }
            Debug.WriteLine(dialog.ResponseText);
            RenderNextAndPrevious();
            PlaylistProvider.LoadAllLists();
            GenerateGrid();
        }

        private void MenuItem_Next(object sender, RoutedEventArgs e)
        {
            if (PlaylistProvider.CurrentVideoIndex < PlaylistProvider.CurrentPlaylist.Count - 1)
            {
                PlaylistProvider.CurrentVideoIndex++;
                PlayVideo(new Uri (PlaylistProvider.CurrentPlaylist[PlaylistProvider.CurrentVideoIndex].Path));
            }
            RenderNextAndPrevious();
        }

        private void MenuItem_Previous(object sender, RoutedEventArgs e)
        {
            if (PlaylistProvider.CurrentVideoIndex > 0)
            {
                PlaylistProvider.CurrentVideoIndex--;
                PlayVideo(new Uri(PlaylistProvider.CurrentPlaylist[PlaylistProvider.CurrentVideoIndex].Path));
            }
            RenderNextAndPrevious();
        }

        private void RenderNextAndPrevious()
        {
            if (PlaylistProvider.CurrentVideoIndex == 0)
                Previous.IsEnabled = false;
            else
                Previous.IsEnabled = true;

            if (PlaylistProvider.CurrentVideoIndex == PlaylistProvider.CurrentPlaylist.Count - 1)
                Next.IsEnabled = false;
            else
                Next.IsEnabled = true;

            PlayingInfo.Header = "Playing: " + PlaylistProvider.CurrentPlaylistName + " [" + (PlaylistProvider.CurrentVideoIndex + 1) + "/" + PlaylistProvider.CurrentPlaylist.Count + "]";
        }

        static bool IsMediaFile(string path)
        {
            if (File.Exists(path))
            {
                return -1 != Array.IndexOf(mediaExtensions, Path.GetExtension(path).ToLower());
            }
            else
                return false;
        }
    }
}
