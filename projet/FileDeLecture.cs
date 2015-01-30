﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSurface
{
    class FileDeLecture
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

        public void Next()
        {
            current_index++;
        }

        public void Previous()
        {
            current_index--;
        }

        public void getCurrentSong()
        {
            fileLecture.ElementAt(current_index);
        }

    }
}