using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new List<BMO>();

            q.Add(new Song());
          
            MusicContainer music = new LF();
            q.Add(new LF());

            q.Add(new Song());

            q.Add(new LF());

            var t = q.Select(x => x is Song == true).ToList();
            var t1 = (from temp in q where temp is Song select temp).ToList();  


        }
    }

    public abstract class BMO
    {
        public string Name { get; set; }
    }










    public class Song : BMO
    {

    }



    public abstract class MusicContainer : BMO
    {
        public List<Song> Songs { get; set; } = new List<Song>();

    }

    public class LF : MusicContainer
    { 
    
    
    }


}
