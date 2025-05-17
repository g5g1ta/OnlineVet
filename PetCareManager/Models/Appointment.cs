public enum AppointmentStatus
{
    Pending,
    Completed,
    Cancelled     
}
namespace PetCareManager.Models{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }   
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public int VetId { get; set; }
        public User Vet { get; set; }
        public AppointmentStatus Status { get; set; }

    }
}