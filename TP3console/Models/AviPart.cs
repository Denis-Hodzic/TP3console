using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3console.Models.EntityFramework
{
    public partial class Avi
    {
        public override bool Equals(object? obj)
        {
            return obj is Avi avi &&
                   Idfilm == avi.Idfilm &&
                   Idutilisateur == avi.Idutilisateur &&
                   Commentaire == avi.Commentaire &&
                   Note == avi.Note &&
                   EqualityComparer<Film>.Default.Equals(IdfilmNavigation, avi.IdfilmNavigation) &&
                   EqualityComparer<Utilisateur>.Default.Equals(IdutilisateurNavigation, avi.IdutilisateurNavigation);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Idfilm, Idutilisateur, Commentaire, Note, IdfilmNavigation, IdutilisateurNavigation);
        }

        public override string ToString()
        {
            return $"Note : {Note}";
        }
    }
}
