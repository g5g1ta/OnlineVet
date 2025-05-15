using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetCareManager.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; } = "Unknown";
        public string Breed { get; set; } = "Unknown";
        public DateTime DateOfBirth { get; set; }  
        public string Gender { get; set; } = "Unknown";
        public string? MedicalHistory { get; set; }  

        [Required]
        public int OwnerId { get; set; }

        [BindNever]
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}

