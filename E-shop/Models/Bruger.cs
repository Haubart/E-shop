//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_shop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    
    public partial class Bruger
    {
        public int BrugerID { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string ForNavn { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string EfterNavn { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string Adgangskode { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string Mail { get; set; }
        public Nullable<bool> status { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string Adresse { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string Postnr { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string By { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Dette felt er nødvendigt.")]
        public string Land { get; set; }

    }
}
