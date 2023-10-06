using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Tools
{
    /// <summary>
    /// Interaction logic for NumericTextBox.xaml
    /// </summary>
    public partial class NumericTextBox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NumericTextBox()
        {
            InitializeComponent();
            this.DataContext = this;

            CornerRadius = 0;
            Value = 0;
            Maximun = 99999999;
            Minimun = 0;
        }



        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(int), typeof(NumericTextBox));



        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value);}
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericTextBox));

        public int Maximun
        {
            get { return (int)GetValue(MaximunProperty); }
            set { SetValue(MaximunProperty, value); }
        }
        public static readonly DependencyProperty MaximunProperty =
            DependencyProperty.Register("Maximun", typeof(int), typeof(NumericTextBox));

        public int Minimun
        {
            get { return (int)GetValue(MinimunProperty); }
            set { SetValue(MinimunProperty, value); }
        }
        public static readonly DependencyProperty MinimunProperty =
            DependencyProperty.Register("Minimun", typeof(int), typeof(NumericTextBox));

        private void UpClick(object sender, MouseButtonEventArgs e)
        {
            if(Value<Maximun)Value++;
            OnPropertyChanged("Value");
        }

        private void DownClick(object sender, MouseButtonEventArgs e)
        {
            if (Value-1 >= Minimun) Value--;
            OnPropertyChanged("Value");
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            var t = sender as TextBox;

            if (!regex.IsMatch(e.Text) && t.Text.Length>=1) {
                if (int.Parse(t.Text[0].ToString()) == 0) {
                    t.Text = t.Text.Substring(1);
                    t.SelectionStart = t.Text.Length;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = sender as TextBox;
            t.SelectionStart = t.Text.Length;
            OnPropertyChanged("Value");
        }
    }
}
