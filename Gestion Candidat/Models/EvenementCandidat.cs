//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestion_Candidat.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EvenementCandidat
    {
        public int CdEvenement { get; set; }
        public int CdCandidat { get; set; }
        public string CdResp { get; set; }
        public Nullable<int> TypEvenement { get; set; }
        public string CommentaireEvenement { get; set; }
        public string DtEvenement { get; set; }
        public Nullable<System.DateTime> DtCreation { get; set; }
    
        public virtual Candidat Candidat { get; set; }
        public virtual Salarie Salarie { get; set; }
        public virtual typEvenementCandidat typEvenementCandidat { get; set; }
    }
}
