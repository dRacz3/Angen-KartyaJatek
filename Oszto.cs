using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{
    class Oszto
    {

        KartyaPakli pakli;

        // Alap konstruktor ami új paklit csinál
        public Oszto()
        {
            pakli = new KartyaPakli();
        }


        // Pakli tetejéről (sor elejéről) vesz egy kártyát
        public Kartya OsztEgyKartyat()
        {

            return pakli.Pakli.Dequeue();
        }

        // Igaz, ha van meg kartya
        // Hamis egyébként
        public bool VanMegKartya()
        {
            return (pakli.Pakli.Count == 0);
        }



    }
}
