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

    public partial class Role
    {
        public int CdRole { get; set; }
        public int CdHumain { get; set; }
        public string CdSalarie { get; set; }
        public Nullable<int> CdCandidat { get; set; }
        [Display(Name = "Type candidat")]
        public string TypTdb { get; set; }
        public Nullable<bool> IsResp { get; set; }
        public string CdResp { get; set; }
        public Nullable<bool> isSupport { get; set; }
        public Nullable<bool> IsCP { get; set; }
        public Nullable<bool> isBO { get; set; }
        public Nullable<bool> IsMO { get; set; }
        public Nullable<bool> isFO { get; set; }
        public Nullable<bool> isDAF { get; set; }

        public virtual Candidat Candidat { get; set; }
        public virtual Humain Humain { get; set; }
        public virtual Salarie Salarie { get; set; }
        public virtual Salarie Salarie1 { get; set; }
        public virtual typTdb typTdb1 { get; set; }
    }
}
