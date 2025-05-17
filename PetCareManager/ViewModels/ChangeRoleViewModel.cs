namespace PetCareManager.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string Role { get; set; }
        public TimeSpan? AvailableFrom { get; set; }
        public TimeSpan? AvailableTo { get; set; }
    }
}