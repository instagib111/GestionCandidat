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

    public partial class Salarie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Salarie()
        {
            this.Candidat = new HashSet<Candidat>();
            this.Candidat1 = new HashSet<Candidat>();
            this.EvenementCandidat = new HashSet<EvenementCandidat>();
            this.FavorisCandidat = new HashSet<FavorisCandidat>();
            this.IDENTIFIANTSALARIE = new HashSet<IDENTIFIANTSALARIE>();
            this.Role = new HashSet<Role>();
            this.Role1 = new HashSet<Role>();
            this.TacheCandidat = new HashSet<TacheCandidat>();
            this.TacheCandidat1 = new HashSet<TacheCandidat>();
        }

        [Display(Name = "Trigramme")]
        public string CdSalarie { get; set; }
        public int CdHumain { get; set; }
        [Display(Name = "Nb. Enfant")]
        public Nullable<short> NbEnfant { get; set; }
        [Display(Name = "No. Sécurité Sociale")]
        public string NoSecuSocial { get; set; }
        [Display(Name = "Date de naissance")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DtNaissance { get; set; }
        public string LieuNaissance { get; set; }
        public string Nationalite { get; set; }
        public string Fonction { get; set; }
        public bool Cadre { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DtEntre { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DtEffet { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DtSortie { get; set; }
        public string RegionContrat { get; set; }
        public Nullable<double> SalaireAnnuel { get; set; }
        public string SalaireAnnuelLettre { get; set; }
        public Nullable<double> PrimemAnnuel { get; set; }
        public string PrimemAnnuelLettre { get; set; }
        public string CleRib { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DtDernierVisiteMedicale { get; set; }
        public string NbAnneeEtude { get; set; }
        public string NumPoste { get; set; }
        public string NumExt { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DtCreation { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DtModification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidat> Candidat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidat> Candidat1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EvenementCandidat> EvenementCandidat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavorisCandidat> FavorisCandidat { get; set; }
        public virtual Humain Humain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IDENTIFIANTSALARIE> IDENTIFIANTSALARIE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Role1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TacheCandidat> TacheCandidat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TacheCandidat> TacheCandidat1 { get; set; }
    }
}
