using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angen
{
    /*
    Kártya: magyar, 32 lap
    Játék típusa: ütésszerző
    Játékosok: 2

    A játék célja
    A felvevőnek 2, ellenfelének 1 ütést szerezni a lehetséges 4-ből.

    A kártyák rangsora hagyományos
    Legerősebb az ász, majd király, felső, alsó, X, IX, VIII, VII.

    A játék menete
X    Haladási irány: jobbra tartással.Az osztó mindkét félnek 4-4 lapot ad.
X    A 17.-et aduként üti fel és a maradék kártyákat erre helyezi húzható talonként. 
X    Felvevőnek jelentkezhet az a személy, aki megítélése szerint a lehetséges négy ütésből legalább kettőt el tud vinni.
X    Ha valaki vállalkozik erre, ellenfele jelezheti részvételi szándékát, de el is dobhatja kártyái. 
X    A felvevő két lapot cserélhet: húz egyet a talonból és lerak egy kártyát, majd újra húz és lerak egyet. 
X    Lapcsere után a felvevő hív ki az első ütéshez.
X    A színre szín adása kötelező, hívott szín hiányában adut kell tenni. A kihívás joga az ütést megszerző játékosé.
X    Az elszámolásnál az a játékos, aki az előírt ütésszámot nem teljesítette, előre meghatározott összeget fizet a bankba.
X    A bankot az egy leosztásban négy ütést nyerő játékos viheti el.
    */


    class Program
    {

        static void Main(string[] args)
        {
            Console.Clear();

            // Létrehozunk új játékot..
            Megjelenito.PrintJatekSzabalyok();
            Megjelenito.Fontos("Üdvözöllek, kezdés előtt add meg a neved kérlek!");

            String nev = Console.ReadLine();


            JatekMenet game = new JatekMenet(nev);
            while(!game.JatekVegetErt)
            {
                game.UjKorKezdese();
            }

            Console.ReadLine();
        }
    }
}
