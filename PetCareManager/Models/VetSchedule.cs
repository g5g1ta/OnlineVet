namespace PetCareManager.Models;

public class VetSchedule
{
    public int VetScheduleId { get; set; }
    public int VetId { get; set; }
    public User Vet { get; set; }
    public TimeSpan AvailableFrom { get; set; } 
    public TimeSpan AvailableTo { get; set; }  

    public bool IsAvailable { get; set; }
}