using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{
    class Visszajatszo
    {


        public static void VisszaJatszikFajlt()
        {
            Console.WriteLine("Ad meg a visszajátszandó fájl nevét (elérési úttal ha nem a bináris mellet van!");
            String fileName = Console.ReadLine();

            List<String> esemenyek = new List<string>();
            try
            {
                var lines = File.ReadAllLines(fileName);
                Console.WriteLine("Gomb lenyomásával tudod léptetni az eseményeket előre!");

                //Üres sorokat nem mentünk ki..
                foreach (var item in lines)
                {
                    if (item != "" && item != "\n")
                    {
                        esemenyek.Add(item);
                    }
                }
                // Soronként visszaadjuk a felhasználónak a fájl tartalmát visszajátszásként
                foreach (var line in esemenyek)
                {
                    Console.WriteLine(line);
                    Console.ReadKey(true);
                }

                Megjelenito.Fontos("Az események végére értél!");
            }
            catch (Exception)
            {
                Console.WriteLine("A megadott fájl nem található, vagy nincs hozzáférésed! Megpróbálod újra beírni?");
                bool megprobalja = UserInteraction.Eldontes(ConsoleKey.Y, ConsoleKey.N);
                if (megprobalja)
                    VisszaJatszikFajlt();
            }

        }

    }
}
