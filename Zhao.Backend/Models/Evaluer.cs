using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Zhao.Backend.Extra;

namespace Zhao.Backend.Models
{
    public class Evaluer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key] public int Id { get; set; }

        public static readonly string[] ChoixType =
        {
            "Salle à manger",
            "Salon privé (événements)"
        };

        [Required(ErrorMessage = "Le prénom est requis.")]
        [StringLength(100, ErrorMessage = "Le prénom ne peut pas contenir plus de 100 caractères.")]
        [DisplayName("Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas contenir plus de 100 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le type de réservation est requis.")]
        [DisplayName("Type de réservation")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Le courriel est requis.")]
        [EmailAddress(ErrorMessage = "Veuillez saisir une adresse courriel valide")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            ErrorMessage = "Le format de l'adresse courriel est invalide.")]
        public string Courriel { get; set; }

        [Required(ErrorMessage = "La date de la visite est requises.")]
        [DataType(DataType.Date)]
        [DisplayName("Date de la visite")]
        public DateTime DateVisite { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La qualité du repas est requise.")]
        [Range(1, 5, ErrorMessage = "La qualité du repas est noté de 1 à 5.")]
        [DisplayName("Qualité du repas (1 à 5)")]
        public int QualiteRepas { get; set; }

        [Required(ErrorMessage = "La qualité du service est requise.")]
        [Range(1, 5, ErrorMessage = "La qualité du service est noté de 1 à 5.")]
        [DisplayName("Qualité du service (1 à 5)")]
        public int QualiteService { get; set; }

        public string Commentaires { get; set; }

        public string CourrielHtml { get; set; }
    }
}