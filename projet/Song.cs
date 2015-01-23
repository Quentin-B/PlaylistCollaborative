using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSurface
{
    public class Song
    {
        String id;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

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

        int like;

        public int Like
        {
            get { return like; }
            set { like = value; }
        }

        public Song(String name, String artist, String location, String category = null)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this.artist = artist;
            this.location = location;
            this.category = category;
            this.like = 0;
        }

    }
}
