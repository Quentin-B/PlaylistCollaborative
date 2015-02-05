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
        public enum Category { POP_ROCK, TECHNO, ANNEES_70, ANNEES_80, ANNEES_90, DUBSTEP, REGGAE };

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
        Category _category;

        public Category _Category
        {
            get { return _category; }
            set { _category = value; }
        }

        int like;

        public int Like
        {
            get { return like; }
            set { like = value; }
        }

        public Song(String name, String artist, String location, Category category)
        {
            this.id = "id"+Guid.NewGuid().ToString("N");
            this.name = name;
            this.artist = artist;
            this.location = location;
            this._category = category;
            this.like = 0;
        }

    }
}
