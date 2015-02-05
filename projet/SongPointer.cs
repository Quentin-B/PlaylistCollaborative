using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSurface
{
    public class SongPointer
    {
        String id;
        float duration;
        float position;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public float Duration{
            get{return duration;}
            set{duration = value;}
        }

        public float Position{
            get{return position;}
            set{position = value;}
        }


      

        public SongPointer(String id, float duration, float position)
        {
            this.id = id;
            this.duration = duration;
            this.position = position;

        }

    }
}
