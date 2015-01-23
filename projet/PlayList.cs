using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSurface
{
    class PlayList
    {
        private static PlayList instance;

        private Dictionary<String, Song> playlistDic;

        public Dictionary<String, Song> PlaylistDic
        {
            get { return playlistDic; }
            set { playlistDic = value; }
        }

        private PlayList()
        {
            playlistDic = new Dictionary<string, Song>();
        }

        public static PlayList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayList();
                }
                return instance;
            }
        }

        public void Add(String id, Song s)
        {
            playlistDic.Add(id, s);
        }

        public Song getSongById(String id)
        {
            return playlistDic[id];
        }

        public int plusASong(String id)
        {
            Song s = getSongById(id);
            try
            {
                s.Like++;

            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Methode plusASong, id_song : " + id + " not found.");
                Debug.WriteLine("Exception Message: " + e.Message);
            }

            return s.Like;
        }
    }
}
