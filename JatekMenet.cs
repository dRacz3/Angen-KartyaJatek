using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Angen
{


    class JatekMenet
    {
        Oszto Oszto;
        Jatekos Felhasznalo;
        Jatekos AI;
        Kartya Adu;
        static Random rnd = new Random();
        Dictionary<String, int> eredmeny = new Dictionary<String, int>();
        Dictionary<String, int> adottKorbenElvittUtesek = new Dictionary<string, int>();


        public bool JatekVegetErt { get; set; }
        public static bool ElfogytakAKartyakEgyikJatekosnak { get; set; }

        int bank = 0;

        #region INICIALIZALAS

        // Konstruktor a játékos nevével, a játék ezután azt jeleníti meg.
        public JatekMenet(String nev)
        {
            Oszto = new Oszto();
            Felhasznalo = new Jatekos(nev);
            AI = new Jatekos("AI");
            eredmeny.Add(Felhasznalo.Nev, 0);
            eredmeny.Add(AI.Nev, 0);
            ElfogytakAKartyakEgyikJatekosnak = false;
            JatekVegetErt = false;
        }


        // Újra inicializálja a kört, új kártyák stb.
        public void Kezdes()
        {
            Oszto = new Oszto();
            Felhasznalo = new Jatekos(Felhasznalo.Nev);
            AI = new Jatekos("AI");


            VegeVanMar(); // Győzelem feltétele 4 kör győzelem

            if (!JatekVegetErt)
            {
                ElfogytakAKartyakEgyikJatekosnak = false;
                adottKorbenElvittUtesek.Clear();
                adottKorbenElvittUtesek.Add(Felhasznalo.Nev, 0);
                adottKorbenElvittUtesek.Add(AI.Nev, 0);

                Megjelenito.Fontos("Új játék kedődött! Az osztó kiosztja a kártyákat..");
                for (int i = 0; i < 4; i++)
                {
                    Felhasznalo.KartyatKap(Oszto.OsztEgyKartyat());
                    AI.KartyatKap(Oszto.OsztEgyKartyat());
                }
                Console.WriteLine("\nOsztó végzett a kártyák kiosztásával!");
                Adu = Oszto.OsztEgyKartyat();

                Megjelenito.Takaritas();
                Megjelenito.PrintJatekSzabalyok();
                Megjelenito.Fontos("Kezdéshez nyomj meg egy gombot!");
                //keyinfo = Console.ReadKey(true);
                Console.Clear();
            }
            else
            {
                Megjelenito.Fontos("A Játék véget ért! Valaki elérte a 4 győzelmet!");
            }
        }


        // Ellenőrzi véget ért-e a játék
        private bool VegeVanMar()
        {
            if(eredmeny[Felhasznalo.Nev] == 4)
            {
                return true;
            }
            else if(eredmeny[AI.Nev] ==4)
            {
                return true;
            }

            return false;
        }
        #endregion


        // Új kör esetén sorsolunk ki kezd, majd azután folytatódik tovább a játék
        public bool UjKorKezdese()
        {
            Kezdes();
            int i = rnd.Next(0, 101);

            // Kezdés a felhasználóhoz kerül
            if (i < 50)
            {
                // Megjelenítés
                Megjelenito.Takaritas();
                Megjelenito.HeaderMessage = "Kezdés lehetősége a felhasználónál van...\n";
                Megjelenito.HeaderRajzolas(Adu);
                Felhasznalo.KartyakatMutat();
                bool akarFelvevoLenni = KerdezdMegAkarEJátszaniAJatekos();
                // Ha a felhasználó szeretne felvevő lenni ide lépünk be..
                if (akarFelvevoLenni)
                {
                    // Végig zongorázzuk azt amikor a játékos a felvevő...
                    StateMachineAmikorJatekosAFelvevo();
                }
                // Felhasználóé volt a kezdés lehetősége de nem élt vele..
                else
                {
                    bool egybolAIDonthetett = false;
                    StateMachineAmikorAIDonthet(egybolAIDonthetett);
                }
            }
            else
            {
                bool egybolAIDonthetett = true;
                StateMachineAmikorAIDonthet(egybolAIDonthetett);
            }

            //Megjelenito.ClearEvents();
            Megjelenito.AddEvent("Kör lezárult! Új kör kezdődik!\n");
            Megjelenito.Fontos("A kör lezárult! Következő kör kezdődik... Kezdéshez nyomj meg egy gombot!");
            Console.ReadKey(true);
            return JatekVegetErt;

        }

        #region DÖNTÉSI LOGIKA


        // Ha a felhasználót sorsolta ki a gép hogy kezdjen:
        public void StateMachineAmikorJatekosAFelvevo()
        {
            // Újrarajzolás
            Megjelenito.AddEventEsUjrarajzol("Játékos elvállalta a felvevő szerepét\n", Adu);
            Felhasznalo.KartyakatMutat();
            // AI elvállalja-e a kört?
            bool AI_ElvállaljaE = AILogika.AIKarEJatszani();
            if (AI_ElvállaljaE)
            {
                // Ha AI elvállalta
                Megjelenito.AddEventEsUjrarajzol("AI elvállalta a kört!\n", Adu);
                Felhasznalo.KartyakatMutat();

                // Lapcsere üzemmód..
                int csereSzam = KerdezdMegHanyLapotSzeretneCserélniAFelhasznalo();
                CsereljLapot(csereSzam);
                bool felhasznaloHivE = true;

                // 4 kör van...
                int j = 0;
                while (j < 4 && !ElfogytakAKartyakEgyikJatekosnak &&!JatekVegetErt)
                {
                    Megjelenito.AddEventEsUjrarajzol(String.Format("{0} kihívás! A felhasználó a kihívó-e: {1}\n", j, felhasznaloHivE), Adu);
                    AdutFelütAmigKihivoNemTudRakni(felhasznaloHivE);
                    felhasznaloHivE = KartyakatKijatszak(felhasznaloHivE);
                    Trace.WriteLine(String.Format("Felhasznalo kezdett amúgy.. FelhasználóHívE most? {0}, Kör szám: {1}", Felhasznalo, j));
                    j++;
                }
                // Kiértékelés
                if (adottKorbenElvittUtesek[AI.Nev] < 1)
                {
                    Megjelenito.AddEvent("AI nem teljesítette az adott körben előírt ütés számot! Fizet a bankba!\n");
                    eredmeny[Felhasznalo.Nev]++;
                    bank++;
                }
                if (adottKorbenElvittUtesek[Felhasznalo.Nev] < 2)
                {
                    Megjelenito.AddEvent(String.Format("{0} nem teljesítette az adott körben előírt ütés számot! Fizet a bankba!\n",Felhasznalo.Nev));
                    eredmeny[AI.Nev]++;
                    bank++;
                }

                ElvitteEValakiABankot();

                // Adott kör lezárult, eredményeket kiírjuk
                Console.WriteLine(String.Format("\nA kör lezárult! Elvit ütések: {0}:{1} | {2}:{3}", Felhasznalo.Nev, adottKorbenElvittUtesek[Felhasznalo.Nev], AI.Nev, adottKorbenElvittUtesek[AI.Nev]));
                Megjelenito.Fontos(String.Format("A kör lezárult! Jelenlegi eredmény: {0}:{1} | {2}:{3}", Felhasznalo.Nev, eredmeny[Felhasznalo.Nev], AI.Nev, eredmeny[AI.Nev]));
            }
            else
            {
                Megjelenito.AddEventEsUjrarajzol("Felhasználó jelentkezett felvevőnek, AI bedobta a lapokat!\n", Adu);
            }
        }


        // Ha AI dönhet arról, hogy felvevő akar-e lenni akkor ez a sorozat játszódik le
        public void StateMachineAmikorAIDonthet(bool elsoAkiDonthet)
        {
            if (!elsoAkiDonthet)
            {
                Megjelenito.AddEventEsUjrarajzol("Játékos nem vállalta el a kört.\n", Adu);
            }
            int esely = rnd.Next(0, 101);

            bool AI_szeretneFelvevoLenni = esely < 50;
            // Ha AI szeretné elvállalni a Felvevő szerepét:
            if (AI_szeretneFelvevoLenni)
            {
                Megjelenito.AddEventEsUjrarajzol("AI szeretne felvevő lenni...\n", Adu);

                bool felhasznaloHivE = false;
                int j = 0;
                while (j < 4 && !ElfogytakAKartyakEgyikJatekosnak && !JatekVegetErt)
                { 
                    Megjelenito.AddEventEsUjrarajzol(String.Format("{0}. kihívás! A felhasználó a kihívó-e: {1}\n", j + 1, felhasznaloHivE), Adu);
                    AdutFelütAmigKihivoNemTudRakni(felhasznaloHivE);
                    Trace.WriteLine(String.Format("AI kezdett amúgy.. FelhasználóHívE most? {0}, Kör szám: {1}", Felhasznalo, j));
                    felhasznaloHivE = KartyakatKijatszak(felhasznaloHivE);
                    j++;
                }

                // Kiértékelés
                if (adottKorbenElvittUtesek[AI.Nev] < 2)
                {
                    eredmeny[Felhasznalo.Nev]++;
                    bank++;
                    Megjelenito.AddEvent("AI nem teljesítette az adott körben előírt ütés számot! Fizet a bankba!\n");
                }
                if (adottKorbenElvittUtesek[Felhasznalo.Nev] < 1)
                {
                    eredmeny[AI.Nev]++;
                    bank++;
                    Megjelenito.AddEvent(String.Format("{0} nem teljesítette az adott körben előírt ütés számot! Fizet a bankba!\n", Felhasznalo.Nev));
                }

                ElvitteEValakiABankot();

                // Adott kör lezárult, eredményeket kiírjuk
                Megjelenito.Fontos(String.Format("A kör lezárult! Jelenlegi eredmény: {0}:{1} | {2}:{3}", Felhasznalo.Nev, eredmeny[Felhasznalo.Nev], AI.Nev, eredmeny[AI.Nev]));
            }
            else
            {
                Megjelenito.AddEventEsUjrarajzol("AI nem szeretne felvevő lenni!\n", Adu);
            }
        }

        // Megnézi valaki elért-e egy körben 4 ütést, az viheti a bankot!
        private void ElvitteEValakiABankot()
        {
            if (adottKorbenElvittUtesek[AI.Nev] == 4)
            {
                Megjelenito.AddEvent("AI elvitte a bankot! A bank értéke: " + bank + "\n");
                bank = 0;
            }
            else if (adottKorbenElvittUtesek[Felhasznalo.Nev] == 4)
            {
                Megjelenito.AddEvent(String.Format("{0} elvitte a bankot! A bank értéke: {1}\n", Felhasznalo.Nev, bank));
                bank = 0;
            }
        }


        // Elágazás kiszervezése copy paste elkerülésére
        private void AdutFelütAmigKihivoNemTudRakni(bool felhasznaloHivE)
        {
            if (felhasznaloHivE)
            {
                AdutKerAmigNincsMegfeleloSzineAHivonak(Felhasznalo);
            }
            else
            {
                AdutKerAmigNincsMegfeleloSzineAHivonak(AI);
            }
        }


        // Copy paste elkerülésére közös rész. Ha nem a felhasználó hív ->AI kezd, és fordítva
        public bool KartyakatKijatszak(bool felhasznaloHiv)
        {
            if (felhasznaloHiv)
            {
                bool felhasznaloNemPasszolt = JatszKiKartyat(Felhasznalo, felhasznaloHiv);
                bool tudottUtni = AIJatszKiKartyat(felhasznaloHiv);
                // Ha user nem passzol -> AI ütött => AI pont
                // Ha user passzol -> AI ütött => AI pont
                // Ha user nem paszol -> AI ütött => User pont
                // Ha user passzol -> AI nem ütött - senki nem kap
                if (felhasznaloNemPasszolt && tudottUtni)
                {
                    adottKorbenElvittUtesek[AI.Nev]++;
                    Megjelenito.AddEvent("Felhasználó tudott rakni, AI ütött! AI kap pontot!\n");
                    return false;
                }
                else if (!felhasznaloNemPasszolt && tudottUtni)
                {
                    Megjelenito.AddEvent("Felhasználó nem tudott hívni, AI ütött! AI kap pontot!\n");
                    adottKorbenElvittUtesek[AI.Nev]++;
                    return false;
                }
                else if (felhasznaloNemPasszolt && !tudottUtni)
                {

                    Megjelenito.AddEvent("Felhasználó tudott rakni, AI nem ütött! Felhasználó kap pontot!\n");
                    adottKorbenElvittUtesek[Felhasznalo.Nev]++;
                    return true;
                }
                else
                {
                    Megjelenito.AddEvent("Senki nem kap pontot ütésért.\n");
                    return false;
                    // nincs pont..
                }
            }
            else
            {
                bool tudottMitKijatszani = AIJatszKiKartyat(felhasznaloHiv);
                bool nemPasszolt = JatszKiKartyat(Felhasznalo, felhasznaloHiv);

                // Ha AI tudott mit kijátszani -> Felhasznalo nem passzolt (ütött) => Felhasználó kap pontot
                // Ha AI tudott mit kijátszani -> Felhasználo passzolt => AI kap pontot
                // Ha AI nem tudott mit kijátszani ->Felhasználi nem passzolt (ütött) => Felhasználó kap pontot
                // Amúgymeg senki
                if (tudottMitKijatszani && nemPasszolt)
                {
                    Megjelenito.AddEvent("AI tudott rakni, Felhasználó ütött! Felhasználó kap pontot!\n");
                    adottKorbenElvittUtesek[Felhasznalo.Nev]++;
                    return true;
                }
                else if (tudottMitKijatszani && !nemPasszolt)
                {
                    Megjelenito.AddEvent("AI tudott rakni, Felhasználó nem ütött! AI kap pontot!\n");
                    adottKorbenElvittUtesek[AI.Nev]++;
                    return false;
                }
                else if (!tudottMitKijatszani && nemPasszolt)
                {
                    Megjelenito.AddEvent("AI nem tudott rakni, Felhasználó ütött! Felhasználó kap pontot!\n");
                    adottKorbenElvittUtesek[Felhasznalo.Nev]++;
                    return true;
                }
                else
                {
                    Megjelenito.AddEvent("Senki nem kap pontot ütésért.\n");
                    return true;
                    // nincs pont..
                }
            }
        }


        #endregion


        #region FELHASZNÁLÓI INTERAKCIÓ

        // Wrapper az interakcio köré
        private bool KerdezdMegAkarEJátszaniAJatekos()
        {
            Console.WriteLine("\nGratulálok, Te kezdesz! Szeretnél felvevő lenni?");
            return UserInteraction.Eldontes(ConsoleKey.Y, ConsoleKey.N);
        }


        // Wrapper az interakcio köré
        private int KerdezdMegHanyLapotSzeretneCserélniAFelhasznalo()
        {
            Console.WriteLine("\nAdd meg hány kártyát szeretnél cserélni!");
            int[] validErtekek = { 0, 1, 2 };
            return UserInteraction.SzamotBeker(validErtekek);
        }


        // Lapcserét megvalósító algoritmus, lapozza a fokuszált lapot, és ha a user enter nyom lecseréli azt az osztótól kapottra!
        private void CsereljLapot(int cserelendoLapSzam)
        {
            if (cserelendoLapSzam != 0)
            {
                Kartya ujKartya = Oszto.OsztEgyKartyat();

                String baseState = "Csere szakasz!Választ ki fókuszba a kártyát amit le szeretnél cserélni, majd enterrel cserélj!";
                Megjelenito.HeaderMessage = baseState;
                Felhasznalo.KartyatKap(ujKartya);
                Megjelenito.AddEvent("Új kártya került felhúzásra " + ujKartya.KartyaNeve() + "\n");

                int cserekSzama = 1; // Már egy lapot felvettünk
                while (cserekSzama != cserelendoLapSzam)
                {
                    Megjelenito.Takaritas();
                    Megjelenito.HeaderRajzolas(Adu);
                    Console.WriteLine("Hátra lévő cserék száma: {0}", (cserelendoLapSzam - cserekSzama));
                    Felhasznalo.KartyakatMutat();
                    ConsoleKeyInfo keyinfo;

                    keyinfo = Console.ReadKey(true);
                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            Felhasznalo.FokuszaltKartyaSwitch(-1);
                            break;
                        case ConsoleKey.RightArrow:
                            Felhasznalo.FokuszaltKartyaSwitch(1);
                            break;
                        case ConsoleKey.Enter:
                            Felhasznalo.FokuszaltKartyatEldob();
                            Console.WriteLine();
                            ujKartya = Oszto.OsztEgyKartyat();
                            Felhasznalo.KartyatKap(ujKartya);
                            Megjelenito.AddEvent("Új kártya került felhúzásra " + ujKartya.KartyaNeve() + "\n");

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Nyomj meg egy gombot a folytatáshoz..");
                            Console.ForegroundColor = ConsoleColor.White;
                            //Console.ReadKey(true);
                            cserekSzama++;
                            break;
                        default:
                            // Do nothing...
                            break;
                    }
                }
                Felhasznalo.FokuszaltKartyatEldob();
            }
        }


        // Játékos kártya kijátszása. figyeli a gomblenyomásokat és hogy valid-e a lépés
        private bool JatszKiKartyat(Jatekos jatekos, bool felhasznaloAKihivo)
        {
            String baseState = "Kártya kijátszása következik! Navigáció : <- / -> nyilakkal, Enter : kártya bedobása, Space : Passzolás, Escape - Feladás";
            Megjelenito.HeaderMessage = baseState;
            Megjelenito.Takaritas();
            Megjelenito.HeaderRajzolas(Adu);
            ConsoleKeyInfo keyinfo;
            bool validLepesTortent = false;
            while (!validLepesTortent)
            {
                Megjelenito.Takaritas();
                Megjelenito.HeaderRajzolas(Adu);
                Felhasznalo.KartyakatMutat();
                keyinfo = Console.ReadKey(true);
                switch (keyinfo.Key)
                {
                    case ConsoleKey.Enter:
                        Kartya tmp = jatekos.FokuszaltKartyatKijatszik(Adu, felhasznaloAKihivo);
                        if (tmp != null)
                        {
                            Adu = tmp;
                            String kartyaKijatszosEvent = String.Format("{0} kijátszotta : {1} kártyát\n", jatekos.Nev, tmp.KartyaNeve());
                            Megjelenito.AddEvent(kartyaKijatszosEvent);
                            validLepesTortent = true;
                        }
                        else
                        {
                            String ervenytelenLepesString = String.Format("Érvénytelen lépés! Ez a kártya nem játszható ki..\n");
                            Megjelenito.AddEvent(ervenytelenLepesString);
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        validLepesTortent = true;
                        return false;
                    case ConsoleKey.LeftArrow:
                        Felhasznalo.FokuszaltKartyaSwitch(-1);
                        break;
                    case ConsoleKey.RightArrow:
                        Felhasznalo.FokuszaltKartyaSwitch(1);
                        break;
                    case ConsoleKey.Escape:
                        Megjelenito.Fontos("Játékos feladta a játékot!");
                        Megjelenito.AddEvent("Játékos feladta a játékot!\n");
                        JatekVegetErt = true;
                        return true;
                    default:
                        break;
                }
            }
            return true;

        }

        #endregion

        #region AI_INTERAKCIO


        // Wrapper metódus -> figyeli tud-e kijátszani az AI kártyát
        private bool AIJatszKiKartyat(bool felhasznaloAkihivo)
        {
            Kartya tmp = AILogika.KartyatKijatszik(Adu, AI, felhasznaloAkihivo);
            if (tmp != null)
            {
                Adu = tmp;
                String kartyaKijatszosEvent = String.Format("{0} kijátszotta : {1} kártyát\n", AI.Nev, tmp.KartyaNeve());
                Megjelenito.AddEvent(kartyaKijatszosEvent);
                Megjelenito.Takaritas();
                Megjelenito.HeaderRajzolas(Adu);
                AI.KartyakatMutat();
                return true;
            }
            else
            {
                String ervenytelenLepesString = String.Format("{0} Nem tud mit kijátszani..\n", AI.Nev);
                Megjelenito.AddEvent(ervenytelenLepesString);
                Megjelenito.Takaritas();
                Megjelenito.HeaderRajzolas(Adu);
                AI.KartyakatMutat();
                return false;
            }

        }


        //Egészen addig amig a kihivó játékos kezében nincs legalább 1 szín ami megegyezik az aduval, addig az osztó új kártyát oszt.
        private void AdutKerAmigNincsMegfeleloSzineAHivonak(Jatekos kihivo)
        {
            while (true)
            {
                if (kihivo.VanAdottSzinuKartyaja(Adu.Szin))
                {
                    Megjelenito.AddEventEsUjrarajzol(String.Format("{0} a kihívó, és most már elvileg van aduval megegyező szín a kezében!\n", kihivo.Nev), Adu);
                    return;
                }
                else
                {
                    Adu = Oszto.OsztEgyKartyat();
                    String ujAduLettKerve = String.Format("{0} a kihívó, nincs adott szín! Új adu :{1} \n", kihivo.Nev, Adu.KartyaNeve());
                    Megjelenito.AddEvent(ujAduLettKerve);
                }
            }
        }

        #endregion

    }
}
