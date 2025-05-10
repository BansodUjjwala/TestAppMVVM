using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestAppMVVM.Models;
using TestAppMVVM.Services;
using TestAppMVVM.Utilities;

namespace TestAppMVVM.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private string _selectedCity = "All";
        private bool _isLoading;
        private string _errorMessage;

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<User> FilteredUsers { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (SetField(ref _selectedCity, value))
                {
                    FilterUsers();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        public ICommand LoadUsersCommand { get; }

        public MainViewModel(IUserService userService)
        {
            Users = new ObservableCollection<User>();
            FilteredUsers = new ObservableCollection<User>();
            Cities = new ObservableCollection<string> { "All" };
            _userService = userService;
            LoadUsersCommand = new RelayCommand(async () => await LoadUsersAsync());
        }

        private async Task LoadUsersAsync()
        {
            IsLoading = true;
            ErrorMessage = null;

            try
            {
                var users = await _userService.GetUsersAsync();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
                UpdateCities();
                FilterUsers();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateCities()
        {
            var uniqueCities = Users.Select(u => u.Address.City).Distinct().OrderBy(c => c);
            Cities.Clear();
            Cities.Add("All");
            foreach (var city in uniqueCities)
            {
                Cities.Add(city);
            }
        }

        private void FilterUsers()
        {
            FilteredUsers.Clear();
            var usersToShow = SelectedCity == "All"
                ? Users
                : Users.Where(u => u.Address.City == SelectedCity);

            foreach (var user in usersToShow)
            {
                FilteredUsers.Add(user);
            }
        }
    }
}
