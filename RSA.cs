using System;
using System.IO;
using System.Numerics;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            // = 21 646 157 677 * 68 738 894 069 977 (factorisé en 37 min)
            // 1 487 932 939 581 322 413 763 429
            BigInteger P = 21646157677;
            BigInteger Q = 68738894069977;
            BigInteger N = P * Q;
            // == 1 487 932 939 581 322 413 763 429
            // == 0 734 726 920 089 060 482 937 205
            // ==   153 208 669 861 212 813 680 928
            string fechier = "653932310995966683273685";
            BigInteger E = 157;
            //BigInteger E = BigInteger.Parse(fechier);
            BigInteger D = BigInteger.Parse(fechier);
            BigInteger PhiN = (P - 1) * (Q - 1);

            string adresseLecture = "C:\\Users\\Thibault-MSI\\Desktop\\test.txt";
            string adresseEcriture = "C:\\Users\\Thibault-MSI\\Desktop\\test2.txt";
            string adresseJeSaisPas = "C:\\Users\\Thibault-MSI\\Desktop\\test3.txt";
            string adresseNumero4 = "C:\\Users\\Thibault-MSI\\Desktop\\test4.txt";
            string texte = "";
            
            //BigInteger P = 23;
            //BigInteger Q = 17;
            //BigInteger N = P * Q;
            //BigInteger PhiN = (P - 1) * (Q - 1);
            //BigInteger E = 7;
            //BigInteger D = 151;
/*
            BigInteger test = 80;
            BigInteger[] truc = new BigInteger[3];

            Console.WriteLine(P);
            Console.WriteLine(Q);
            Console.WriteLine(N);
            Console.WriteLine(E);
            Console.WriteLine(PhiN);

            DateTime starttime = DateTime.Now;

            truc = Bezout(E, PhiN);
            
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("R : " + truc[0]);
            Console.WriteLine("U : " + truc[1]);
            Console.WriteLine("V : " + truc[2]);
            Console.WriteLine("D : " + D);

            Console.WriteLine();

            Console.WriteLine(test);

            test = modpow(test, E, N);

            Console.WriteLine(test);

            test = modpow(test, D, N);

            Console.WriteLine(test);

            Console.WriteLine();
            
            */

            Console.WriteLine(chiffrage(adresseLecture, adresseEcriture, E, N));

            //12:08 (Ptite note perso)

            Console.WriteLine(dechiffrage(adresseEcriture, adresseJeSaisPas, D, N));
            
            TimeSpan tempsecoule = DateTime.Now.Subtract(starttime);
            Console.WriteLine("\n" + tempsecoule);
        }

        static public string chiffrage(string fluxEntrant, string fluxSortant, BigInteger E, BigInteger N)
        {
            string texte = "";
            try
            {
                BigInteger nombre = 0;

                using (StreamReader sr = new StreamReader(fluxEntrant))
                {
                    while (sr.Peek() >= 0)
                    {
                        BigInteger i = 0;
                        nombre = sr.Read();

                        nombre = modpow(nombre, E, N);

// Permet de calibrer le bloc aux bonnes dimensions, si le nombre est plus court que N, ajoute des '0' devant le nombre avant
// de l'envoyer

                        while (nombre.ToString().Length + i < N.ToString().Length)
                        {
                            texte += "0";
                            i++;
                        }

                        i = 0;

                        texte += nombre.ToString();
                        
                    }
                }

                if (File.Exists(fluxSortant))
                {
                    File.Delete(fluxSortant);
                }
                File.AppendAllText(fluxSortant, texte);

                return "Chiffrage effectué avec succès !";
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return "Echec du chiffrage";
            }
        }

        static public string dechiffrage(string fluxEntrant, string fluxSortant, BigInteger D, BigInteger N)
        {
            try
            {
                string texte = "";
                string bufferTexte = "";
                char buffer = ' ';
                BigInteger nombre = 0;
                BigInteger i = 0;
                BigInteger j = 0;

                using (StreamReader sr = new StreamReader(fluxEntrant))
                {
                    while (sr.Peek() >= 0)
                    {
                        buffer = (char)sr.Read();
                        if ((Char.IsNumber(buffer)) && i < N.ToString().Length)
                        {
                            bufferTexte += buffer;
                            i++;
                        }
                        
                        //On récupère un bloc de la taille de N, et on le traite
                        if (i == N.ToString().Length)
                        {
                            nombre = BigInteger.Parse(bufferTexte);
                            bufferTexte = "";
                            i = 0;

                            nombre = modpow(nombre, D, N);

                            buffer = (char)nombre;

                            texte += buffer;
                        }
                    }

                    if (File.Exists(fluxSortant))
                    {
                        File.Delete(fluxSortant);
                    }
                    File.AppendAllText(fluxSortant, texte, UnicodeEncoding.Unicode);

                    return "Dechiffrage effectué avec succès!";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur {0}", e.ToString());
                return "Echec du dechiffrage.";
            }
        }        

// Obvious fonction, permet de trouver un PGCD entre deux nombres
// Permet de trouver E en mettant nombre1 = PhiN et nombre2 = i++
// Permet de trouver un grand nombre de E potentiel
        static public BigInteger PGCD(BigInteger nombre1, BigInteger nombre2)
        {
            if(nombre1 > nombre2)
            {
                if (nombre1 % nombre2 == 0)
                    return nombre2;
                else
                    return PGCD(nombre2, (nombre1 % nombre2));
            }
            else
            {
                if (nombre2 % nombre1 == 0)
                    return nombre1;
                else
                    return PGCD(nombre1, (nombre2 % nombre1));
            }
        }


//Fonction obsolète qui me permettait de trouver des valeurs de D pour de petits nombres
// Attention, si utilisée pour de grandes valeurs de PhiN, le programme tourneras en boucle pour rien, utiliser BigInteger bezout(E,PhiN)
// à la place
        public static BigInteger trouveD(BigInteger nombreE, BigInteger nombrePhiN)
        {
            BigInteger E = nombreE;
            BigInteger PhiN = nombrePhiN;

            for(BigInteger i = 0; i < PhiN; i++)
            {
                if ((i * E % PhiN) == 1)
                    return i;
            }

            return 0;
        }

//Permet de trouver D en fonction de A = E , et B = PhiN
//La valeur de D étant égale à celle de U en sortie de programme
        static public BigInteger[] Bezout(BigInteger A, BigInteger B)
        {
            BigInteger R, U, V, r, u, v, q, rs, us, vs;
            BigInteger[] table = new BigInteger[3];

            R = A;
            r = B;
            U = 1;
            V = 0;
            u = 0;
            v = 1;
            
            while (r != 0)
            {
                q = R / r;
                rs = R;
                us = U;
                vs = V;
                R = r;
                U = u;
                V = v;
                r = rs - q * r;
                u = us - q * u;
                v = vs - q * v;
            }

            if (U < 0)
            {
                while (U < 0)
                {
                    U = U + A;
                }
            }

            if (V < 0)
            {
                while (V < 0)
                {
                    V = V + B;
                }
            }
            table[0] = R;
            table[1] = U;
            table[2] = V;

            return table;
        }


//Permet de calculer (baseNum^exp) % m, en utilisant le décalage d'un bit de 'exp' vers la droite à chaque tour de boucle
        static public BigInteger modpow(BigInteger baseNum, BigInteger exp, BigInteger m)
        {
            BigInteger result = 1;

            while (exp > 0)
            {
                if (exp % 2 > 0)
                    result = (result * baseNum) % m;

                exp >>= 1;
                baseNum = (baseNum * baseNum) % m;
            }
            return result;
        }
    }
}
