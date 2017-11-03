using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{

    // Statikus osztály funkciók kiemelésére
    class JatekLogika
    {

        public static bool ErvenyesLepesE(Kartya elozo, Kartya kovetkezo)
        {
            return SzinekEgyeznekE(elozo, kovetkezo);// && MagasabbE(elozo, kovetkezo);
        }


        //Szinre szin adas ellenörzese
        public static bool SzinekEgyeznekE(Kartya elozo, Kartya kovetkezo)
        {
            if (elozo.Szin == kovetkezo.Szin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Érték vizsgálat kártyák között
        public static bool MagasabbE(Kartya elozo, Kartya kovetkezo)
        {
            if (elozo.Ertek < kovetkezo.Ertek)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }


    // Felhasnzálói interakció kiszervezve külön osztályba
    class UserInteraction
    {
        public static bool Eldontes(ConsoleKey igen, ConsoleKey nem)
        {
            Console.WriteLine("Y - ha igen, N - ha nem");
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (!(key.Key == igen || key.Key == nem))
            {
                key = Console.ReadKey(true);
            }
            if (key.Key == igen)
            {
                Console.WriteLine("Felhasználó válasza igen");
                return true;
            }
            else if (key.Key == nem)
            {
                Console.WriteLine("Felhasználó válasza nem");
                return false;
            }
            return false;
        }


        public static int SzamotBeker(int[] validSzamok)
        {
            Console.WriteLine("Add meg a kívánt számot, elfogadott értékek:");
            foreach (int szam in validSzamok)
            {
                Console.Write(szam + "\t");
            }
            Console.WriteLine();

            while (true)
            {
                int szam = -1;
                String input = Console.ReadLine();
                bool valobanSzam = int.TryParse(input,out szam);
                if (valobanSzam && validSzamok.Contains(szam))
                {
                    Console.WriteLine("A játékos {0} lapot cserél!", szam);
                    return szam;
                }
                else
                {
                    Console.Write("\nRossz értéket adtál meg mert:");
                    if(!valobanSzam)
                    {
                        Console.WriteLine("A megadott érték nem szám!");
                    }
                    else
                    {
                        Console.WriteLine("A megadott érték nem elfogadott input");
                    }
                    Console.WriteLine("Próbáld újra!" );
                }
            }
        }

    }


}
