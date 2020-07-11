using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Program
    {
        enum kek { kekw, lol, qq, qe}


        static void Main(string[] args)
        {
            Console.WriteLine(kek.kekw.ToString());




            //var d1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            //var d2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);

            //Console.WriteLine(d1.Date);
            //Console.WriteLine(d2.Date);

            //if (d2 > d1)
            //{
            //    Console.WriteLine("еее");
            //}
            //else
            //{
            //    Console.WriteLine("slaigh bells");
            //}


            //var q = new List<BMO>();

            //q.Add(new Song());
          
            //MusicContainer music = new LF();
            //q.Add(new LF());

            //q.Add(new Song());

            //q.Add(new LF());

            //var t = q.Select(x => x is Song == true).ToList();
            //var t1 = (from temp in q where temp is Song select temp).ToList();  


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
