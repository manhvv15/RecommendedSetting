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
    /// Interaction logic for AgeChart.xaml
    /// </summary>
    public partial class AgeChart : UserControl
    {
        string dep_id;
        string time_mode;
        string time_start;
        string time_end;
        List<Entities.HomeEntity.Entity> listAllEmployee;
        ChartValues<int> chartValues1 = new ChartValues<int>();
        ChartValues<int> chartValues2 = new ChartValues<int>();
        ChartValues<int> chartValues3 = new ChartValues<int>();
        ChartValues<int> chartValues4 = new ChartValues<int>();
        public AgeChart(string dep_id, string time_mode, string time_start, string time_end, List<Entities.HomeEntity.Entity> listAllEmployee)
        {
            InitializeComponent();
            this.dep_id = dep_id;
            this.time_mode = time_mode;
            this.time_start = time_start;
            this.time_end = time_end;
            this.listAllEmployee = listAllEmployee;
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString("#4C5BD4");
            var brush2 = (Brush)converter.ConvertFromString("#3F964F");
            var brush3 = (Brush)converter.ConvertFromString("#FFA800");
            var brush4 = (Brush)converter.ConvertFromString("#D44C4C");
            SetupDataChart();
            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Title = "Dưới 30 tuổi",
                    Values = chartValues1,
                    StackMode = StackMode.Values,
                    Fill = brush1,
                    Stroke = brush1
                },
                new StackedColumnSeries
                {
                    Title = "30 - 44 tuổi",
                    Values = chartValues2,
                    StackMode = StackMode.Values,
                    Fill = brush2,
                    Stroke = brush2
                },
                new StackedColumnSeries
                {
                    Title = "45 - 59 tuổi",
                    Values = chartValues3,
                    StackMode = StackMode.Values,
                    Fill = brush3,
                    Stroke = brush3
                },
                new StackedColumnSeries
                {
                    Title = "Trên 60 tuổi",
                    Values = chartValues4,
                    StackMode = StackMode.Values,
                    Fill = brush4,
                    Stroke = brush4
                }
            };

            Formatter = value => value.ToString();
            DataContext = this;
            chart.AxisX[0].Separator.StrokeThickness = 0;
            //chart.AxisY[0].Separator.StrokeThickness = 0;
        }

        private void SetupDataChart()
        {
            var dates_string = new List<String>();
            DateTime dateStart = ConvertDate(time_start);
            DateTime dateEnd = ConvertDate(time_end);
            if (dateEnd < dateStart) return;
            // các ngày trong năm
            if (time_mode == "0")
            {

                for (var dt = dateStart; dt <= dateEnd; dt = dt.AddDays(1))
                {
                    string _dt = dt.ToString("dd/MM/yyyy");
                    SetValueChart(_dt);
                    dates_string.Add(_dt);
                }
                Labels = dates_string.ToArray();
            }
            // các tháng trong năm
            else if (time_mode == "1")
            {
                for (var dt = dateStart; dt <= dateEnd; dt = dt.AddMonths(1))
                {
                    string _dt = dt.ToString("MM/yyyy");
                    SetValueChart(_dt);
                    dates_string.Add("Tháng " + _dt);
                }
                Labels = dates_string.ToArray();
            }
            // các năm
            else
            {
                for (var dt = dateStart; dt <= dateEnd; dt = dt.AddYears(1))
                {
                    string _dt = dt.ToString("yyyy");
                    SetValueChart(_dt);
                    dates_string.Add("Năm " + _dt);
                }
                Labels = dates_string.ToArray();
            }

        }

        private void SetValueChart(string date)
        {
            int count = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            foreach (var item in listAllEmployee)
            {
                string time = ConvertDateToCompare(item.create_time);
                if (date == time)
                {
                    int age = GetaAgeOfEmployee(item.ep_birth_day);
                    if (dep_id == "")
                    {
                        if (age < 30 && age >=0)
                        {
                            count++;
                        }
                        else if (age >= 30 && age <= 44)
                        {
                            count2++;
                        }
                        else if (age >= 45 && age <= 59)
                        {
                            count3++;
                        }
                        else if (age >= 60)
                        {
                            count4++;
                        }
                    }
                    else
                    {
                        if (dep_id == item.dep_id)
                        {
                            if (age < 30 && age >= 0)
                            {
                                count++;
                            }
                            else if (age >= 30 && age <= 44)
                            {
                                count2++;
                            }
                            else if (age >= 45 && age <= 59)
                            {
                                count3++;
                            }
                            else if (age >= 60)
                            {
                                count4++;
                            }
                        }
                    }
                }
            }
            chartValues1.Add(count);
            chartValues2.Add(count2);
            chartValues3.Add(count3);
            chartValues4.Add(count4);
        }

        private int GetaAgeOfEmployee(string date)
        {
            DateTime myDate;
            int year_now = DateTime.Now.Year;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return year_now - myDate.Year;
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return year_now - myDate.Year;
                }
                catch
                {
                    return -1;
                }
            }
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
        public Func<double, string> Formatter { get; set; }

    }
}

