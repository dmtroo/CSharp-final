using System.Windows.Controls;
using ProgrammingInCSharp.Lab04.ViewModels;

namespace ProgrammingInCSharp.Lab04.Views
{
    /// <summary>
    /// Interaction logic for AuthView.xaml
    /// </summary>
    public partial class AuthView : UserControl
    {
        public AuthView()
        {
            InitializeComponent();
            DataContext = new AuthViewModel();
        }
    }
}
