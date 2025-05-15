namespace PetCareManager.Models
{
    public class MedicalRecord
    {
        public int MedicalRecordId { get; set; }
        public string Description { get; set; }  
        public DateTime Date { get; set; }  

        public int PetId { get; set; }
        public Pet Pet { get; set; }

    
        public int VetId { get; set; }
        public User Vet { get; set; }
}


}