using System;
using System.Windows;
using System.Windows.Controls;

namespace ProgrammingInCSharp.Lab04.Tools.Controls
{
    /// <summary>
    /// Interaction logic for DatePickerWithCaption.xaml
    /// </summary>
    public partial class DatePickerWithCaption : UserControl
    {
        public DatePickerWithCaption()
        {
            InitializeComponent();
        }
        public string Caption
        {
            get { return DpCaption.Text; }
            set { DpCaption.Text = value; }
        }
        public DateTime? SelectedDate
        {
            get { return (DateTime?) GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
            "SelectedDate",
            typeof(DateTime?),
            typeof(DatePickerWithCaption),
            new PropertyMetadata(null)
        );

    }
}
