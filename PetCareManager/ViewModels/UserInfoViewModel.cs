namespace PetCareManager.ViewModels
{
    class UserInfoViewModel 
    {
        public string Email{get; set;}
        public string Username{get; set;}

         public List<string> Roles { get; set; } = new List<string>();
    }
}