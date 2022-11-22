using CosmosDemo.Domain.Models;
using CosmosDemo.Domain.Repositories;

namespace CosmosDemo.MAUI
{
    public partial class MainPage : ContentPage
    {
        private readonly IUserRepository _userRepository;

        public MainPage(IUserRepository userRepository)
        {
            InitializeComponent();

            _userRepository = userRepository;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            string userEmail = AppUserEmail.Text;
            AppUser user = await _userRepository.GetByEmail(userEmail);

            if(user != null)
            {
                await DisplayAlert("User Found!", $"Hello {user.Name}!", "OK");
            }
            else
            {
                await DisplayAlert("User Not Found!", $"User with email {userEmail} doesn't exist", "OK");
            }
        }
    }
}