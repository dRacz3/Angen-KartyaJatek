using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{

    // Kártyát leíró tulajdonságok
    enum Szin { Piros, Zold, Tok, Makk };

    // Kártya értékei és String reprezentációja érték szerint rendezve
    enum Ertek
    {
        II = 1, III = 2, IV = 3,
        V = 4, VI = 5, VII = 6,
        VIII = 7, IX = 8, J = 9,
        Q = 10, Kiraly = 11, Asz = 12,

    };

    class Kartya
    {
        public Szin Szin { get; }
        public Ertek Ertek { get; }
        public Kartya(Szin szin, Ertek ertek)
        {
            this.Szin = szin;
            this.Ertek = ertek;
        }


        // Formázó metódus.. Rövidít ha kell a kompakt kiírás érdekében
        public String KartyaNeve()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Szin.ToString()[0] + ":");

            if(Ertek.ToString().Length >4)
            {
                sb.Append(Ertek.ToString()[0]);
            }
            else
            {
            sb.Append(Ertek.ToString());
            }

            return sb.ToString();
        }

        // Másoló metódus, ha nem szeretnénk a referenciát kiajánlani a kontextuson kívülre.
        public Kartya Copy()
        {
            return new Kartya(this.Szin, this.Ertek);
        }
    }

    // Wrapper osztály a kártyák köré.
    class KartyaPakli
    {
        // Kártyahúzást imitáló logikát a Queue kezeli
        public Queue<Kartya> Pakli { get; set; }
        private static Random rng = new Random();

        public KartyaPakli()
        {
            List<Kartya> kartyak = UjPakli();
            Kever(kartyak);
            Pakli = new Queue<Kartya>(kartyak);
        }


        // Uj paklit kész...
        public static List<Kartya> UjPakli()
        {
            List<Kartya> kartyak = new List<Kartya>();
            foreach (Szin s in Enum.GetValues(typeof(Szin)))
            {
                foreach (Ertek e in Enum.GetValues(typeof(Ertek)))
                {
                    kartyak.Add(new Kartya(s, e));
                }
            }
            return kartyak;
        }

        // Ref: https://stackoverflow.com/questions/273313/randomize-a-listt
        // Based on : https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        // Megkeveri az adott paklit.
        public void Kever(List<Kartya> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Kartya value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }



    }


   

}
