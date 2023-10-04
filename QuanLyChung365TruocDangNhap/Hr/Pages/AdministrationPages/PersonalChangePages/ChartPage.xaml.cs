using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.View;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.PersonalChangePages
{
    /// <summary>
    /// Interaction logic for ChartPage.xaml
    /// </summary>
    public partial class ChartPage : Page, INotifyPropertyChanged
    {
        string token;
        string id;
        int m11, m12, m13, m14, m15, m16, m21, m22, m23, m24, m25, m26;
        string title11 = "";
        string title12 = "";
        string title21 = "";
        string title22 = "";
        int TypeUser;



        private SeriesCollection seriesCollection;

        public SeriesCollection SeriesCollection1
        {
            get { return seriesCollection; }
            set
            {
                seriesCollection = value;
                OnPropertyChanged("SeriesCollection1");
            }
        }


        bool perAdd, perEdit, perDel, perShow; // quyền thêm, sửa, xóa,xem

        public bool PerAdd
        {
            get { return perAdd; }
            set
            {
                perAdd = value;
                OnPropertyChanged("PerAdd");
            }
        }

        public bool PerEdit
        {
            get { return perEdit; }
            set
            {
                perEdit = value;
                OnPropertyChanged("PerEdit");
            }
        }

        public bool PerDel
        {
            get { return perDel; }
            set
            {
                perDel = value;
                OnPropertyChanged("PerDel");
            }
        }
        public bool PerShow
        {
            get { return perShow; }
            set
            {
                perShow = value;
                OnPropertyChanged("PerShow");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        public ChartPage(string token, string id,int TypeUser, bool perAdd, bool perEdit, bool perDel, bool perShow)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            this.PerShow = perShow;
            DataContext = this;
 
            GetAllData();
            if (PerShow)
            {
                UpDownSalary.Visibility = Visibility.Visible;
            }
            else
            {
                UpDownSalary.Visibility = Visibility.Collapsed;
            }
        }

        private void GetAllData()
        {
            GetUpDownSalary1();
            GetUpDownSalary2();
            GetDataAppointment1();
            GetDataAppointment2();
            GetDataRotation1();
            GetDataRotation2();
            GetDataDownSizing1();
            GetDataDownSizing2();
        }

        /*private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dp1.IsDropDownOpen = true;
        }
        private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            dp2.IsDropDownOpen = true;
        }

        private void Border_MouseUp2(object sender, MouseButtonEventArgs e)
        {
            dp3.IsDropDownOpen = true;
        }

        private void Border_MouseUp3(object sender, MouseButtonEventArgs e)
        {
            dp4.IsDropDownOpen = true;
        }*/

        private void GenderChart()
        {
            var converter = new BrushConverter();
            var brush1 = (Brush)converter.ConvertFromString("#4C5BD4");
            var brush2 = (Brush)converter.ConvertFromString("#FFA800");

            SeriesCollection1 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Mốc thời gian 1: " +"(" + title11 + " - " + title12 + ")",
                    Values = new ChartValues<int> { m11, m12, m13, m14, m15, m16 },
                    DataLabels = true,
                    Stroke = brush1,
                }
            };

            SeriesCollection1.Add(new ColumnSeries
            {
                Title = "Mốc thời gian 2 " + "(" + title21 + " - " + title22 + ")",
                Values = new ChartValues<int> { m21, m22, m23, m24, m25, m26 },
                DataLabels = true,
                Stroke = brush2
            });

            //SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Tăng lương", "Giảm lương", "Bổ nhiệm, quy hoạch", "Luân chuyển công tác", "Giảm biên chế", "Nghỉ việc" };
            Formatter = value => value.ToString("N1");

            DataContext = this;
        }

        private async void GetUpDownSalary1()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string date1 = "";
                string date2 = "";
                if (dp1.SelectedDate != null)
                {
                    date1 = dp1.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (dp2.SelectedDate != null)
                {
                    date2 = dp2.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                string api = "http://210.245.108.202:3006/api/hr/report/reportDetail";

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new Dictionary<string, string>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add("link", "bieu-do-danh-sach-nhan-vien-tang-giam-luong.html");
                parameters.Add("winform", "1");
                parameters.Add("page", "1");
                parameters.Add("from_date", date1);
                parameters.Add("to_date", date2);
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                UpDownSalaryEntity result = JsonConvert.DeserializeObject<UpDownSalaryEntity>(responseContent);

                if (result.data != null)
                {
                    List<Data3> listUpDownSalaryEntity = result.data;
                    int count_up = 0;
                    int count_down = 0;
                    foreach(var item in listUpDownSalaryEntity)
                    {
                        if(item.sb_tanggiam > 0)
                        {
                            count_up++;
                        }
                        else
                        {
                            count_down++;
                        }
                    }
                    m11 = count_up;
                    m12 = count_down;
                    GenderChart();
                }

            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }

        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dp1.Text != "")
            {
                title11 = dp1.SelectedDate.Value.ToString("dd/MM/yyyy");
            }

            if (dp2.Text != "")
            {
                title12 = dp2.SelectedDate.Value.ToString("dd/MM/yyyy");
            }

            if (dp3.Text != "")
            {
                title21 = dp3.SelectedDate.Value.ToString("dd/MM/yyyy");
            }

            if (dp4.Text != "")
            {
                title22 = dp4.SelectedDate.Value.ToString("dd/MM/yyyy");
            }

            GetAllData();
        }

        private async void GetUpDownSalary2()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string date1 = "";
                string date2 = "";
                if (dp3.SelectedDate != null)
                {
                    date1 = dp3.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (dp4.SelectedDate != null)
                {
                    date2 = dp4.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                string api = "http://210.245.108.202:3006/api/hr/report/reportDetail";

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new Dictionary<string, string>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add("link", "bieu-do-danh-sach-nhan-vien-tang-giam-luong.html");
                parameters.Add("winform", "1");
                parameters.Add("page", "1");
                parameters.Add("pageSize", "1000000");
                parameters.Add("from_date", date1);
                parameters.Add("to_date", date2);
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                UpDownSalaryEntity result = JsonConvert.DeserializeObject<UpDownSalaryEntity>(responseContent);

                if (result.data != null)
                {
                    List<Data3> listUpDownSalaryEntity = result.data;
                    int count_up = 0;
                    int count_down = 0;
                    foreach (var item in listUpDownSalaryEntity)
                    {
                        if (item.sb_tanggiam > 0)
                        {
                            count_up++;
                        }
                        else
                        {
                            count_down++;
                        }
                    }
                    m21 = count_up;
                    m22 = count_down;
                    GenderChart();
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }

}

        private async void GetDataAppointment1()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api;
                string dateFrom = "";
                string dateTo = "";
                if (dp1.SelectedDate != null)
                {
                    dateFrom = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }
                if (dp2.SelectedDate != null)
                {
                    dateTo = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListAppoint";
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", id);
                form.Add("page", "1");
                form.Add("pageSize", "10000");
                form.Add("fromDate", dateFrom);
                form.Add("toDate", dateTo);
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                PlanningAppointmentList result = JsonConvert.DeserializeObject<PlanningAppointmentList>(responseContent);

                if (result.data != null)
                {
                    m13 = result.data.totalCount;
                    
                    GenderChart();
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }
        }

        private async void GetDataAppointment2()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api;
                string dateFrom = "";
                string dateTo = "";
                if (dp3.SelectedDate != null)
                {
                    dateFrom = dp3.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (dp4.SelectedDate != null)
                {
                    dateTo = dp4.SelectedDate.Value.ToString("yyyy-MM-dd");
                }

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListAppoint";
                var form = new Dictionary<string, string>();
                form.Add("page", "1");
                form.Add("pageSize", "1000000");
                form.Add("fromDate", dateFrom);
                form.Add("toDate", dateTo);
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                PlanningAppointmentList result = JsonConvert.DeserializeObject<PlanningAppointmentList>(responseContent);

                if (result.data != null)
                {
                    m23 = result.data.totalCount;
                    
                    GenderChart();
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }

        }  

        private async void GetDataRotation1()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api;
                string dateFrom = "";
                string dateTo = "";
                if (dp1.SelectedDate != null)
                {
                    dateFrom = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }
                if (dp2.SelectedDate != null)
                {
                    dateTo = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListTranferJob";

                httpRequestMessage.RequestUri = new Uri(api);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("fromDate", dateFrom);
                form.Add("toDate", dateTo);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListJobRotation result = JsonConvert.DeserializeObject<ListJobRotation>(responseContent);

                if (result.data != null)
                {
                    m14 = result.data.totalItems;
                    
                    GenderChart();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("error");
            }
        }

        private async void GetDataRotation2()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api;
                string dateFrom = "";
                string dateTo = "";
                if (dp3.SelectedDate != null)
                {
                    dateFrom = DateTime.ParseExact(dp3.SelectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }
                if (dp4.SelectedDate != null)
                {
                    dateTo = DateTime.ParseExact(dp4.SelectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListTranferJob";

                httpRequestMessage.RequestUri = new Uri(api);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("fromDate", dateFrom);
                form.Add("toDate", dateTo);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListJobRotation result = JsonConvert.DeserializeObject<ListJobRotation>(responseContent);

                if (result.data != null)
                {
                    m24 = result.data.totalItems;
                    
                    GenderChart();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("error");
            }
        }

        private async void GetDataDownSizing1()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api;
                string dateFrom = "";
                string dateTo = "";
                if (dp1.SelectedDate != null)
                {
                    dateFrom = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("M/d/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }
                if (dp2.SelectedDate != null)
                {
                    dateTo = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("M/d/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListQuitJob";

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var form = new Dictionary<string, string>();
                form.Add("page", "1");
                form.Add("pageSize", "1000000");
                form.Add("fromDate", dateFrom);
                form.Add("toDate", dateTo);
                form.Add("winform", "1");
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                DownsizingaEntity result = JsonConvert.DeserializeObject<DownsizingaEntity>(responseContent);

                if (result.data != null)
                {
                    int count1 = 0;
                    int count2 = 0;
                    foreach(var item in result.data.items)
                    {
                        if(item.type == "1")
                        {
                            count1++;
                        }
                        else if(item.type == "2")
                        {
                            count2++;
                        }
                    }
                    m15 = count1;
                    m16 = count2;
                    
                    GenderChart();
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }
        }

        private async void GetDataDownSizing2()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api;
                string dateFrom = "";
                string dateTo = "";
                if (dp3.SelectedDate != null)
                {
                    dateFrom = DateTime.ParseExact(dp3.SelectedDate.Value.ToString("M/d/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }
                if (dp4.SelectedDate != null)
                {
                    dateTo = DateTime.ParseExact(dp4.SelectedDate.Value.ToString("M/d/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture).Ticks.ToString();
                }

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListQuitJob";

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var form = new Dictionary<string, string>();
                form.Add("page", "1");
                form.Add("pageSize", "1000000");
                form.Add("fromDate", dateFrom);
                form.Add("toDate", dateTo);
                form.Add("winform", "1");
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                DownsizingaEntity result = JsonConvert.DeserializeObject<DownsizingaEntity>(responseContent);

                if (result.data != null)
                {
                    int count1 = 0;
                    int count2 = 0;
                    foreach (var item in result.data.items)
                    {
                        if (item.type == "1")
                        {
                            count1++;
                        }
                        else if (item.type == "2")
                        {
                            count2++;
                        }
                    }
                    m25 = count1;
                    m26 = count2;
                    
                    GenderChart();

                }
            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void NavigateToPage(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string name = textBlock.Name;
            switch (name)
            {
                case "Appointment":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new AppointmentPage(token, id,TypeUser, PerAdd, PerEdit, PerDel,PerShow));
                        }
                    };
                    break;
                case "Downsizing":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new DownsizingPage(token, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "UpDownSalary":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new UpDownSalaryPage(token, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "WorkingRotation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new WorkingRotationPage(token, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "BreakTheRules":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new BreakTheRules(token, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "Chart":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ChartPage(token, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
            }
        }
    }
}
