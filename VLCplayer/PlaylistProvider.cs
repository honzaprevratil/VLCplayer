using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCplayer.convertor;

namespace VLCplayer
{
    public class PlaylistProvider
    {
        public List<VideoClass> CurrentPlaylist { get; set; } = new List<VideoClass>();
        public string CurrentPlaylistName { get; set; }
        public int CurrentVideoIndex { get; set; }

        public List<string> PlayLists { get; set; } = new List<string>();

        private static readonly string PlayListsPaths = "playlists/";

        public PlaylistProvider()
        {
            LoadAllLists();
        }

        public void CreateNew(string PlaylistName, string VideoPath)
        {
            VideoClass video = new VideoClass() { Path = VideoPath };

            CurrentPlaylistName = PlaylistName;

            string PlayListPath = PlayListsPaths + PlaylistName + ".csv";
            // save playlist to csv file
            Convertor.Write(PlayListPath, new List<VideoClass>() { video });
        }

        public void AddToPlayList(string PlaylistName, string VideoPath)
        {
            VideoClass video = new VideoClass() { Path = VideoPath };

            string PlayListPath = PlayListsPaths + PlaylistName + ".csv";

            // read from playlist from csv
            List<VideoClass> videoList = Convertor.Read<VideoClass>(PlayListPath);

            videoList.Add(video);

            // save playlist to csv file
            Convertor.Write(PlayListPath, videoList);
        }

        public void LoadAllLists()
        {
            // if it doesn't exist, create
            if (!Directory.Exists(PlayListsPaths))
            {
                Directory.CreateDirectory(PlayListsPaths);
                return;
            }

            DirectoryInfo d = new DirectoryInfo(PlayListsPaths);
            FileInfo[] Files = d.GetFiles("*.csv"); //Getting CSV files
            PlayLists.Clear();
            foreach (FileInfo file in Files)
            {
                PlayLists.Add(file.Name.Substring( 0, (file.Name.Length - 4) ));
            }
        }

        public VideoClass Load(string PlaylistName)
        {
            string PlayListPath = PlayListsPaths + PlaylistName + ".csv";
            CurrentPlaylist = Convertor.Read<VideoClass>(PlayListPath);

            CurrentPlaylistName = PlaylistName;

            CurrentVideoIndex = 0;

            if (CurrentPlaylist.Count > 0)
                return CurrentPlaylist[0];
            else
                return new VideoClass();
        }
    }
}
