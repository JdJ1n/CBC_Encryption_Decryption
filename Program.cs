//PIF1006-TP3-Program.cs
//MEMBRES DE L'ÉQUIPE NO 3
//Jiadong Jin JINJ86100000
using PIF1006_tp3;
using System;
using System.IO;
using System.Linq;

string? msg;
string? cle;

bool menu_continue = true;
while (menu_continue)
{
    //kqj
    //k\u0002q\u0010\u0017\u001dj\u0019
    Console.WriteLine("\r\nMenu");
    Console.WriteLine("Veuillez entrer un numéro pour sélectionner la fonction que vous voulez utiliser:");
    Console.WriteLine("(1) Cryptage");
    Console.WriteLine("(2) Décryptage");
    Console.WriteLine("(3) Quitter l'application.");
    switch (Console.ReadKey().KeyChar)
    {
        case '1':
            Console.WriteLine("\r\nVeuillez entrer le message original à crypter:");
            msg = Console.ReadLine();
            Console.WriteLine("Veuillez entrer la clé:");
            cle = Console.ReadLine();
            if (msg is not null && cle is not null) {
                Console.WriteLine(Chiffrement.Chiffrer(msg, cle));
            }
            break;
        case '2':
            Console.WriteLine("\r\nVeuillez entrer le message original à décrypter:");
            msg = Console.ReadLine();
            Console.WriteLine("Veuillez entrer la clé:");
            cle = Console.ReadLine();
            if (msg is not null && cle is not null)
            {
                Console.WriteLine(Chiffrement.Dechiffrer(msg, cle));
            }
            break;
        case '3':
            menu_continue = false;
            break;
        default:
            Console.WriteLine("\r\n" + "Caractère non valide.\r\n");
            break;
    }

}
