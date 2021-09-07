using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zhao.Backend.Models
{
    public class Promotion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Le type de promotion est requis.")]
        [DisplayName("Type de promotion")]
        public string Type { get; set; }

        public static List<SelectListItem> ChoixType { get; } = new()
        {
            new SelectListItem { Value = "Tous", Text = "Tous" },
            new SelectListItem { Value = "Comptoir", Text = "Comptoir" },
            new SelectListItem { Value = "Salle à manger", Text = "Salle à manger" },
            new SelectListItem { Value = "Livraison", Text = "Livraison" }
        };

        [Required(ErrorMessage = "Le taux applicable est requis.")]
        [DisplayName("Taux applicable")]
        [DisplayFormat(DataFormatString = "{0}%")]
        public int TauxApplicable { get; set; }

        [Required(ErrorMessage = "La date de début est requise.")]
        [DisplayName("Date de début")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [DisplayName("Date de fin")]
        [DataType(DataType.Date)]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "La description de la promotion est requise.")]
        public string Description { get; set; }
    }

    public enum PromotionChoixType
    {
        Tous = 0,
        Comptoir,
        SalleManger,
        Livraison,
    }
}