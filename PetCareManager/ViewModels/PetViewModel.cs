namespace PetCareManager.ViewModels
{
    public class PetViewModel
    {
        public int PetId{get; set;}
        public string Name { get; set; }
        public string Breed { get; set; }   
        public DateTime DateOfBirth { get; set; }  

        public string? MedicalHistory { get; set; }
        public string Gender { get; set; }

        
    }

}