using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.HRReportPages.HRReportPages.Charts
{
    /// <summary>
    /// Interaction logic for NewStaffChart.xaml
    /// </summary>
    public partial class NewStaffChart : UserControl
    {
        string dep_id;
        string time_mode;
        string time_start;
        string time_end;
        List<Entities.HomeEntity.Entity> listAllEmployee;
        ChartValues<int> chartValues = new ChartValues<int>();
        public NewStaffChart(string dep_id, string time_mode, string time_start, string time_end, List<Entities.HomeEntity.Entity> listAllEmployee)
        {
            InitializeComponent();
            this.dep_id = dep_id;
            this.time_mode = time_mode;
            this.time_start = time_start;
            this.time_end = time_end;
            this.listAllEmployee = listAllEmployee;
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#4C5BD4");

            SetupDataChart();
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = null,
                    Values = chartValues,
                    PointForeground = brush,
                    DataLabels = true,
                    Fill = Brushes.Transparent,
                    Stroke = brush
                },
            };
            YFormatter = value => value.ToString("N1");
            DataContext = this;
        }


        private void SetupDataChart()
        {
            var dates_string = new List<String>();
            DateTime dateStart = ConvertDate(time_start);
            DateTime dateEnd = ConvertDate(time_end);
            // các ngày trong năm
            if (time_mode == "0")
            {
                if (dateEnd < dateStart) return;
                for (var dt = dateStart; dt <= dateEnd; dt = dt.AddDays(1))
                {
                    string _dt = dt.ToString("dd/MM/yyyy");
                    SetValueChart(_dt);
                    dates_string.Add(_dt);
                }
                Labels = dates_string.ToArray();
            }
            // các tháng trong năm
            else if(time_mode == "1")
            {
                DateTime startMonth = ConvertDate(time_start);
                DateTime endMonth = ConvertDate(time_end);
                if (startMonth > endMonth) return;
                for (var dt = dateStart; dt <= dateEnd; dt = dt.AddMonths(1))
                {
                    string _dt = dt.ToString("MM/yyyy");
                    SetValueChart(_dt);
                    dates_string.Add("Tháng " + dt.Month);
                }
                Labels = dates_string.ToArray();
            }
            // các năm
            else
            {
                int startYear = ConvertDate(time_start).Year;
                int endYear = ConvertDate(time_end).Year;
                if (startYear > endYear) return;
                for (var dt = dateStart; dt <= dateEnd; dt = dt.AddYears(1))
                {
                    string _dt = dt.ToString("yyyy");
                    SetValueChart(_dt);
                    dates_string.Add("Năm " + dt.Year);
                }
                Labels = dates_string.ToArray();
            }
            
        }

        private void SetValueChart(string date)
        {
            int count = 0;
            foreach (var item in listAllEmployee)
            {
                string time = ConvertDateToCompare(item.create_time);
                if (date == time)
                {
                    if (dep_id == "")
                    {
                        count++;
                    }
                    else
                    {
                        if (dep_id == item.dep_id) count++;
                    }
                }
            }
            chartValues.Add(count);
        }

        private DateTime ConvertDate(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "MM/dd/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate;
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "M/d/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate;
                }
                catch
                {
                    return new DateTime();
                }
            }

        }
        private string ConvertDateToCompare(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss",
                                              System.Globalization.CultureInfo.InvariantCulture);
                if(time_mode == "0")
                {
                    return myDate.ToString("dd/MM/yyyy");
                }else if(time_mode == "1")
                {
                    return myDate.ToString("MM/yyyy");
                }
                else
                {
                    return myDate.ToString("yyyy");
                }
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                              System.Globalization.CultureInfo.InvariantCulture);
                    if (time_mode == "0")
                    {
                        return myDate.ToString("dd/MM/yyyy");
                    }
                    else if (time_mode == "1")
                    {
                        return myDate.ToString("MM/yyyy");
                    }
                    else
                    {
                        return myDate.ToString("yyyy");
                    }
                }
                catch
                {
                    return "";
                }
            }

        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

    }
}