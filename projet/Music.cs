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
    }
}
