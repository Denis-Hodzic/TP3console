using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3console.Models.EntityFramework
{
    public partial class Film
    {
        public override bool Equals(object? obj)
        {
            return obj is Film film &&
                   Idfilm == film.Idfilm &&
                   Nom == film.Nom &&
                   Description == film.Description &&
                   Idcategorie == film.Idcategorie &&
                   EqualityComparer<ICollection<Avi>>.Default.Equals(Avis, film.Avis) &&
                   EqualityComparer<Categorie>.Default.Equals(IdcategorieNavigation, film.IdcategorieNavigation);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Idfilm, Nom, Description, Idcategorie, Avis, IdcategorieNavigation);
        }

        public override string ToString()
        {
            return $"-------------------------\nNom : {Nom}\nDescription : {Description}\nAvis : {Avis}";
        }
    }
}
