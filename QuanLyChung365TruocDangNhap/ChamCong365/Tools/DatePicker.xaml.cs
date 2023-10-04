using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public DatePicker()
        {
            InitializeComponent();
            this.DataContext = this;
            _Years = new List<int>();
            var y = DateTime.Now.Year;
            for (int i = 1; i <40; i++)
            {
                _Years.Add(y-i);
            }
            _Years.Reverse();
            for (int i = 0; i < 20; i++)
            {
                _Years.Add(y + i);
            }
            OnPropertyChanged("Years");
            _Months = new List<string>();
            for (int i = 1; i <=12; i++)
            {
                _Months.Add("Tháng " +i.ToString());
            }
            OnPropertyChanged("Months");

        }

        public class itemz {
            public int Day { get; set; }
            public bool LastMonth { get; set; } = false;
            public bool Today { get; set; } = false;
        }

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(DatePicker));

        private double _DayItemWidth;

        public double DayItemWidth
        {
            get { return _DayItemWidth; }
        }

        private List<itemz> _Days;

        public List<itemz> Days
        {
            get { return _Days; }
        }
        private List<int> _Years;

        public List<int> Years
        {
            get { return _Years; }
        }
        private List<string> _Months;

        public List<string> Months
        {
            get { return _Months; }
        }



        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { 
                SetValue(SelectedDateProperty, value);
                var z = (DateTime?)GetValue(SelectedDateProperty);
                if (z!=null)
                {
                    Part_TextBox.Text = z.Value.ToString("dd/MM/yyyy");
                }
            }
        }
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePicker), new PropertyMetadata(null));



        private void LoadDay(int year,int month) {
            var start = (int)new DateTime(year, month, 1).DayOfWeek;
            _Days = new List<itemz>();
            if (month - 1 > 0)
            {
                for (int i = 0; i < start; i++)
                {
                    var x = DateTime.DaysInMonth(year, month - 1);
                    _Days.Add(new itemz() { Day = x - i, LastMonth = true });
                }
                _Days.Reverse();
            }
            else {
                if (year - 1 > 0)
                {
                    for (int i = 0; i < start; i++)
                    {
                        var x = DateTime.DaysInMonth(year-1, month);
                        _Days.Add(new itemz() { Day = x - i, LastMonth = true });
                    }
                    _Days.Reverse();
                }
            }
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                var d = new itemz() { Day = i };

                if (year==DateTime.Now.Year) 
                if (month==DateTime.Now.Month) 
                if(i == DateTime.Now.Day)d.Today = true;

                _Days.Add(d);
            }
            OnPropertyChanged("Days");
        }
        private void textBoxFormat() {
            var error = true;
            var txt = Part_TextBox.Text.Trim();
            if (txt.Length >= 7 && txt.Length <= 10)
            {
                DateTime dateValue;
                if (DateTime.TryParseExact(txt, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
                {
                    SelectedDate = dateValue;
                    Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy");
                    error = false;
                }
                else if (txt.Length >= 7 && txt.Length <= 8)
                {
                    int day = 0; int month = 0; int year = 0;
                    if (txt.Length == 8)
                    {
                        var ck = int.TryParse(txt.Substring(0, 2), out day) && (int.TryParse(txt.Substring(2, 2), out month)) && int.TryParse(txt.Substring(4, 4), out year);
                        if (ck)
                        {
                            if (month > 0 && month <= 12)
                            {
                                int days = DateTime.DaysInMonth(year, month);
                                if (days >= day)
                                {
                                    var d = string.Format("{0}/{1}/{2}", day, month, year);
                                    SelectedDate = DateTime.Parse(d);
                                    Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy");
                                    error = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        var ck = int.TryParse(txt.Substring(0, 2), out day) && (int.TryParse(txt.Substring(2, 1), out month)) && int.TryParse(txt.Substring(3, 4), out year);
                        if (ck)
                        {
                            if (month > 0 && month <= 12)
                            {
                                int days = DateTime.DaysInMonth(year, month);
                                if (days >= day)
                                {
                                    var d = string.Format("{0}/{1}/{2}", day, month, year);
                                    SelectedDate = DateTime.Parse(d);
                                    Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy");
                                    error = false;
                                }
                              
                            }
                        }
                    }

                }
                else if (txt.Length >= 9 && txt.Length <= 10)
                {
                    if (txt.Length == 10)
                    {
                        var checkint = 0;
                        var t = int.TryParse(txt[2].ToString(), out checkint) || int.TryParse(txt[5].ToString(), out checkint);
                        if (!t)
                        {
                            int day = 0; int month = 0; int year = 0;
                            var ck = int.TryParse(txt.Substring(0, 2), out day) && (int.TryParse(txt.Substring(3, 2), out month)) && int.TryParse(txt.Substring(5, 4), out year);
                            if (ck)
                            {
                                if (month > 0 && month <= 12)
                                {
                                    int days = DateTime.DaysInMonth(year, month);
                                    if (days >= day)
                                    {
                                        var d = string.Format("{0}/{1}/{2}", day, month, year);
                                        SelectedDate = DateTime.Parse(d);
                                        Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy");
                                        error = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var checkint = 0;
                        var t = int.TryParse(txt[2].ToString(), out checkint) || int.TryParse(txt[4].ToString(), out checkint);
                        if (!t)
                        {
                            int day = 0; int month = 0; int year = 0;
                            var ck = int.TryParse(txt.Substring(0, 2), out day) && (int.TryParse(txt.Substring(3, 1), out month)) && int.TryParse(txt.Substring(5, 4), out year);
                            if (ck)
                            {
                                if (month > 0 && month <= 12)
                                {
                                    int days = DateTime.DaysInMonth(year, month);
                                    if (days >= day)
                                    {
                                        var d = string.Format("{0}/{1}/{2}", day, month, year);
                                        SelectedDate = DateTime.Parse(d);
                                        Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy");
                                        error = false;
                                    }
                                }
                              
                            }
                        }
                    }
                }
            }
            if (error)
            {
                Part_TextBox.Text = "";
                SelectedDate = null;
            }
            else
            {
                Part_TextBox.SelectionStart = Part_TextBox.Text.Length;

            }
        }
        private void grdTextBox(object sender, MouseButtonEventArgs e)
        {
            Part_TextBox.Focus();
            //Part_TextBox.SelectAll();
        }

        private void Part_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                textBoxFormat();
            }
        }

        private void lv_Loaded(object sender, RoutedEventArgs e)
        {
            _DayItemWidth = lv.ActualWidth / 7-.5;
            OnPropertyChanged("DayItemWidth");

            cboMonth.SelectedIndex = DateTime.Now.Month - 1;
            cboYear.SelectedIndex = Years.IndexOf(DateTime.Now.Year);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
     
        }

        private void cboMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var year = 0;
            var month = 0;

            if (cboYear.SelectedIndex >= 0) year = Years[cboYear.SelectedIndex];
            if (cboMonth.SelectedIndex >= 0) month = cboMonth.SelectedIndex + 1 ;

            if(year>0 && month>0)LoadDay(year,month);
        }

        private void Previous(object sender, MouseButtonEventArgs e)
        {

            if(cboMonth.SelectedIndex-1>=0)cboMonth.SelectedIndex--;
            else if (cboYear.SelectedIndex - 1 >=0)
            {
                cboMonth.SelectedIndex = Months.Count-1;
                cboYear.SelectedIndex--;
            }
        }

        private void NextMonth(object sender, MouseButtonEventArgs e)
        {
            if (cboMonth.SelectedIndex + 1 < Months.Count) cboMonth.SelectedIndex++;
            else if (cboYear.SelectedIndex + 1 < Years.Count) {
                cboMonth.SelectedIndex = 0;
                cboYear.SelectedIndex++;
            }
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv.SelectedIndex>=0)
            {
                var item = lv.SelectedItem as itemz;
                if (!item.LastMonth)
                {
                    SelectedDate = new DateTime(Years[cboYear.SelectedIndex], cboMonth.SelectedIndex + 1, item.Day);
                    Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy") ;
                }
                else {
                    if (cboMonth.SelectedIndex - 1 >= 0)
                    {
                        SelectedDate = new DateTime(Years[cboYear.SelectedIndex], cboMonth.SelectedIndex, item.Day);
                        Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy");
                    }
                    else {
                        if (cboYear.SelectedIndex - 1 < Years.Count)
                        {
                            if (cboYear.SelectedIndex - 1 >= 0) {
                                SelectedDate = new DateTime(Years[cboYear.SelectedIndex - 1], Months.Count, item.Day);
                                Part_TextBox.Text = SelectedDate.Value.ToString("dd/MM/yyyy");
                            }
                        }
                    }
                }

                pop.IsOpen = false;
            }

        }

        private void pop_Opened(object sender, EventArgs e)
        {
            lv.SelectedIndex = -1;
        }

        private void Part_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            textBoxFormat();
        }
    }
}
