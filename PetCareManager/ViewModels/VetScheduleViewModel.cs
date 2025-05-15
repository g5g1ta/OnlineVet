namespace PetCareManager.ViewModels
{
    public class VetScheduleViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public TimeSpan? AvailableFrom { get; set; }
        public TimeSpan? AvailableTo { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAvailable { get; set; }
    }
}