using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using ProgrammingInCSharp.Lab04.Exceptions;
using ProgrammingInCSharp.Lab04.Models;
using ProgrammingInCSharp.Lab04.Navigation;
using ProgrammingInCSharp.Lab04.Repository;
using ProgrammingInCSharp.Lab04.Tools;

namespace ProgrammingInCSharp.Lab04.ViewModels
{
    class AddPersonViewModel : INotifyPropertyChanged, INavigatable
    {

        #region Fields

        private static FileRepository _fileRepository = new();
        private Person _person;
        private DateTime _birthdate = DateTime.Today;
        private Action _gotoMain;

        #endregion

        #region Constructors

        public AddPersonViewModel(Action gotoMain)
        {
            _gotoMain = gotoMain;
        }

        #endregion

        #region Commands

        public MainNavigationTypes ViewType => MainNavigationTypes.AddPerson;
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand<object> _gotoMainCommand;
        private RelayCommand<object> _cancelCommand;

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate
        {
            get => _birthdate;
            set => _birthdate = value;
        }
        public RelayCommand<object> AddCommand
        {
            get
            {
                return _gotoMainCommand ??= new RelayCommand<object>(_ => Add(), CanExecute);
            }
        }

        public RelayCommand<object> CancelCommand
        {
            get
            {
                return _cancelCommand ??= new RelayCommand<object>(o => _gotoMain.Invoke());
            }
        }

        #endregion

        private async Task Add()
        {
            bool isAdult = false;
            string sunSign = "";
            string chineseSign = "";
            bool isBirthday = false;
            
            await Task.Run(() => isAdult = Person.FindIsAdult(Birthdate));
            await Task.Run(() => sunSign = Person.FindSunSign(Birthdate.Day, Birthdate.Month));
            await Task.Run(() => chineseSign = Person.FindChineseSign(Birthdate.Year));
            await Task.Run(() => isBirthday = Person.FindIsBirthday(Birthdate));
            
            try
            {
               _person = new Person(Name, Surname, Email, Birthdate, isAdult, sunSign, chineseSign, isBirthday);
            }
            catch (InvalidEmailException e)
            {
                MessageBox.Show(e.Message, "Error");
                return;
            }
            catch (FutureBirthdateException e)
            {
                MessageBox.Show(e.Message, "Error");
                return;
            }
            catch (PastBirthdateException e)
            {
                MessageBox.Show(e.Message, "Error");
                return;
            }

            MainViewModel.AddPersonToGrid(new PersonViewModel(_person, _gotoMain));
            await Task.Run(() => _fileRepository.AddOrUpdateAsync(_person));
            _gotoMain.Invoke();

            ClearAll();
        }

        private void ClearAll()
        {
            Name = "";
            Surname = "";
            Email = "";
            Birthdate = DateTime.Today;
        }

        private bool CanExecute(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Birthdate.ToString()) || string.IsNullOrWhiteSpace(Name) ||
                     string.IsNullOrWhiteSpace(Surname) || string.IsNullOrWhiteSpace(Email));
        }
    }
}
