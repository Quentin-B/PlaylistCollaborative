using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSurface
{
    public class FileDeLecture
    {
        private static FileDeLecture instance;
        private LinkedList<Song> fileLecture;
        private int current_index;

        public int Current_index
        {
            get { return current_index; }
            set { 
                current_index = value;
                if (current_index < 0)
                    current_index = 0;
                //if (current_index > fileLecture.Count - 1)
                //    current_index = fileLecture.Count - 1;

            }
        }

        public LinkedList<Song> FileLecture
        {
            get { return fileLecture; }
            set { fileLecture = value; }
        }  

        private FileDeLecture()
        {
            fileLecture = new LinkedList<Song>();
            current_index = 0;
        }

        public static FileDeLecture Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileDeLecture();
                }
                return instance;
            }
        }

        public void Add(Song s)
        {
            fileLecture.AddLast(s);
        }

        public void remove(Song s)
        {
            fileLecture.Remove(s);
        }

        public void removeCurrent()
        {
            Song s = getCurrentSong();
            if(s==null)
                return;

            fileLecture.Remove(s);
        }

        public Song Next()
        {
            if (Current_index == fileLecture.Count - 1)
                return null;
            Current_index++;
            return getCurrentSong();
        }

        public Song Previous()
        {
            Current_index--;
            return getCurrentSong();
        }

        public Song getCurrentSong()
        {
            try
            {
                return fileLecture.ElementAt(current_index);
            }
           catch(ArgumentOutOfRangeException e)
            {
                return null;
            }
        }

        public bool isEmpty()
        {
            return fileLecture.Count == 0;
        }

        public int getIndexSong(Song s)
        {
            for (int i = 0; i < fileLecture.Count; i++)
                if (fileLecture.ElementAt(i).Id.Equals(s.Id))
                    return i;
            return -1;
        }
    }
}
