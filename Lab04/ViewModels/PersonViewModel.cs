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
    class PersonViewModel : INotifyPropertyChanged, INavigatable
    {

        #region Fields

        private static FileRepository _fileRepository = new();
        private Person _person;
        private Action _gotoMain;

        #endregion

        #region Constructors

        public PersonViewModel(Person person, Action gotoMain)
        {
            _person = person;
            _gotoMain = gotoMain;

            NewName = Name;
            NewSurname = Surname;
            NewBirthdate = Birthdate;
        }

        #endregion

        #region Commands

        public MainNavigationTypes ViewType => MainNavigationTypes.Person;
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand<object> _cancelCommand;
        private RelayCommand<object> _gotoMainCommand;

        #endregion

        #region Properties

        public Person Person => _person;
        public string Name => _person.Name;
        public string Surname => _person.Surname;
        public string Email => _person.Email;
        public DateTime Birthdate => _person.Birthdate;
        public bool IsAdult => _person.IsAdult;
        public string SunSign => _person.SunSign;
        public string ChineseSign => _person.ChineseSign;
        public bool IsBirthday => _person.IsBirthday;
        public string StringBirthdate => _person.StringBirthdate;

        public string NewName { get; set; }
        public string NewSurname { get; set; }
        public DateTime NewBirthdate { get; set; }

        public RelayCommand<object> EditCommand
        {
            get
            {
                return _gotoMainCommand ??= new RelayCommand<object>(_ => Edit(), CanExecute);
            }
        }
        public RelayCommand<object> CancelCommand
        {
            get
            {
                return _cancelCommand ??= new RelayCommand<object>(o => Cancel());
            }
        }
        

        #endregion

        private async Task Edit()
        {
            bool isAdult = false;
            string sunSign = "";
            string chineseSign = "";
            bool isBirthday = false;

            await Task.Run(() => isAdult = Person.FindIsAdult(NewBirthdate));
            await Task.Run(() => sunSign = Person.FindSunSign(NewBirthdate.Day, NewBirthdate.Month));
            await Task.Run(() => chineseSign = Person.FindChineseSign(NewBirthdate.Year));
            await Task.Run(() => isBirthday = Person.FindIsBirthday(NewBirthdate));

            try
            {
                _person = new Person(NewName, NewSurname, Email, NewBirthdate, isAdult, sunSign, chineseSign, isBirthday);
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

            await Task.Run(() => _fileRepository.AddOrUpdateAsync(_person));
            _gotoMain.Invoke();
        }
       
        private void Cancel()
        {
            _gotoMain.Invoke();
        }

        private bool CanExecute(object o)
        {
            return !(string.IsNullOrWhiteSpace(NewBirthdate.ToString()) || string.IsNullOrWhiteSpace(NewName) || string.IsNullOrWhiteSpace(NewSurname));
        }
    }
}
