using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zhao.Backend.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key] public int Id { get; set; }

        public readonly string[] ChoixType =
        {
            "Salle à manger",
            "Salon privé (événements)"
        };

        public static List<SelectListItem> ChoixConfirmation { get; } = new()
        {
            new SelectListItem { Value = "Non-confirmée", Text = "Non-confirmée" },
            new SelectListItem { Value = "Confirmée", Text = "Confirmée" },
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

        [Required(ErrorMessage = "La date et l'heure de la réservation sont requises.")]
        [DataType(DataType.DateTime)]
        [DisplayName("Date et heure de réservation")]
        public DateTime DateHeure { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire.")]
        [DisplayName("Numéro de téléphone")]
        [Phone]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Le nombre de personne(s) est obligatoire.")]
        [DisplayName("Nombre de personne(s)")]
        [Range(1, int.MaxValue, ErrorMessage = "Il doit au moins avoir une personne pour réserver la table.")]
        public int NbPersonnes { get; set; } = 1;
        [DisplayName("Statut de la réservation")]
        public string Confirmation { get; set; }

        public string CourrielHtml { get; set; }
    }
}