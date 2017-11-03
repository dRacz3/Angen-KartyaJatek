using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{

    // Játékos osztály a fő propertykkel, és alapvető kártya kezelési funkciókkal
    class Jatekos
    {
        public List<Kartya> Kartyak { get; }
        int FokuszaltKartyaIndex = 0;
        public String Nev { get; }

        public Jatekos(String nev)
        {
            Kartyak = new List<Kartya>();
            Nev = nev;
        }


        // Hozzáad egy kártyát a játékos kezében lévőkhöz
        public void KartyatKap(Kartya k)
        {
            Console.Write("\n {0}: Új kártya került a kezébe!", Nev);
            Kartyak.Add(k);
            Console.Write(", {0}-nál lévő kártyák száma:{1} \n", Nev, Kartyak.Count);
        }


        // Kiírja konzolra a játékosnál lévő kártyákat
        public void KartyakatMutat()
        {
            Console.WriteLine("\n{0}-nál most a következő kártyák vannak:", Nev);
            Megjelenito.PrintKartyak(Kartyak);
            Console.Write("\nA fókuszált kártya pedig a : \t ");
            if(Kartyak.Count != 0)
            {
                Megjelenito.PrintKartya(Kartyak[FokuszaltKartyaIndex]);
            }
            else
            {
                JatekMenet.ElfogytakAKartyakEgyikJatekosnak = true;
            }
        }


        // A fókuszált kártyát cseréli a beadott paraméter előjelétől függő irányban
        public void FokuszaltKartyaSwitch(int sign)
        {
            if (sign > 0)
            {
                FokuszaltKartyaIndex++;
            }
            else
            {
                FokuszaltKartyaIndex--;
            }
            FokuszaltKartyaIndex %= Kartyak.Count;
            // Átestünk negatív index tartományba.. Azaz a másik oldalon vagyok!
            if (FokuszaltKartyaIndex == -1)
            {
                FokuszaltKartyaIndex = Kartyak.Count - 1;
            }
        }


        // Fókuszált kártyát adott indexűre cseréli a játékos kezében
        public void FokuszaltKartyaCsereAdottIndexre(int index)
        {
            FokuszaltKartyaIndex = index;
        }

        public Kartya FokuszaltKartyatKijatszik(Kartya adu, bool ellenorizzeHogyNagyobbE)
        {
            Kartya kartya = Kartyak[FokuszaltKartyaIndex];
            if (!ellenorizzeHogyNagyobbE)
            {
                if (JatekLogika.ErvenyesLepesE(adu, kartya) && JatekLogika.MagasabbE(adu, kartya))
                {
                    Kartyak.RemoveAt(FokuszaltKartyaIndex);
                    FokuszaltKartyaIndex = 0;
                    return kartya;
                }
            }
            else
            {
                if (JatekLogika.ErvenyesLepesE(adu, kartya))
                {
                    Kartyak.RemoveAt(FokuszaltKartyaIndex);
                    FokuszaltKartyaIndex = 0;
                    return kartya;
                }
            }
            return null;
        }

        // Eldobja a fókuszált kártyát a kezéből
        public void FokuszaltKartyatEldob()
        {
            Console.Write("\nA következő kártya eldobásra került: ");
            Megjelenito.PrintKartya(Kartyak[FokuszaltKartyaIndex]);
            Kartyak.RemoveAt(FokuszaltKartyaIndex);
        }

        // A játékos megnézi van-e adott színű kártyája
        public bool VanAdottSzinuKartyaja(Szin szin)
        {
            foreach (Kartya k in Kartyak)
            {
                if (k.Szin == szin)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
