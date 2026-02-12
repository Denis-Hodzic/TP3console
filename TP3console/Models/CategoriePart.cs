using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3console.Models.EntityFramework
{
    public partial class Categorie
    {
        public override bool Equals(object? obj)
        {
            return obj is Categorie categorie &&
                   Idcategorie == categorie.Idcategorie &&
                   Nom == categorie.Nom &&
                   Description == categorie.Description &&
                   EqualityComparer<ICollection<Film>>.Default.Equals(Films, categorie.Films);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Idcategorie, Nom, Description, Films);
        }

        public override string ToString()
        {
            return $"Nom : {Nom}";
        }
    }
}
