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
            Exo2Q6();
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
    }
}



