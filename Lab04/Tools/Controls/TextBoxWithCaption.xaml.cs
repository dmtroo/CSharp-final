using System.Windows;
using System.Windows.Controls;

namespace ProgrammingInCSharp.Lab04.Tools.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithCaption.xaml
    /// </summary>
    public partial class TextBoxWithCaption : UserControl
    {
        public TextBoxWithCaption()
        {
            InitializeComponent();
        }
        public string Caption
        {
            get
            {
                return TBoxCaption.Text;
            }
            set
            {
                TBoxCaption.Text = value;
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
        (
            "Text",
            typeof(string),
            typeof(TextBoxWithCaption),
            new PropertyMetadata(null)
        );

    }
}