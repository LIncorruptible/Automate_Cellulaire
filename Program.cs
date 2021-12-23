using System;
using System.IO;
using System.Threading;

namespace Automate_Cellulaire
{
    class Program
    {
        /*FP0 : Interface_de_Depart()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service permet de guider l'utilisateur dans le lancement du programme.
         *      Il doit indiquer les différentes instructions pour veiller au bon fonctionnement du code.
         * INPUT : -----
         * OUTPUT : -----
         */
        public static void Interface_de_Depart()
        {
            Console.WriteLine("===== Lancement du jeu de la vie =====");
            Console.WriteLine(
                "Pour commencer vous allez devoir renseigner le fichier de départ à executer."
                + "\nSi vous voulez en créer un, il devra être déposé dans le dossier matrice situé à la racine du projet."
                + "\nEt correspondre au critère suivant :"
                + "\nChaque ligne commence et se termine par || ainsi que chaque caractère qui en est séparé."
                + "\nUne cellule vivante a pour symbole X et une morte a O."
                + "\nSi vous ne voulez pas créer de fichier vous pouvez renseignez le nom fichier d'un des exemples fournit à la racine du projet dans le dossier matrices."
            );
        }

        /*FP1 : Lecture_Matricielle()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service doit à partir d'un fichier contenant une "matrice" et des choix utilisateurs,
         *      traduire cette dernière en tableau à 2 dimensions de caractères.
         * INPUT : -----
         * OUTPUT : Matrice2D de caractères
         */
        public static char[,] Lecture_Matricielle()
        {
            string fichier = demandeFichier();

            string[] Lignes = File.ReadAllLines(@"../../../matrices/" + fichier);

            int[] Taille = chercheTailleMatrice(fichier); int nbLigne = Taille[0], nbColon = Taille[1];

            char[,] matriceFichier = new char[nbLigne, nbColon];

            for (int indiceLigne = 0; indiceLigne < nbLigne; indiceLigne++)
            {
                for (int indiceColon = 0; indiceColon < nbColon; indiceColon++)
                {
                    matriceFichier[indiceLigne, indiceColon] = Convert.ToChar(Lignes[indiceLigne].Split('|')[indiceColon]);
                }
            }

            bool choix = Demande_Taille(ref nbLigne, ref nbColon);

            if (choix == true) return Redimensionne_Matrice(matriceFichier, nbLigne, nbColon);
            else return matriceFichier;
        }
        /*FS1.1 : Demande_Taille()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service demande à l'utilisateur s'il souhaite changer la taille de la grille de jeu.
         *      Et attribut selon le choix une taille en nbLigne et nbColon, et retourner ensuite un booléen pour "O" vrai et "N" faux.
         * INPUT : (entier) nbColon, nbLigne
         * OUTPUT : (booléen)
         */
        public static bool Demande_Taille(ref int nbLigne, ref int nbColon)
        {
            Console.WriteLine($"Souhaitez-vous agrandir la grille de jeu ? O/N\nSa taille actuelle est de {nbLigne} lignes x {nbColon} colonnes.");

            bool choix = (Convert.ToString((Console.ReadLine())) == "O") ? true : false;

            if (choix == true)
            {
                int saisieL = 0, saisieC = 0;
                do
                {
                    Console.WriteLine($"Saisissez le nouveau nbLigne compris entre {nbLigne} et 45");
                    saisieL = Convert.ToInt32(Console.ReadLine());
                } while (saisieL < nbLigne || saisieL > 45);

                do
                {
                    Console.WriteLine($"Saisissez le nouveau nbColonne entre {nbColon} et 100");
                    saisieC = Convert.ToInt32(Console.ReadLine());
                } while (saisieC < nbColon || saisieC > 100);

                nbLigne = saisieL; nbColon = saisieC; 
                
                Console.WriteLine("Appuyer sur une touche pour continuer"); Console.ReadLine();
            }
            return choix;
        }

        /*FS1.2 : Redimensionne_Matrice()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service redimensionne la grille de jeu en placant dans une nouvelle matrice, l'ancienne plus petite.
         *      Le placement se fait aléatoirement mais contient bien toute la matrice à placer.
         * INPUT : 
         *      (Matrice2D de cacractères) matriceFichier
         *      (entier) nbColon, nbLigne
         * OUTPUT : (Matrice2D de cacractères) matriceFichier
         */
        public static char[,] Redimensionne_Matrice(char[,] matriceFichier, int nbLigne, int nbColon)
        {
            char[,] nouvelleMatrice = new char[nbLigne, nbColon];

            int compteNbLigne = matriceFichier.GetLength(0), compteNbColon = matriceFichier.GetLength(1), indLigneMF = 0, indColonMF = 0;

            int coord_Y = new Random().Next(0 + (nbLigne - compteNbLigne)), coord_X = new Random().Next(0 + (nbColon - compteNbColon));

            for (int indiceLigne = 0; indiceLigne < nbLigne; indiceLigne++)
            {
                for (int indiceColonne = 0; indiceColonne < nbColon; indiceColonne++)
                {
                    if ( (indiceLigne >= coord_Y) && (indiceLigne <= coord_Y + compteNbLigne - 1) )
                    {
                        if ( (indiceColonne >= coord_X) && (indiceColonne <= coord_X + compteNbColon - 1) )
                        {
                            nouvelleMatrice[indiceLigne, indiceColonne] = matriceFichier[indLigneMF, indColonMF]; 
                            indColonMF++;
                        }
                        else nouvelleMatrice[indiceLigne, indiceColonne] = 'O';
                    }
                    else nouvelleMatrice[indiceLigne, indiceColonne] = 'O';
                }
                if ((indiceLigne >= coord_Y) && (indiceLigne <= coord_Y + compteNbLigne - 1)) indLigneMF++;
                indColonMF = 0;
            }
            return nouvelleMatrice;
        }

        /*FS1.3 : demandeFichier()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service demande à l'utilisateur le fichier à utiliser et vérifie s'il existe ou non afin de prévenir éventuellement l'utilisateur.
         *      
         * INPUT : (chaine de caractères) fichier
         * OUTPUT : -----
         */
        public static string demandeFichier()
        {
            string fichier;
            do
            {
                Console.WriteLine("Renseignez le nom avec l'extension du fichier souhaité > ");
                fichier = (Console.ReadLine()).ToString();
            } while (!File.Exists("../../../matrices/" + fichier));

            return fichier;
        }

        /*FS1.4 : chercheTailleMatrice()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service renvoie la taille en nbLigne et nbColonne d'une matrice
         * INPUT : (string) fichier
         * OUTPUT : (Tableau de 2 entiers)
         */
        public static int[] chercheTailleMatrice(string fichier)
        {
            string[] Lignes = File.ReadAllLines(@"../../../matrices/" + fichier);

            int compteLigne = 0, compteColonne = 0;


            foreach (string Ligne in Lignes)
            {
                if (Ligne != "")
                {
                    compteLigne +=1;

                    string[] colonnes = Ligne.Split('|');

                    foreach (string colonne in colonnes) if (colonne != "") compteColonne++;
                }
            }
            compteColonne = compteColonne / compteLigne;

            int[] Tablo = new int[2] { compteLigne, compteColonne };

            return Tablo;
        }

        /* FP2 : Traitement_Matricielle()
                 * Auteur : Baptiste Risse 21/12/2021
                 * Valeur ajoutée :
                 *      Ce service permet de traiter la matrice existante, pour ensuite retourner une nouvelle matrice.
                 * INPUT : matrice (2D) de caractère
                 * OUTPUT : nouvelle matrice (2D) de caractère
                 */
        public static char[,] Traitement_Matricielle(char[,] matrice)
        {
            int indiceLignes, indiceColonnes, nbVoisins, indiceDimensions;
            bool avenir;
            int[] Dimensions = new int[2];

            int nbDimensions = matrice.Rank;

            for (indiceDimensions = 0; indiceDimensions < nbDimensions; indiceDimensions++)
            {
                Dimensions[indiceDimensions] = matrice.GetLength(indiceDimensions);
            }

            char[,] newMatrice = new char[Dimensions[0], Dimensions[1]];

            for (indiceLignes = 0; indiceLignes < Dimensions[0]; indiceLignes++)
            {
                for (indiceColonnes = 0; indiceColonnes < Dimensions[1]; indiceColonnes++)
                {
                    nbVoisins = Compteur_Voisins(matrice, indiceLignes, indiceColonnes, Dimensions[0], Dimensions[1]);

                    avenir = Avenir_cellule(nbVoisins, matrice, indiceLignes, indiceColonnes);

                    newMatrice[indiceLignes, indiceColonnes] = Ecriture_Matricielle(avenir);

                }

            }
            Affichage_Jeu(newMatrice);

            return newMatrice;
        }

        /* FS2.1 : Compteur_Voisins()
                 * Auteur : Baptiste Risse 21/12/2021
                 * Valeur ajoutée :
                 *      Ce service permet de compter le nombre de voisins d'une case dans la matrice.
                 * INPUT : matrice (2D) de caractère, indiceLignes entier, indiceColonnes entier, nbLignes entier, nbColonnes entier
                 * OUTPUT : compteur entier
                 */
        public static int Compteur_Voisins(char[,] matrice, int indiceLignes, int indiceColonnes, int nbLignes, int nbColonnes)
        {
            int deuxiemeIndiceLignes, deuxiemeIndiceColonnes, ligneMin, ligneMax, colonneMin, colonneMax, compteur = 0;

            ligneMin = indiceLignes - 1;
            ligneMax = indiceLignes + 1;
            colonneMin = indiceColonnes - 1;
            colonneMax = indiceColonnes + 1;


            if (indiceLignes == 0 || indiceLignes == nbLignes - 1 || indiceColonnes == 0 || indiceColonnes == nbColonnes - 1)
            {
                Exceptions_Voisins(indiceLignes, indiceColonnes, nbLignes, nbColonnes, ref ligneMin, ref ligneMax, ref colonneMin, ref colonneMax);
            }


            for (deuxiemeIndiceLignes = ligneMin; deuxiemeIndiceLignes <= ligneMax; deuxiemeIndiceLignes++)
            {
                for (deuxiemeIndiceColonnes = colonneMin; deuxiemeIndiceColonnes <= colonneMax; deuxiemeIndiceColonnes++)
                {
                    if (matrice[deuxiemeIndiceLignes, deuxiemeIndiceColonnes] == 'X')
                    {
                        compteur++;
                    }
                }
            }
            if (matrice[indiceLignes, indiceColonnes] == 'X')
            {
                return compteur - 1;
            }
            else
            {
                return compteur;
            }

        }

        /* FT2.1.1 : Exceptions_Voisins()
         * Auteur : Baptiste Risse 21/12/2021
         * Valeur ajoutée :
         *      Ce service permet de retourner la limites des lignes et des colonnes si la case concernée se trouve sur un "bord" du tableau.
         * INPUT : indiceLignes entier, indiceColonnes entier, nbLignes entier, nbColonnes entier, ligneMin entier, ligneMax entier, colonneMin entier, colonneMax entier
         * OUTPUT : ligneMin entier, ligneMax entier, colonneMin entier, colonneMax entier
         */
        public static void Exceptions_Voisins(int indiceLignes, int indiceColonnes, int nbLignes, int nbColonnes, ref int ligneMin, ref int ligneMax, ref int colonneMin, ref int colonneMax)
        {
            if (indiceLignes == 0)
            {
                ligneMin = indiceLignes;
            }

            if (indiceLignes == nbLignes - 1)
            {
                ligneMax = indiceLignes;
            }

            if (indiceColonnes == 0)
            {
                colonneMin = indiceColonnes;
            }

            if (indiceColonnes == nbColonnes - 1)
            {
                colonneMax = indiceColonnes;
            }

        }

        /* FS2.2 : Avenir_cellule()
         * Auteur : Baptiste Risse 21/12/2021
         * Valeur ajoutée :
         *      Ce service permet d'identifier si une cellule sera vivante ou morte dans la prochaine matrice.
         *      Si la cellule est vivante la fonctin retourne true sinon elle retourne false.
         * INPUT : nbVoisins entier, matrice (2D) de caractère, indiceLignes entier, indiceColonnes entier
         * OUTPUT : true ou false
         */
        public static bool Avenir_cellule(int nbVoisins, char[,] matrice, int indiceLignes, int indiceColonnes)
        {
            if (matrice[indiceLignes, indiceColonnes] == 'X')
            {
                if (nbVoisins == 2 || nbVoisins == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (nbVoisins == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /*FS2.2 : Ecriture_Matricielle()
        * Auteur : Justin Ferdinand 21/12/2021
        * Valeur ajoutée : 
        *      Ce service permet d'écrire les caractères de la matrice en fonction de leurs état.
        *      Il renvoie X si la cellule est vivante (naissance ou survie), O si mort.
        * INPUT : booléen avenir
        * OUTPUT : X ou O
        */
        public static char Ecriture_Matricielle(bool avenir)
        {
            char newCase;
            if (avenir == true)
            {
                newCase = 'X';
            }
            else
            {
                newCase = 'O';
            }
            return newCase;
        }

        /*FS2.4 : Affichage_Jeu()
         * Auteur : Justin Ferdinand 21/12/2021
         * Valeur ajoutée : 
         *      Ce service permet d'afficher les cases de la matrice.
         *      Il peut mettre en pause pendant un instant le programme par l'appuie de la touche espace.
         * INPUT : Matrice2D de caractères, matriceActuelle de caractères.
         * OUTPUT : -----
         */
        public static void Affichage_Jeu(char[,] matriceActuelle)
        {
            int nbLigne = matriceActuelle.GetLength(0);
            int nbColon = matriceActuelle.GetLength(1);

            for (int indiceLigne = 0; indiceLigne < nbLigne; indiceLigne++)
            {
                for (int indiceColon = 0; indiceColon < nbColon; indiceColon++)
                {
                    Console.SetCursorPosition(indiceColon, indiceLigne);

                    if (matriceActuelle[indiceLigne, indiceColon] == 'O')
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write($"  ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($" ");
                    }
                }
                Console.WriteLine();
            }
            Thread.Sleep(20);
        }

        /*FP3 : Bool_continue()
            * Auteur : Justin Ferdinand 21/12/2021
            * Valeur ajoutée : 
            *      Ce service décide s'il faut continuer ou non en déterminant si l'état actuelle de la grille est stable ou non.
            * INPUT : 
            *      (Matrice2D de cacractères) ancienneMatrice, nouvelleMatrice
            * OUTPUT : (booléen)
            */
        public static bool Bool_continue(char[,] ancienneMatrice, char[,] nouvelleMatrice)
        {
            int nbLigne_de_ancienneMatrice = ancienneMatrice.GetLength(0);
            int nbColon_de_ancienneMatrice = ancienneMatrice.GetLength(1);

            int nbLigne_de_nouvelleMatrice = nouvelleMatrice.GetLength(0);
            int nbColon_de_nouvelleMatrice = nouvelleMatrice.GetLength(1);

            if ((nbColon_de_ancienneMatrice != nbColon_de_nouvelleMatrice) || (nbLigne_de_ancienneMatrice != nbLigne_de_nouvelleMatrice))
            {
                return false;
            }
            else
            {
                for (int indiceLigne = 0; indiceLigne < nbLigne_de_ancienneMatrice; indiceLigne++)
                {
                    for (int indiceColon = 0; indiceColon < nbColon_de_ancienneMatrice; indiceColon++)
                    {
                        if (ancienneMatrice[indiceLigne, indiceColon] != nouvelleMatrice[indiceLigne, indiceColon]) return false;
                    }
                }
                return true;
            }
        }

        static void Main(string[] args)
        {
            Interface_de_Depart();

            char[,] matrice = Lecture_Matricielle();

            bool boucle = false;

            Console.Clear();

            do
            {
                char[,] nouvelleMatrice = Traitement_Matricielle(matrice);

                boucle = Bool_continue(matrice, nouvelleMatrice);

                matrice = nouvelleMatrice;
            } while (boucle == false);
        }
    }
}
