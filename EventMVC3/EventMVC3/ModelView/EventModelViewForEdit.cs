using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventMVC3.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EventMVC3.ModelView
{
    public class EventModelViewForEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الحدث مطلوب.")]
        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? FineshTime { get; set; }

        public string? PlaceName { get; set; }

        public string? City { get; set; }

        public string Discription { get; set; } = null!;

        public int? UserId { get; set; }

        [Required(ErrorMessage = "التصنيف مطلوب.")]
        public int Category { get; set; }

        public int? Price { get; set; }

        public string? Image { get; set; }

        public string? ConstraintAge { get; set; }

        [ValidateNever]
        public EventCategory CategoryNavigation { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public User? User { get; set; }
    }
}
