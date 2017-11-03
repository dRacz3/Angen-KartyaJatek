using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{

    // Utility osztály a konzolos megjelenítés uniformizálására
    class Megjelenito
    {
        // Headerben állandóan megjelenő üzenet
        static public String HeaderMessage {get;set;}
        // Események listája, max 10 darab lehet jelen. Ezeket a header alá rajzolja.
        static private Queue<String> EventQueue { get; set; }


        // Event hozzáadása a listához. Ha túlcsordulna, a legrégebbit kidobja
        static public void AddEvent(string s)
        {
            if(EventQueue == null) // Lusta inicializalas
            {
                EventQueue = new Queue<string>();
            }
            EventQueue.Enqueue(s);
            if (EventQueue.Count == 10)
            {
                EventQueue.Dequeue();
            }
            
        }

        /// <summary>
        /// Hozzáad új eventet és újrarajzolja a konzolt
        /// </summary>
        /// <param name="s"></param>
        /// <param name="Adu"></param>
        static public void AddEventEsUjrarajzol(string s, Kartya Adu)
        {
            Megjelenito.Takaritas();
            AddEvent(s);
            Megjelenito.HeaderRajzolas(Adu);
        }



        // Kártyát kiírja a megfelelő színnel új sorba a konzolra.
        public static void PrintKartya(Kartya kartya)
        {
            ConsoleSzinBeallit(kartya);
            Console.Write(kartya.KartyaNeve());
            Console.ForegroundColor = ConsoleColor.White;
        }

   
        // Kiírja konzolra az IEnumerable<Kartya> interface elemeit szinezve, tagolva. Soronként max 4 Kártyát.
        public static void PrintKartyak(IEnumerable<Kartya> kartyak)
        {
            int count = 0;
            foreach (Kartya k in kartyak)
            {
                ConsoleSzinBeallit(k);
                Console.Write(k.KartyaNeve() + "\t\t");
                Console.ForegroundColor = ConsoleColor.White;
                count++;
                if(count == 4)
                {
                    Console.Write("\n");
                    count = 0;
                }
            }
        }

        // Beállítja a konzol színét a kártyának megfelelőre
        // A kártya kiírása előtt hívandó. Utána vissza kell állítani a 
        // Konzol színét sajnos.
        public static void ConsoleSzinBeallit(Kartya kartya)
        {
            switch (kartya.Szin)
            {
                case Szin.Piros:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Szin.Zold:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Szin.Tok:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Szin.Makk:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        // Formázott szöveg kiiratás fontosabb üzenetek megjelenítésére amit a felhasználónak fontos, hogy észrevegyen!
        public static void Fontos(String szoveg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("-------------------------------");
            Console.WriteLine(szoveg);
            Console.WriteLine("-------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }

        // A billentyűk funkcióit jeleníti meg
        public static void PrintHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("*************************************************************** \n");
            sb.Append("\tAz elérhető parancsok a következőek:\n ");
            sb.Append("\th - Súgó mutatása \t");
            sb.Append("\ts - Kártyák megmutatása\t");
            sb.Append("\n\t<- / -> - Fókuszált kártya cseréje\n");
            sb.Append("\tx - Feladás/Kilépés \t");
            sb.Append("\n");
            sb.Append("*************************************************************");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(sb.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
        

        public static void Takaritas()
        {
            Console.Clear();
        }


        public static void HeaderRajzolas(Kartya adu)
        {
            try
            {
                Console.SetCursorPosition(0,0);
                Console.WriteLine(HeaderMessage);
                PrintEventHistory();
                Console.WriteLine("-----------------------------------");
                Console.Write("Jelenlegi adu:\t");
                Megjelenito.PrintKartya(adu);
                Console.Write("\n\n");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }


        private static void PrintEventHistory()
        {
            if(EventQueue == null)
            {
                EventQueue = new Queue<string>();
            }
            List<String> list = new List<string>(EventQueue);
            Console.WriteLine("Utolsó néhány esemény:");
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write("\t->{0}", list[i]);
            }
        }


        public static void PrintJatekSzabalyok()
        {
            Megjelenito.Fontos("Angén kártyajáték, készítette: Rácz Dániel, Bl02DQ");
            StringBuilder sb = new StringBuilder();
            sb.Append("Kártya: magyar, 32 lap \n    Játék típusa: ütésszerző \n    Játékosok: 2");
            sb.Append("A játék célja: \nA felvevőnek 2, ellenfelének 1 ütést szerezni a lehetséges 4 - ből.");
            sb.Append("A kártyák rangsora hagyományos\nLegerősebb az ász, majd király, felső, alsó, X, IX, VIII, VII.\n");
            sb.Append("Haladási irány: jobbra tartással.Az osztó mindkét félnek 4 - 4 lapot ad.\n");
            sb.Append("A 17.- et aduként üti fel és a maradék kártyákat erre helyezi húzható talonként.\n");
            sb.Append("Felvevőnek jelentkezhet az a személy, aki megítélése szerint a lehetséges négy ütésből legalább kettőt el tud vinni. \n");
            sb.Append("Ha valaki vállalkozik erre, ellenfele jelezheti részvételi szándékát, de el is dobhatja kártyái. \n");
            sb.Append("A felvevő két lapot cserélhet: húz egyet a talonból és lerak egy kártyát, majd újra húz és lerak egyet. \n");
            sb.Append("Lapcsere után a felvevő hív ki az első ütéshez.\n");
            sb.Append("A színre szín adása kötelező, hívott szín hiányában adut kell tenni.A kihívás joga az ütést megszerző játékosé.\n");
            sb.Append("Az elszámolásnál az a játékos, aki az előírt ütésszámot nem teljesítette, előre meghatározott összeget fizet a bankba.\n");
            sb.Append("A bankot az egy leosztásban négy ütést nyerő játékos viheti el");

            Console.WriteLine(sb.ToString());
        }

    }
}
