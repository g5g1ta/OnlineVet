namespace PetCareManager.ViewModels
{
    public class ScheduleViewModel
    {
        public int VetId { get; set; }  
        public TimeSpan AvailableFrom { get; set; }
        public TimeSpan AvailableTo { get; set; }
        public bool IsAvailable { get; set; }
    }
}
