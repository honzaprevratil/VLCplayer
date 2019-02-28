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

namespace VLCplayer
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var vlcLibDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            MyControl.MediaPlayer.VlcLibDirectory = vlcLibDirectory;
            MyControl.MediaPlayer.EndInit();
            MyControl.MediaPlayer.Play(new Uri(@"D:\source\repos\VLCplayer\videos\big_buck_bunny_480p_h264.mov"));
        }

        public void PlayVideo()
        {
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
        }

        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {

            }
        }
    }
}
