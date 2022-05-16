using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ProgrammingInCSharp.Lab04.Navigation;
using ProgrammingInCSharp.Lab04.Repository;
using ProgrammingInCSharp.Lab04.Tools;

namespace ProgrammingInCSharp.Lab04.ViewModels
{
    class MainViewModel : INotifyPropertyChanged, INavigatable
    {

        #region Fields

        private static FileRepository _fileRepository;
        private string _wordToSort;
        private string _wordToFilter;
        private List<string> _allColumns;
        private List<string> _allFilterColumns;
        private bool _isFiltered;
        private Action _gotoMain;
        private Action _gotoAddPerson;
        private Action<PersonViewModel> _gotoPerson;


        #endregion

        #region Constructors
        public MainViewModel(Action gotoAddPerson, Action<PersonViewModel> gotoPerson, Action gotoMain)
        {
            _gotoAddPerson = gotoAddPerson;
            _gotoPerson = gotoPerson;
            _gotoMain = gotoMain;
            _fileRepository = new FileRepository();
            _people = new ObservableCollection<PersonViewModel>(_fileRepository.GetAll(gotoMain));
            _grid = new ObservableCollection<PersonViewModel>(_fileRepository.GetAll(gotoMain));

            _allColumns = new List<string>
            {
                "Name",
                "Surname",
                "Email",
                "Birthdate",
                "Is adult?",
                "Sun sign",
                "Chinese sign",
                "Is birthday?"
            };

            _allFilterColumns = new List<string>
            {
                "Adults",
                "Children",
                "Is birthday",
                "No birthday"
            };
        }

        #endregion

        #region Commands

        public MainNavigationTypes ViewType => MainNavigationTypes.Main;
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand<object> _addPersonCommand;
        private RelayCommand<object> _changePersonCommand;
        private RelayCommand<object> _removePersonCommand;
        private RelayCommand<object> _sortCommand;
        private RelayCommand<object> _filterCommand;
        private RelayCommand<object> _cancelCommand;

        private static ObservableCollection<PersonViewModel> _people;
        private static ObservableCollection<PersonViewModel> _grid;

        #endregion

        #region Properties

        public PersonViewModel SelectedPerson { get; set; }

        public ObservableCollection<PersonViewModel> People
        {
            get => _people;
            private set
            {
                _people = value;
                OnPropertyChanged();
            }

        }

        public ObservableCollection<PersonViewModel> Grid
        {
            get => _grid;
            private set
            {
                _grid = value;
                OnPropertyChanged();
            }
        }

        public List<string> AllColumns => _allColumns;
        public List<string> AllFilterColumns => _allFilterColumns;

        public string WordToSort
        {
            get => _wordToSort;
            set
            {
                _wordToSort = value;
                OnPropertyChanged();
            }
        }

        public string WordToFilter
        {
            get => _wordToFilter;
            set
            {
                _wordToFilter = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand<object> AddPersonCommand
        {
            get
            {
                return _addPersonCommand ??= new RelayCommand<object>(_ => GotoAddPerson());
            }
        }

        public RelayCommand<object> ChangePersonCommand
        {
            get
            {
                return _changePersonCommand ??= new RelayCommand<object>(_ => GoToPerson(), CanExecute);
            }
        }

        public RelayCommand<object> RemovePersonCommand
        {
            get
            {
                return _removePersonCommand ??= new RelayCommand<object>(o => RemovePerson(), CanExecute);
            }
        }

        public RelayCommand<object> SortCommand
        {
            get
            {
                return _sortCommand ??= new RelayCommand<object>(_ => Sort(), CanExecuteForSort);
            }
        }

        public RelayCommand<object> FilterCommand
        {
            get
            {
                return _filterCommand ??= new RelayCommand<object>(o => FilterPeople(), CanExecuteForFilter);
            }
        }

        public RelayCommand<object> CancelCommand
        {
            get
            {
                return _cancelCommand ??= new RelayCommand<object>(o => CancelFilter(), CanExecuteForCancel);
            }
        }

        #endregion

        private void GotoAddPerson()
        {
            _gotoAddPerson.Invoke();
        }

        public void GoToPerson()
        {
            _gotoPerson.Invoke(SelectedPerson);
        }

        private async Task RemovePerson()
        {
            if (SelectedPerson != null)
            {
                await Task.Run(() => _fileRepository.Remove(SelectedPerson.Person));
                People.Remove(SelectedPerson);
                Grid.Remove(SelectedPerson);
                OnPropertyChanged(nameof(_people));
                OnPropertyChanged(nameof(_grid));
            }
        }

        public static void AddPersonToGrid(PersonViewModel person)
        {
            if(_people != null && !_people.Contains(person))
            {
                _people.Add(person);
                _grid.Add(person);
            }
        }

        private void Sort()
        {
            if (WordToSort.Equals("Name"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.Name).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.Name).ToList());
            }
            else if (WordToSort.Equals("Surname"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.Surname).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.Surname).ToList());
            }
            else if (WordToSort.Equals("Email"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.Email).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.Email).ToList());
            }
            else if (WordToSort.Equals("Birthdate"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.Birthdate).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.Birthdate).ToList());
            }
            else if (WordToSort.Equals("Is adult?"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.IsAdult).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.IsAdult).ToList());
            }
            else if (WordToSort.Equals("Sun sign"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.SunSign).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.SunSign).ToList());
            }
            else if (WordToSort.Equals("Chinese sign"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.ChineseSign).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.ChineseSign).ToList());
            }
            else if (WordToSort.Equals("Is birthday?"))
            {
                People = new ObservableCollection<PersonViewModel>(_people.OrderBy(person => person.IsBirthday).ToList());
                Grid = new ObservableCollection<PersonViewModel>(_grid.OrderBy(person => person.IsBirthday).ToList());
            }
        }

        private void FilterPeople()
        {
            if(_wordToFilter.Equals("Adults")) 
                Grid = new ObservableCollection<PersonViewModel>(_grid.Where(p => p.IsAdult).ToList());
            else if (_wordToFilter.Equals("Children")) 
                Grid = new ObservableCollection<PersonViewModel>(_grid.Where(p => !p.IsAdult).ToList());
            else if (_wordToFilter.Equals("Is birthday"))
                Grid = new ObservableCollection<PersonViewModel>(_grid.Where(p => p.IsBirthday).ToList());
            else if (_wordToFilter.Equals("Children"))
                Grid = new ObservableCollection<PersonViewModel>(_grid.Where(p => !p.IsBirthday).ToList());
            _isFiltered = true;
        }

        private void CancelFilter()
        {
            Grid = new ObservableCollection<PersonViewModel>(People);
            _isFiltered = false;
        }

        private bool CanExecute(object o)
        {
            return SelectedPerson != null;
        }

        private bool CanExecuteForSort(object o)
        {
            return WordToSort != null;
        }

        private bool CanExecuteForFilter(object o)
        {
            return !_isFiltered;
        }

        private bool CanExecuteForCancel(object o)
        {
            return _isFiltered;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
