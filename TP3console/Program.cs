using TP3console.Models.EntityFramework;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TP3console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new FilmsDbContext())
            {
                //Explicite(main)

                //Pas de modif dans la base
                //ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                //select nom from Film where nom = 'Titanic'
                Film titanic = ctx.Films.First(f => f.Nom.Contains("Titanic"));
                //update de la description
                titanic.Description = "Un bateau échoué . Date :" + DateTime.Now;
                //sauvegarde
                int nbChanges = ctx.SaveChanges();
                Console.WriteLine("Nombre d'enregistrements modifiés ou ajoutés : " + nbChanges);

                //Explicite
                Categorie categorieAction = ctx.Categories.First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction.Nom);
                //Chargement des films dans categorieAction
                ctx.Entry(categorieAction).Collection(c => c.Films).Load();
                Console.WriteLine("Films : ");
                foreach (var film in categorieAction.Films)
                {
                    Console.WriteLine(film.Nom);
                }

                //Hâtif

                //Chargement de la catégorie Action et des films de cette catégorie
                Categorie categorieAction2 = ctx.Categories
                .Include(c => c.Films)
                .First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction2.Nom);
                Console.WriteLine("Films : ");
                foreach (var film in categorieAction2.Films)
                {
                    Console.WriteLine(film.Nom);
                }

                //Chargement de la catégorie Action, des films de cette catégorie et des avis
                Categorie categorieAction3 = ctx.Categories
                .Include(c => c.Films)
                .ThenInclude(f => f.Avis)
                .First(c => c.Nom == "Action");



                //Chargement de la catégorie Action
                Categorie categorieAction1 = ctx.Categories.First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction1.Nom);
                Console.WriteLine("Films : ");
                //Chargement des films de la catégorie Action.
                foreach (var film in categorieAction1.Films) // lazy loading initiated
                {
                    Console.WriteLine(film.Nom);
                }
            }

            //Exo2Q1();
            //Exo2Q2();
            //Exo2Q3();
            //Exo2Q4();
            //Exo2Q5();
            Console.WriteLine("--------------------------");
            //Exo2Q6();
            //Exo2Q7();
            //Exo2Q8();
            //Exo2Q9();
            Console.ReadKey();


        }
        public static void Exo2Q1()
        {
            var ctx = new FilmsDbContext();
            foreach (var film in ctx.Films)
            {
                Console.WriteLine(film.ToString());
            }
        }
        //Autre possibilité :
        public static void Exo2Q1Bis()
        {
            var ctx = new FilmsDbContext();
            //Pour que cela marche, il faut que la requête envoie les mêmes noms de colonnes que les classes c#.
            var films = ctx.Films.FromSqlRaw("SELECT * FROM film");
            foreach (var film in films)
            {
                Console.WriteLine(film.ToString());
            }
        }

        public static void Exo2Q2()
        {
            var ctx = new FilmsDbContext();
            var emails = ctx.Utilisateurs.Select(u => u.Email).ToList();
            foreach (var email in emails)
            {
                Console.WriteLine(email);
            }
        }
        public static void Exo2Q3()
        {
            var ctx = new FilmsDbContext();
            var users = ctx.Utilisateurs.OrderBy(u => u.Login).ToList();
            foreach (var utilisateur in users)
            {
                Console.WriteLine(utilisateur.ToString());
            }
        }

        public static void Exo2Q4()
        {
            var ctx = new FilmsDbContext();

            var films = ctx.Films
                .Join(
                    ctx.Categories,
                    f => f.Idcategorie,
                    c => c.Idcategorie,
                    (f, c) => new
                    {
                        NomFilm = f.Nom,
                        IdFilm = f.Idfilm,
                        NomCategorie = c.Nom
                    }
                )
                .ToList();

            foreach (var film in films)
            {
                Console.WriteLine($"Film : {film.NomFilm} | IdFilm : {film.IdFilm}| Catégorie : {film.NomCategorie}");
            }
        }

        public static void Exo2Q5()
        {
            var ctx = new FilmsDbContext();
            var NbCategorie = ctx.Categories
                .Count();
            Console.WriteLine($"Nombre de categorie : {NbCategorie}");
        }

        public static void Exo2Q6()
        {
            var ctx = new FilmsDbContext();
            var basseNoteAvis = ctx.Avis.Min(a => a.Note);
            Console.WriteLine($"La note la plus basse : {basseNoteAvis}");
        }

        public static void Exo2Q7()
        {
            var ctx = new FilmsDbContext();
            var count = 0;
            var filtreFilms = ctx.Films
                .Where(f => f.Nom.StartsWith("Le"))
                .ToList();
            foreach (var film in filtreFilms)
            {
                Console.WriteLine($"Film qui commence avec 'Le' : {film.Nom}");
                count++;
            }
            Console.WriteLine($"Nombre de film qui commence avec 'Le' : {count}");
        }

        public static void Exo2Q8()
        {
            var ctx = new FilmsDbContext();
            var noteMoyenne = ctx.Avis.Where(a=>a.IdfilmNavigation.Nom.ToLower()=="pulp fiction").Average(a => a.Note);
            Console.WriteLine($"La note moyenne de Pulp Fiction : {Math.Round(noteMoyenne,2)}");
        }

        public static void Exo2Q9()
        {
            var ctx = new FilmsDbContext();

            var bestUser = ctx.Avis
                .OrderByDescending(a => a.Note)
                .Select(a => a.IdutilisateurNavigation) // ou a.IdutilisateurNavigation.Login
                .FirstOrDefault();

            if (bestUser == null)
            {
                Console.WriteLine("Aucun avis en base.");
                return;
            }

            Console.WriteLine(bestUser); // ou Console.WriteLine(bestUser.Login);
        }

        public static void ExoAjoutUtilisateur()
        {
            var ctx = new FilmsDbContext();

            // 1) Création + init
            var moi = new Utilisateur
            {
                Email = "moi@mail.com",
                Login = "moi",
                Pwd = "mdp" // si vous stockez en clair dans le TP (sinon hash)
            };

            // 2) Ajout au contexte (via la collection)
            ctx.Utilisateurs.Add(moi);

            // 3) Sauvegarde
            ctx.SaveChanges();

            Console.WriteLine($"Utilisateur ajouté : id={moi.Idutilisateur} login={moi.Login}");
        }

        public static void ExoModifierFilm()
        {
            var ctx = new FilmsDbContext();

            // Récupérer le film (case-insensitive)
            var film = ctx.Films
                .FirstOrDefault(f => f.Nom.ToLower() == "l'armee des douze singes");

            if (film == null)
            {
                Console.WriteLine("Film introuvable.");
                return;
            }

            // Récupérer la catégorie "Drame"
            var catDrame = ctx.Categories
                .FirstOrDefault(c => c.Nom.ToLower() == "drame");

            if (catDrame == null)
            {
                Console.WriteLine("Catégorie 'Drame' introuvable.");
                return;
            }

            // Modifs demandées
            film.Description = "Votre description ici (rajout TP).";
            film.Idcategorie = catDrame.Idcategorie;

            ctx.SaveChanges();

            Console.WriteLine("Film modifié (description + catégorie Drame).");
        }

        public static void ExoSupprimerFilm()
        {
            var ctx = new FilmsDbContext();

            var film = ctx.Films
                .FirstOrDefault(f => f.Nom.ToLower() == "l'armee des douze singes");

            if (film == null)
            {
                Console.WriteLine("Film introuvable.");
                return;
            }

            // 1) Supprimer les avis liés (FK sans cascade)
            var avisDuFilm = ctx.Avis.Where(a => a.Idfilm == film.Idfilm);
            ctx.Avis.RemoveRange(avisDuFilm);

            // 2) Supprimer le film
            ctx.Films.Remove(film);

            // 3) Sauvegarde
            ctx.SaveChanges();

            Console.WriteLine("Film + avis associés supprimés.");
        }

        public static void ExoAjouterAvis()
        {
            var ctx = new FilmsDbContext();

            // Récupérer "moi" (adapte le login)
            var moi = ctx.Utilisateurs.FirstOrDefault(u => u.Login.ToLower() == "moi");
            if (moi == null)
            {
                Console.WriteLine("Utilisateur 'moi' introuvable. Ajoute-toi d'abord.");
                return;
            }

            // Récupérer le film (adapte le nom)
            var film = ctx.Films.FirstOrDefault(f => f.Nom.ToLower() == "pulp fiction");
            if (film == null)
            {
                Console.WriteLine("Film introuvable.");
                return;
            }

            // Créer l'avis
            var avis = new Avi // ou Avis selon ton nom de classe
            {
                Idfilm = film.Idfilm,
                Idutilisateur = moi.Idutilisateur,
                Note = 5 // adapte l’échelle (ex 0..5 / 0..10)
                         // Commentaire = "Incroyable." // si ton modèle a un champ texte
            };

            ctx.Avis.Add(avis);
            ctx.SaveChanges();

            Console.WriteLine("Avis ajouté.");
        }

    }
}



