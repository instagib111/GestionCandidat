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
    using System.ComponentModel.DataAnnotations;

    public partial class FichierCandidat
    {
        public int CdFichier { get; set; }
        [Display(Name = "Trigramme")]
        public int CdCandidat { get; set; }
        [Display(Name = "Type fichier")]
        public int TypFichier { get; set; }
        public string Nom { get; set; }
        public string LienPath { get; set; }
        [Display(Name = "Créé le")]
        public System.DateTime DtCreation { get; set; }
        [Display(Name = "Modifié le")]
        public System.DateTime DtModification { get; set; }
    
        public virtual Candidat Candidat { get; set; }
        public virtual typFichierCandidat typFichierCandidat { get; set; }
    }
}
