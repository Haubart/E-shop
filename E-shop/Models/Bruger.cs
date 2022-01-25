namespace E_shop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Bruger
    {
        public int BrugerID { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string ForNavn { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string EfterNavn { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        [DataType(DataType.Password)]
        public string Adgangskode { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string Mail { get; set; }
        public Nullable<bool> status { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string Adresse { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string Postnr { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string By { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Dette felt er n�dvendigt.")]
        public string Land { get; set; }

        public string LoginErrorMessage { get; set; }

    }
}