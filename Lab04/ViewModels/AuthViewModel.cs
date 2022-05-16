using ProgrammingInCSharp.Lab04.Navigation;

namespace ProgrammingInCSharp.Lab04.ViewModels
{

    
    class AuthViewModel : BaseNavigationViewModel
    {
        
        public AuthViewModel()
        {
            Navigate(MainNavigationTypes.Main);
        }

        protected override INavigatable CreateViewModel(MainNavigationTypes type)
        {
            switch (type)
            {
                case MainNavigationTypes.AddPerson:
                    return new AddPersonViewModel(() => Navigate(MainNavigationTypes.Main));
                case MainNavigationTypes.Main:
                    return new MainViewModel(() => Navigate(MainNavigationTypes.AddPerson), person => NavigateToPerson(person), () => Navigate(MainNavigationTypes.Main));
                default:
                    return null;
            }
        }

    }
}
