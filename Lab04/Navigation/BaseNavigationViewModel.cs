using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ProgrammingInCSharp.Lab04.ViewModels;

namespace ProgrammingInCSharp.Lab04.Navigation
{

    internal abstract class BaseNavigationViewModel : INotifyPropertyChanged
    {
        List<INavigatable> _viewModels = new();
        private INavigatable _currentViewModel;

        public INavigatable CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            private set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        internal void Navigate(MainNavigationTypes type)
        {
            if (CurrentViewModel != null && CurrentViewModel.ViewType.Equals(type))
                return;
            
            INavigatable viewModel = GetViewModel(type);

            if (viewModel == null)
                return;

            _viewModels.Add(viewModel);
            CurrentViewModel = viewModel;
        }

        internal void NavigateToPerson(PersonViewModel viewModel)
        {
            CurrentViewModel = viewModel;
        }

        private INavigatable GetViewModel(MainNavigationTypes type)
        {
            INavigatable viewModel = _viewModels.FirstOrDefault(viewModel => viewModel.ViewType == type);
            
            if (viewModel != null)
                return viewModel;

            return CreateViewModel(type);
        }

        protected abstract INavigatable CreateViewModel(MainNavigationTypes type);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    enum MainNavigationTypes
    {
        AddPerson,
        Main,
        Person,
    }
}
