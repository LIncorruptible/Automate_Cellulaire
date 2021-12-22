using System;
using System.IO;

namespace Automate_Cellulaire
{
    class Program
    {
        /*
         * FP0 : Interface_de_Depart()
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

        /*
         * FP1 : Lecture_Matricielle()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service doit à partir d'un fichier contenant une "matrice" et des choix utilisateurs,
         *      traduire cette dernière en tableau à 2 dimensions de caractères.
         * INPUT : -----
         * OUTPUT : Matrice2D de caractères
         */
        public static char[,] Lecture_Matricielle()
        {
            string fichier = "default.txt";

            do
            {
                Console.WriteLine("Renseignez le nom avec l'extension du fichier souhaité > ");
                fichier = (Console.ReadLine()).ToString();
            } while (!File.Exists("../../../matrices/" + fichier));

            char[,] matriceFichier = { };
            int compteLigne = 0, compteColonne = 0, nbColon = 0, nbLigne = 0;


            foreach (string Ligne in File.ReadAllLines(@"../../../matrices/" + fichier))
            {
                foreach (string colonne in Ligne.Split("||"))
                {
                    matriceFichier[compteLigne, compteColonne] = Convert.ToChar(colonne); compteColonne++; nbColon = compteColonne;
                } compteLigne++; nbLigne = compteLigne;
            }

            bool choix = Demande_Taille(nbLigne, nbColon);

            if (choix == true)
            {
                return Redimensionne_Matrice(matriceFichier, nbColon, nbLigne);
            } else
            {
                return matriceFichier;
            }
        }

        /*
         * FS1.1 : Lecture_Matricielle()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service demande à l'utilisateur s'il souhaite changer la taille de la grille de jeu.
         *      Et attribut selon le choix une taille en nbLigne et nbColon, et retourner ensuite un booléen pour "O" vrai et "N" faux.
         * INPUT : (entier) nbColon, nbLigne
         * OUTPUT : (booléen)
         */
        public static bool Demande_Taille(int nbLigne, int nbColon)
        {
            Console.WriteLine("Souhaitez-vous agrandir la grille de jeu ? O/N");
            Console.WriteLine($"Sa taille actuelle est de {nbLigne} lignes x {nbColon} colonnes.");

            bool choix = ((Console.ReadLine()).ToString() == "O") ? true : false;

            if (choix == true)
            {
                do
                {
                    Console.WriteLine($"Saisissez le nouveau nbLigne > {nbLigne} : ");
                } while (Convert.ToInt32(Console.ReadLine()) <= nbLigne);

                nbLigne = Convert.ToInt32(Console.ReadLine());

                do
                {
                    Console.WriteLine($"Saisissez le nouveau nbColonne > {nbColon} : ");
                } while (Convert.ToInt32(Console.ReadLine()) <= nbColon);

                nbColon = Convert.ToInt32(Console.ReadLine());
            } return choix;
        }

        /*
         * FS1.2 : Redimensionne_Matrice()
         * Auteur : Maël Rhuin 21/12/2021
         * Valeur ajoutée : 
         *      Ce service redimensionne la grille de jeu en placant dans une nouvelle matrice, l'ancienne plus petite.
         *      Le placement se fait aléatoirement mais contient bien toute la matrice à placer.
         * INPUT : 
         *      (Matrice2D de cacractères) matriceFichier
         *      (entier) nbColon, nbLigne
         * OUTPUT : (Matrice2D de cacractères) matriceFichier
         */
        public static char[,] Redimensionne_Matrice(char[,] matriceFichier, int nbColon, int nbLigne)
        {
            char[,] nouvelleMatrice = { };

            int compteNbLigne = matriceFichier.GetLength(0);
            int compteNbColon = matriceFichier.GetLength(1);

            int coord_Y = new Random().Next(0 + (nbLigne - compteNbLigne));
            int coord_X = new Random().Next(0 + (nbColon - compteNbColon));

            for (int indiceLigne = 0; indiceLigne < nbLigne; indiceLigne++)
            {
                for (int indiceColonne = 0; indiceColonne < nbColon; indiceColonne++)
                {
                    if (((indiceLigne <= coord_Y) || (indiceLigne >= (coord_Y - compteNbLigne))) && ((indiceColonne >= coord_X) || (indiceColonne <= (coord_X + compteNbColon))))
                    {
                        nouvelleMatrice[indiceLigne, indiceColonne] = matriceFichier[(indiceLigne - coord_Y), (indiceColonne - coord_X)];
                    }
                    else nouvelleMatrice[indiceLigne, indiceColonne] = 'O';
                }
            } return nouvelleMatrice;
        }

        static void Main(string[] args)
        {

        }   
    }
}
