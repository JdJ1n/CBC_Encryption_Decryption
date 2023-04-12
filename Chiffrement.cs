//PIF1006-TP3-Chiffrement.cs
//MEMBRES DE L'ÉQUIPE NO 3
//Jiadong Jin JINJ86100000
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006_tp3
{
    public static class Chiffrement
    {
        public static string Chiffrer(string message, string cle) {
            //Vérifier que cette clé est valide
            int[]? test = CleisValid(cle);
            string msg = message;
            if (test == null)
            {
                return "Invalid clé.";
            }
            int[] columns = test;
            //Utiliser tous les caractères du message pour former une structure matricielle et complétez les caractères manquants avec des ' '.
            int rownbs =(int)Math.Ceiling((double)msg.Length / columns.Length);
            List<char[]> strm = new ();
            for (int j = 0; j < rownbs; j++)
            {
                char[] rowbuffer = new char[columns.Length];
                for (int i = 0; i < rowbuffer.Length; i++) {
                
                    if (msg != "")
                    {
                        rowbuffer[i] = msg[0];
                        msg = msg[1..];
                    }
                    else {
                        rowbuffer[i] = ' ';
                    }
                }
                strm.Add(rowbuffer);
            }
            //Utiliser un dictionnaire avec la valeur de la clé comme Key et un tableau de tous les caractères de chaque colonne comme Value.
            Dictionary<int, char[]> transer = new();
            for (int i = 0; i < columns.Length; i++) {
                char[] elebuffer = new char[rownbs];
                for (int j = 0; j < elebuffer.Length; j++) {
                    elebuffer[j] = strm[j][i];
                }
                transer.Add(columns[i], elebuffer);
            }
            //Prendre les valeurs dans l'ordre de Key du dictionnaire
            string transed = "";
            foreach (int k in columns.OrderBy(n=>n)) {
                foreach (char c in transer[k]) {
                    transed += c;
                }
            }
            return CBC(transed.ToCharArray(),Demande());
        }

        public static string Dechiffrer(string message, string cle)
        {
            string toCBC = CBC(message,Demande());
            string msg = toCBC;
            //Vérifier que cette clé est valide
            int[]? test = CleisValid(cle);
            if (test == null)
            {
                return "Invalid clé.";
            }
            int[] clechecked = test;
            int[] columns = new int[clechecked.Length];
            int rownbs = (int)Math.Ceiling((double)msg.Length / columns.Length);
            //Calculer la clé inverse par rapport à la clé originale
            for (int i= 0;i < clechecked.Length;i++) {
                columns[clechecked[i]-1] = Array.IndexOf(clechecked, clechecked[i])+1;
            }
            List<char[]> strm = new();
            //Remplir la matrice avec les informations dans un ordre vertical.
            for (int j = 0; j < columns.Length; j++)
            {
                char[] colbuffer = new char[rownbs];
                for (int i = 0; i < colbuffer.Length; i++)
                {
                    if (msg != "")
                    {
                        colbuffer[i] = msg[0];
                        msg = msg[1..];
                    }
                    else
                    {
                        colbuffer[i] = ' ';
                    }
                }
                strm.Add(colbuffer);
            }
            //Utiliser un dictionnaire avec la valeur de la clé comme Key et un tableau de tous les caractères de chaque colonne comme Value.
            Dictionary<int, char[]> transer = new();
            for (int i = 0; i < columns.Length; i++)
            {
                transer.Add(columns[i], strm[i]);
            }
            //Remplir verticalement la matrice avec le message décrypté.
            char[] transed = new char[columns.Length*rownbs];
            foreach (int k in columns.OrderBy(n => n))
            {
                int index = k-1;
                for (int i= 0;i< transer[k].Length;i++)
                {
                    transed[index+i*columns.Length]=transer[k][i];
                }
            }
            return new string(transed) ;
        }

        //Cryptage par la méthode CBC
        public static string CBC(char[] message, byte vec)
        {
            UTF8Encoding asc = new();
            byte[] ori = asc.GetBytes(message);
            byte[] res = new byte[ori.Length];
            byte vi = vec;
            for (int i = 0; i < res.Length; i++) {
                res[i] =  (byte)(ori[i] ^ vi);
                vi = res[i];
            }
            //Convertir le tableaux des bytes en leurs séquences hexadécimales correspondantes sous forme de chaînes de caractères.
            return ToHexStrFromByte(res);
        }

        ////Décryptage par la méthode CBC
        public static string CBC(string message, byte vec)
        {
            UTF8Encoding asc = new();
            //Convertir un message crypté d'une chaîne hexadécimale en un byte[].
            byte[] ori = ToBytesFromHexString(message);
            byte[] res = new byte[ori.Length];
            byte vi = vec;
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = (byte)(ori[i] ^ vi);
                vi = ori[i];
            }
            string result = "";
            foreach (char c in asc.GetChars(res)) {
                result += c;
            }
            return result;
        }

        public static string ToHexStrFromByte(byte[] byteDatas)
        {
            StringBuilder builder = new();
            for (int i = 0; i < byteDatas.Length; i++)
            {
                builder.Append(string.Format("{0:X2} ", byteDatas[i]));
            }
            return builder.ToString().Trim();
        }

        public static byte[] ToBytesFromHexString(string hexString)
        {
            string[] chars = hexString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] returnBytes = new byte[chars.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(chars[i], 16);
            }
            return returnBytes;
        }

        private static int[]? CleisValid(string cle) {
            string[] check = cle.Trim().Split(" ");
            int[] columns = new int[check.Length];
            try
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    foreach (char c in check[i])
                    {
                        if (!Char.IsDigit(c))
                        {
                            //Si un non-numéro est présent dans la clé, la clé est invalide.
                            return null;
                        }
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (int.Parse(check[i]) == columns[j])
                        {
                            return null;
                        }
                    }
                    columns[i] = int.Parse(check[i]);
                }
            }
            catch (Exception)
            {
                //Si une exception se produit pendant le processus de lecture de la clé, celle-ci n'est pas valide.
                return null;
            }
            if (columns.Max() != columns.Length) {
                //Si la longueur de la clé n'est pas égale à la valeur maximale des chiffres de la clé, la clé n'est pas valide.
                return null;
            }
            return columns;
        }

        //Demander à l'utilisateur d'entrer le vecteur initial
        public static byte Demande()
        {
            Console.WriteLine("Veuillez entrer le vecteur initial:");
            string? input = Console.ReadLine();
            if (input is null||input=="")
            {
                Console.WriteLine("Si aucun vecteur initial n'est saisi, VI=00000000 sera utilisé pour calculer");
                return 0;
            }
            else {
                int entered = Convert.ToInt32(input); ;
                return (byte)entered;
            }
            
        }
    }
}
