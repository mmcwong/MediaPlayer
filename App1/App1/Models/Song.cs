using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Models
{
    public class Song
    {
        public String name;
        public String artist;

        public Song (String name, String artist)
        {
            this.name = name;
            this.artist = artist;
        }

        public String Name {
            get {
                return name;
            }
        }
    }
}
