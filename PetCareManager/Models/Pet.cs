namespace PetCareManager.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }   
        public DateTime DateOfBirth { get; set; }  
        public string Gender { get; set; }
        public string MedicalHistory { get; set; }  

        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}

