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
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for SelectTimeCombobox.xaml
    /// </summary>
    public partial class SelectTimeCombobox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SelectTimeCombobox()
        {
            InitializeComponent();
            this.DataContext = this;

            listMinute = new List<string>();
            for (int i = 0; i < 60; i++)
            {
                if (i < 10) listMinute.Add("0" + i.ToString());
                else listMinute.Add(i.ToString());

            }
        }
        //
        Regex regex = new Regex("[^0-9]+");

        public List<string> listHour { get; set; } = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", };
        public List<string> listMinute { get; set; }
        public List<string> listCa { get; set; } = new List<string> { "SA", "CH" };

        public Double CornerRadius
        {
            get { return (Double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(Double), typeof(SelectTimeCombobox));



        public string Time
        {
            get
            {
                var h = txtH.Text;
                var m = txtM.Text;
                var c = txtC.Text;
                if(!string.IsNullOrEmpty(h) && !string.IsNullOrEmpty(m) && !string.IsNullOrEmpty(c))
                {
                    if (c == "SA")
                    {

                    }
                    else if (c == "CH")
                    {
                        int k;
                        if (int.TryParse(h, out k)) h = (k + 12).ToString();
                    }

                    return string.Format("{0}:{1}:00", h, m);
                }
                else
                {
                    return null;
                }
                

            }
            set
            {
                SetValue(TimeProperty, value);
                var z = (string)GetValue(TimeProperty);
                if (!string.IsNullOrEmpty(z) && z.Contains(":"))
                {
                    var list = z.Split(':').ToList();
                    if (list.Count >= 3)
                    {
                        int m, h;
                        if (int.TryParse(list[0], out h) && h > 0)
                        {
                            if (h <= 12)
                            {
                                lvCaIndex = listCa.FindIndex(i => i == "SA");
                                CaLam = "SA";
                                txtC.Text = CaLam;

                                Hour = list[0];
                                txtH.Text = Hour;
                            }
                            else
                            {
                                lvCaIndex = listCa.FindIndex(i => i == "CH");
                                CaLam = "CH";
                                txtC.Text = CaLam;

                                h = h - 12;
                                if (h < 10) Hour = "0" + h.ToString();
                                else Hour = h.ToString();

                                txtH.Text = Hour;
                            }
                        }
                        if (int.TryParse(list[1], out m) && m >= 0)
                        {
                            lvMinuteIndex = listMinute.FindIndex(i => i == list[1]);
                            Minute = list[1];
                            txtM.Text = Minute;
                        }
                    }
                }
            }
        }
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(SelectTimeCombobox));

        private string _Hour;
        public string Hour
        {
            get { return _Hour; }
            set
            {
                _Hour = value;
                OnPropertyChanged();
            }
        }

        private string _Minute;
        public string Minute
        {
            get { return _Minute; }
            set
            {
                _Minute = value;
                OnPropertyChanged();
            }
        }

        private string _CaLam;
        public string CaLam
        {
            get { return _CaLam; }
            set
            {
                _CaLam = value;
                OnPropertyChanged();
            }
        }


        private int _lvHourIndex;
        public int lvHourIndex
        {
            get
            {
                return _lvHourIndex;
            }

            set
            {
                _lvHourIndex = value;
                OnPropertyChanged();
            }
        }

        private int _lvMinuteIndex;
        public int lvMinuteIndex
        {
            get
            {
                return _lvMinuteIndex;
            }

            set
            {
                _lvMinuteIndex = value;
                OnPropertyChanged();
            }
        }

        private int _lvCaIndex;
        public int lvCaIndex
        {
            get
            {
                return _lvCaIndex;
            }

            set
            {
                _lvCaIndex = value;
                OnPropertyChanged();
            }
        }
        //gio
        private void txtH_GotFocus(object sender, RoutedEventArgs e)
        {
            Hour = "";
        }
        private void txtH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);

            var txt = sender as TextBox;
            var x = Hour + e.Text;
            int hour;
            if (int.TryParse(x, out hour))
            {
                if (hour <= 12)
                {
                    if (hour < 10) txt.Text = "0" + hour.ToString();
                    else txt.Text = hour.ToString();
                }
                else
                {
                    txt.Text = "0" + hour.ToString()[0].ToString();
                }
                Hour = hour.ToString();
                if (Hour.Length == 2)
                {
                    txtM.Focus();
                }
            }
        }
        private void txtH_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listHour.Contains(txtH.Text)) lvHourIndex = listHour.FindIndex(i => i == txtH.Text);
        }
        //phut
        private void txtM_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Minute)) txtM.Text = "";
            else if (listMinute.Contains(txtM.Text)) lvMinuteIndex = listMinute.FindIndex(i => i == txtM.Text);
        }
        private void txtM_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);

            var txt = sender as TextBox;
            var x = Minute + e.Text;
            int minute;
            if (int.TryParse(x, out minute))
            {
                if (minute <= 59)
                {
                    if (minute < 10) txt.Text = "0" + minute.ToString();
                    else txt.Text = minute.ToString();
                }
                else
                {
                    txt.Text = "0" + minute.ToString()[0].ToString();
                }
                Minute = minute.ToString();
                if (Minute.Length == 2)
                {
                    txtC.Focus();
                }
            }
        }
        private void txtM_GotFocus(object sender, RoutedEventArgs e)
        {
            Minute = "";
        }
        //ca
        private void txtC_GotFocus(object sender, RoutedEventArgs e)
        {
            CaLam = "";
        }
        private void txtC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CaLam)) txtC.Text = "";
        }
        private void txtC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            List<string> k = new List<string> { "S", "A", "C", "H" };
            var txt = e.Text.ToUpper();
            if (k.Contains(txt))
            {
                var ck = false;
                listCa.ForEach(i =>
                {
                    if (i.Contains(txt)) ck = true;
                });
                if (ck)
                {
                    CaLam = listCa.Find(i => i.Contains(txt));
                    txtC.Text = listCa.Find(i => i.Contains(txt));
                    if (listCa.Contains(txtC.Text)) lvCaIndex = listCa.FindIndex(i => i == txtC.Text);
                }
                else txtC.Text = "";
            }
            else e.Handled = true;

        }
        //
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var c = sender as ComboBox;
            c.Margin = new Thickness(0 - this.Padding.Left, 0, 0, 0);
        }
        private void Popup_Closed(object sender, EventArgs e)
        {
            btn.IsChecked = false;
        }
        private void lvHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = (sender as ListView).SelectedItem as string;
            if (!string.IsNullOrEmpty(z))
            {
                txtH.Text = z;
                Minute = "";
            }
        }
        private void lvHour_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ListView).ScrollIntoView((sender as ListView).SelectedItem);
        }
        private void lvMinute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = (sender as ListView).SelectedItem as string;
            if (!string.IsNullOrEmpty(z))
            {
                Minute = z;
                txtM.Text = z;
                CaLam = "";
            }
        }
        private void lvCa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = (sender as ListView).SelectedItem as string;
            if (!string.IsNullOrEmpty(z))
            {
                CaLam = z;
                txtC.Text = z;
            }
        }
    }
}
