using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSurface
{
    public class Song
    {
        String location;

        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        String artist;

        public String Artist
        {
            get { return artist; }
            set { artist = value; }
        }
        String category;

        public String Category
        {
            get { return category; }
            set { category = value; }
        }

        public Song(String name, String artist, String location, String category = null)
        {
            this.name = name;
            this.artist = artist;
            this.location = location;
            this.category = category;
        }

    }
}
