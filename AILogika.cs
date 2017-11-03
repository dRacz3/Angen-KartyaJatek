using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{
    // AI döntési logikáját tartalmazza... Akar-e játszani, illetve milyen kártyát hívjon
    class AILogika
    {
        static Random rnd = new Random();

        public static bool AIKarEJatszani()
        {
            int miTortenik = rnd.Next(0, 101);
            if (miTortenik < 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        // Ez a metódus megnézi tud-e kijátszani kártyát az AI, ha nem akkor null értéket ad vissza
        // Ha tud kijátszani, akkor visszaadja a kijátszandó kártyát és kiveszi a saját paklijából.
        public static Kartya KartyatKijatszik(Kartya Adu, Jatekos AI, bool felhasznaloAKihivo)
        {

            bool tudMitKijatszani = false;
            int[] kijatszhatoKartyakIndexe = new int[AI.Kartyak.Count];
            int counter = 0;
            int index = 0;
            // Ha a felhasználó a kihívó.. akkor AI másodiknak jön -> magasabb lapot kell raknia
            if (felhasznaloAKihivo)
            {
                foreach (Kartya k in AI.Kartyak)
                {
                    if (JatekLogika.ErvenyesLepesE(Adu, k) && JatekLogika.MagasabbE(Adu, k))
                    {
                        tudMitKijatszani = true;
                        kijatszhatoKartyakIndexe[index] = counter;
                        index++;
                    }
                    counter++;
                }
            }
            // Különben AI kezd, csak a színre kell figyelnie
            else
            {
                foreach (Kartya k in AI.Kartyak)
                {
                    if (JatekLogika.ErvenyesLepesE(Adu, k))
                    {
                        tudMitKijatszani = true;
                        kijatszhatoKartyakIndexe[index] = counter;
                        index++;
                    }
                    counter++;
                }
            }

            Trace.WriteLine("Mi történik?");
            // Ha nem tud mit kijátszani húzni egyet..
            if (tudMitKijatszani == false)
            {
                Trace.WriteLine("{0} nem tud mit kijátszani.. Kér egy lapot!", AI.Nev);
                return null;
            }
            else
            {
                Trace.WriteLine("{0} kártyát hív!", AI.Nev);
                AI.FokuszaltKartyaCsereAdottIndexre(kijatszhatoKartyakIndexe[0]);
                // Dupla csekkolás van... elvileg itt már olyan indexű van kiválasztva ami színre passszol, az érték vizsgálat opcionális
                // Ha nem a felhasználó a kihívó, akkor az AI rak először, nem kell foglalkoznia a nagysággal-> tisztaság kedvéért új változó
                bool ellenorizzeHogyNagyobbe = !felhasznaloAKihivo; 
                return AI.FokuszaltKartyatKijatszik(Adu, ellenorizzeHogyNagyobbe);
            }
        }


    }
}
