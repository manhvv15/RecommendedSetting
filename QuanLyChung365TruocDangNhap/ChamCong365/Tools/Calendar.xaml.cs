using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Calender.xaml
    /// </summary>
    public partial class Calendar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Calendar()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public class itemz
        {
            public int Day { get; set; }
            public bool LastMonth { get; set; } = false;
            public bool Today { get; set; } = false;
        }

        private double _DayItemWidth;
        public double DayItemWidth
        {
            get { return _DayItemWidth; }
        }

        private List<itemz> _Days;
        public List<itemz> Days
        {
            get { return _Days; }
            set { _Days=value; OnPropertyChanged(); }
        }

        private int _Month;
        public int Month
        {
            get { return _Month; }
            set { _Month = value;OnPropertyChanged(); }
        }

        private int _Year;
        public int Year
        {
            get { return _Year; }
            set { _Year = value; OnPropertyChanged(); }
        }

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set
            {
                SetValue(SelectedDateProperty, value);
            }
        }
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(Calendar), new PropertyMetadata(null));



        public SelectionChangedEventHandler SelectionChanged
        {
            get { return (SelectionChangedEventHandler)GetValue(SelectionChangedProperty); }
            set { SetValue(SelectionChangedProperty, value); }
        }
        public static readonly DependencyProperty SelectionChangedProperty =
            DependencyProperty.Register("SelectionChanged", typeof(SelectionChangedEventHandler), typeof(Calendar));



        private void LoadDay(int year, int month)
        {
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
            else
            {
                if (year - 1 > 0)
                {
                    for (int i = 0; i < start; i++)
                    {
                        var x = DateTime.DaysInMonth(year - 1, month);
                        _Days.Add(new itemz() { Day = x - i, LastMonth = true });
                    }
                    _Days.Reverse();
                }
            }
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                var d = new itemz() { Day = i };

                if (year == DateTime.Now.Year)
                    if (month == DateTime.Now.Month)
                        if (i == DateTime.Now.Day) d.Today = true;

                _Days.Add(d);
            }
            OnPropertyChanged("Days");
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv.SelectedIndex >= 0)
            {
                Days.ForEach(i=> {
                    if (i.Today) i.Today = false;
                });
                Days = Days.ToList();
                var item = lv.SelectedItem as itemz;
                if (!item.LastMonth)
                {
                    SelectedDate = new DateTime(Year, Month, item.Day);
                }
                else
                {
                    if (Month- 1 > 0)
                    {
                        SelectedDate = new DateTime(Year, Month-1, item.Day);
                    }
                    else
                    {
                        if (Year-1>0)
                        {
                            SelectedDate = new DateTime(Year-1, 12, item.Day);
                        }
                    }
                }
                if (SelectionChanged != null) SelectionChanged(sender,e);
            }
        }

        private void Previous(object sender, MouseButtonEventArgs e)
        {
            if (Month - 1 > 0)
            {
                Month--;
            }
            else if (Month - 1 == 0) {
                Month = 12;
                Year--;
            }
            LoadDay(Year, Month);
        }

        private void NextMonth(object sender, MouseButtonEventArgs e)
        {
            if (Month + 1 <=12)
            {
                Month++;
            }
            else if (Month + 1 == 13)
            {
                Month = 1;
                Year++;
            }
            LoadDay(Year, Month);
        }

        private void lv_Loaded(object sender, RoutedEventArgs e)
        {
            _DayItemWidth = lv.ActualWidth / 7 - .5;
            OnPropertyChanged("DayItemWidth");

            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
            LoadDay(Year,Month);
        }

        private void lv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _DayItemWidth = lv.ActualWidth / 7 - .5;
            OnPropertyChanged("DayItemWidth");
        }
    }
}
