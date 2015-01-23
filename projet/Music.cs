using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSurface
{
    class Music
    {
        private String id;
        private String title;
        private String artist;
        private String genre;
       

        public Music()
        {

        }

        public Music(String id, String title, String artist, String genre)
        {
            this.id = id;
            this.title = title;
            this.artist = artist;
            this.genre = genre;
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        public String Genre
        {
            get { return genre; }
            set { genre = value; }
        }

    }
}
