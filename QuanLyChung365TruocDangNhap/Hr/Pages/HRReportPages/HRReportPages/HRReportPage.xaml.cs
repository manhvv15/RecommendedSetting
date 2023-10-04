using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Pages.HRReportPages.HRReportPages.Charts;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.HRReportPages.HRReportPages
{
    /// <summary>
    /// Interaction logic for HRReportPage.xaml
    /// </summary>
    public partial class HRReportPage : Page, INotifyPropertyChanged
    {
        private double width1;

        public double Width1
        {
            get { return width1; }
            set
            {
                width1 = value;
                OnPropertyChanged("Width1");
            }
        }

        private double width2;

        public double Width2
        {
            get { return width2; }
            set
            {
                width2 = value;
                OnPropertyChanged("Width2");
            }
        }

        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        string token;
        string com_id;
        // tổng nhân viên
        List<Entity> listAllEmployee = new List<Entity>();

        // giảm biên chế, nghỉ việc
        List<Entity> listAllEmployeeQuitJob = new List<Entity>();

        // bổ nhiệm, quy hoạch
        List<Entity> listAllEmployeeAppointment = new List<Entity>();

        // tăng giảm lương
        List<Entities.AdministrationEntity.PersonnelChangeEntity.Data3> listAllEmployeeSalary = new List<Entities.AdministrationEntity.PersonnelChangeEntity.Data3>();

        // luân chuyển công tác
        List<Entity> listAllEmployeeRotation = new List<Entity>();

        // danh sách phòng ban
        Dictionary<string, string> listAllDepartment = new Dictionary<string, string>();

        // danh sách lựa chọn thời gian
        Dictionary<string, string> listCbxTime = new Dictionary<string, string>();

        public HRReportPage(string token, string com_id)
        {
            InitializeComponent();
            this.token = token;
            this.com_id = com_id;
            DataContext = this;
            listCbxTime.Add("0", "Các ngày trong năm");
            listCbxTime.Add("1", "Các tháng trong năm");
            listCbxTime.Add("2", "Các năm");
            cbxSelectTime.ItemsSource = listCbxTime;
            cbxSelectTime.SelectedIndex = 0;
            GetAllEmployee();
            GetAllDepartment();
            GetAllEmployeeQuitJob();
            GetAllEmployeeAppointment();
            GetAllEmployeeSalary();
            GetAllEmployeeRotation();


            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }


        private async void GetAllDepartment()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = "http://210.245.108.202:3000/api/qlc/department/list";
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", com_id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listDepartment result = JsonConvert.DeserializeObject<listDepartment>(responseContent);
                if (result.data != null && result.data.items.Count > 0)
                {
                    listAllDepartment.Add("", "Chọn phòng ban");
                    List<Items6> listItem = result.data.items;
                    foreach (var item in listItem)
                    {
                        listAllDepartment.Add(item.dep_id, item.dep_name);
                    }

                    cbxDepartment.Items.Refresh();
                    cbxDepartment.ItemsSource = listAllDepartment;
                    cbxDepartment.SelectedIndex = 0;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetAllEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_all_employee;

                httpRequestMessage.RequestUri = new Uri(api);
                var form = new Dictionary<string, string>();
                form.Add("com_id", com_id);
                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                listAllEmployee = result.data.items;
                BindingData();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingData()
        {
            int maleTotalEmployee = 0;
            int femaleTotalEmployee = 0;
            int totalMarried = 0;
            int totalSingle = 0;
            int age1 = 0;
            int age2 = 0;
            int age3 = 0;
            int age4 = 0;
            int totalEducation1 = 0;
            int totalEducation2 = 0;
            int totalEducation3 = 0;
            int totalEducation4 = 0;
            int totalEducation5 = 0;
            int totalEducation6 = 0;
            int totalEducation7 = 0;
            int totalEducation8 = 0;
            int totalExp1 = 0;
            int totalExp2 = 0;
            int totalExp3 = 0;
            int totalExp4 = 0;
            int totalExp5 = 0;
            tbTotalEmployee.Text = listAllEmployee.Count.ToString();
            tbTotalEmployeeAge.Text = listAllEmployee.Count.ToString();
            tbTotalEmployeeEducation.Text = listAllEmployee.Count.ToString();
            tbTotalEmployeeExp.Text = listAllEmployee.Count.ToString();
            int year_now = DateTime.Now.Year;
            foreach (var item in listAllEmployee)
            {
                // giới tính nhân viên
                if (item.ep_gender == "1")
                {
                    maleTotalEmployee++;
                }
                else if (item.ep_gender == "2")
                {
                    femaleTotalEmployee++;
                }

                // tình trạng hôn nhân
                if (item.ep_married == "1")
                {
                    totalMarried++;
                }
                else if (item.ep_married == "2")
                {
                    totalSingle++;
                }

                // độ tuổi nhân viên
                int ep_year = GetYearOfBirth(item.ep_birth_day);
                if (ep_year > 0)
                {
                    int age = year_now - ep_year;
                    if (age < 30)
                    {
                        age1++;
                    }
                    else if (age >= 30 && age <= 44)
                    {
                        age2++;
                    }
                    else if (age >= 45 && age <= 59)
                    {
                        age3++;
                    }
                    else if (age >= 60)
                    {
                        age4++;
                    }
                }

                // trình độ học vấn nhân viên
                switch (item.ep_education)
                {
                    case "1":
                        totalEducation1++;
                        break;
                    case "2":
                        totalEducation2++;
                        break;
                    case "3":
                        totalEducation3++;
                        break;
                    case "4":
                        totalEducation4++;
                        break;
                    case "5":
                        totalEducation5++;
                        break;
                    case "6":
                        totalEducation6++;
                        break;
                    case "7":
                        totalEducation7++;
                        break;
                    case "8":
                        totalEducation8++;
                        break;
                }

                // thâm niên công tác
                switch (item.ep_exp)
                {
                    case "1":
                        totalExp1++;
                        break;
                    case "2":
                        totalExp2++;
                        break;
                    case "3":
                        totalExp3++;
                        break;
                    case "4":
                        totalExp3++;
                        break;
                    case "5":
                        totalExp4++;
                        break;
                    case "6":
                        totalExp5++;
                        break;
                    case "7":
                        totalExp4++;
                        break;
                    case "8":
                        totalExp5++;
                        break;

                }
            }

            tbMaleTotalEmployee.Text = maleTotalEmployee.ToString();
            tbFemaleTotalEmployee.Text = femaleTotalEmployee.ToString();
            tbTotalSingle.Text = totalSingle.ToString();
            tbTotalMarried.Text = totalMarried.ToString();
            tbAge1.Text = age1.ToString();
            tbAge2.Text = age2.ToString();
            tbAge3.Text = age3.ToString();
            tbAge4.Text = age4.ToString();
            tbEducation1.Text = totalEducation1.ToString();
            tbEducation2.Text = totalEducation2.ToString();
            tbEducation3.Text = totalEducation3.ToString();
            tbEducation4.Text = totalEducation4.ToString();
            tbEducation5.Text = totalEducation5.ToString();
            tbEducation6.Text = totalEducation6.ToString();
            tbEducation7.Text = totalEducation7.ToString();
            tbEducation8.Text = totalEducation8.ToString();
            tbExp1.Text = totalExp1.ToString();
            tbExp2.Text = totalExp2.ToString();
            tbExp3.Text = totalExp3.ToString();
            tbExp4.Text = totalExp4.ToString();
            tbExp5.Text = totalExp5.ToString();
        }

        private async void GetAllEmployeeQuitJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_downsizing;

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var form = new Dictionary<string, string>();
                form.Add("page", "1");
                form.Add("pageSize", "1000000");
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                listAllEmployeeQuitJob = result.data.data;
                BindingDataQuitJob();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingDataQuitJob()
        {
            tbTotalEmployeeDownSizing.Text = listAllEmployeeQuitJob.Count.ToString();
            int totalReduce = 0;
            int quitJob = 0;
            foreach (var item in listAllEmployeeQuitJob)
            {
                switch (item.type)
                {
                    case "1":
                        totalReduce++;
                        break;
                    case "2":
                        quitJob++;
                        break;
                }
            }

            tbReduce.Text = totalReduce.ToString();
            tbQuitJob.Text = quitJob.ToString();
        }

        private async void GetAllEmployeeAppointment()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.appointment_list;

                httpRequestMessage.RequestUri = new Uri(api);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("page", "1");
                form.Add("pageSize", "10000");
                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                listAllEmployeeAppointment = result.data.data;
                BindingDataAppointment();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingDataAppointment()
        {
            tbTotalEmployeeAppointment.Text = listAllEmployeeAppointment.Count.ToString();
            int male = 0;
            int female = 0;
            foreach (var item in listAllEmployeeAppointment)
            {
                switch (item.ep_gender)
                {
                    case "1":
                        male++;
                        break;
                    case "2":
                        female++;
                        break;
                }
            }

            tbTotalMaleAppointment.Text = male.ToString();
            tbTotalFemaleAppointment.Text = female.ToString();
        }

        private async void GetAllEmployeeSalary()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.up_down_salary;

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var form = new Dictionary<string, string>();
                form.Add("link", "bieu-do-danh-sach-nhan-vien-tang-giam-luong.html");
                form.Add("winform", "1");
                form.Add("page", "1");
                form.Add("pageSize", "10000");
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                UpDownSalaryEntity result = JsonConvert.DeserializeObject<UpDownSalaryEntity>(responseContent);
                if (result.data == null) return;
                listAllEmployeeSalary = result.data;
                BindingDataSalary();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingDataSalary()
        {
            tbTotalEmployeeSalary.Text = listAllEmployeeSalary.Count.ToString();
            int salaryUp = 0;
            int salaryDown = 0;
            foreach (var item in listAllEmployeeSalary)
            {
                if (item.sb_tanggiam >= 0)
                {
                    salaryUp++;
                }
                else
                {
                    salaryDown++;
                }
            }

            tbSalaryUp.Text = salaryUp.ToString();
            tbSalaryDown.Text = salaryDown.ToString();
        }

        private async void GetAllEmployeeRotation()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_job_rotation;

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("page", "1");
                headers.Add("pageSize", "10000");
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                listAllEmployeeRotation = result.data.data;
                BindingDataRotation();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingDataRotation()
        {
            tbTotalRotation.Text = listAllEmployeeRotation.Count.ToString();
            int male = 0;
            int female = 0;
            foreach (var item in listAllEmployeeRotation)
            {
                switch (item.ep_gender)
                {
                    case "1":
                        male++;
                        break;
                    case "2":
                        female++;
                        break;
                }
            }

            tbMaleRotation.Text = male.ToString();
            tbFemaleRotation.Text = female.ToString();
        }

        private int GetYearOfBirth(string dateOfBirth)
        {
            try
            {
                DateTime date = DateTime.ParseExact(dateOfBirth, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);
                return date.Year;
            }
            catch
            {
                return -1;
            }
        }


        private void NavigateToPage(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            string name = border.Name;
            switch (name)
            {
                case "NewStaff":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new NewStaffPage());
                        }
                    };
                    break;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Page.ActualWidth < 900)
            {
                Width1 = (Page.ActualWidth - 40) / 2;
                Width2 = Page.ActualWidth - 20;
            }
            else
            {
                Width1 = (Page.ActualWidth - 60) / 3;
                Width2 = (Page.ActualWidth - 40) / 2;
            }
        }

        private void NavigateToRecruitmentReport(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new RecruitmentReport(token, com_id));
                }
            }
        }


        private void SetupChart()
        {
            string dep_id = "";
            if (cbxDepartment.SelectedIndex > -1) dep_id = cbxDepartment.SelectedValue.ToString();
            string time_mode = cbxSelectTime.SelectedValue.ToString();
            string time_start = dp1.SelectedDate.Value.ToString("dd/MM/yyyy");
            string time_end = dp2.SelectedDate.Value.ToString("dd/MM/yyyy");
            grNewStaff.Children.Clear();
            grNewStaff.Children.Add(new NewStaffChart(dep_id, time_mode, time_start, time_end, listAllEmployee));

            grQuitJobChart.Children.Clear();
            grQuitJobChart.Children.Add(new QuitJobChart(dep_id, time_mode, time_start, time_end, listAllEmployeeQuitJob));

            grApointment.Children.Clear();
            grApointment.Children.Add(new AppointmentChart(dep_id, time_mode, time_start, time_end, listAllEmployeeAppointment));

            grUpDownSalary.Children.Clear();
            grUpDownSalary.Children.Add(new UpDownSalaryChart(dep_id, time_mode, time_start, time_end, listAllEmployeeSalary));

            grRotation.Children.Clear();
            grRotation.Children.Add(new RotationChart(dep_id, time_mode, time_start, time_end, listAllEmployeeRotation));

            grMarried.Children.Clear();
            grMarried.Children.Add(new StackColumnChart(dep_id, time_mode, time_start, time_end, listAllEmployee));

            grEducation.Children.Clear();
            grEducation.Children.Add(new AcademiLevelChart(dep_id, time_mode, time_start, time_end, listAllEmployee));

            grAge.Children.Clear();
            grAge.Children.Add(new AgeChart(dep_id, time_mode, time_start, time_end, listAllEmployee));
        }

        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            if (Validate())
                SetupChart();
        }

        private bool Validate()
        {
            if (dp1.SelectedDate == null || dp2.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thời gian");
                return false;
            }
            DateTime date1;
            DateTime date2;
            try
            {
                date1 = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                try
                {
                    date1 = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("MM/dd/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                date2 = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                try
                {
                    date2 = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("MM/dd/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return false;
                }
            }

            string mode = cbxSelectTime.SelectedValue.ToString();
            double space;
            if (mode == "0")
            {
                space = (date2 - date1).TotalDays;
            }
            else if (mode == "1")
            {
                space = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
            }
            else
            {
                space = ((date1.Year - date2.Year) * 12);
            }

            if (space > 60)
            {
                MessageBox.Show("Khoảng cách quá lớn, vui lòng chọn lại hình thức thống kê hoặc chọn lại thời gian!");
                return false;
            }

            return true;
        }

        private void cbxDepartment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxDepartment.SelectedIndex = -1;
            string textSearch = cbxDepartment.Text + e.Text;
            cbxDepartment.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxDepartment.Text = "";
                cbxDepartment.Items.Refresh();
                cbxDepartment.ItemsSource = listAllDepartment;
                cbxDepartment.SelectedIndex = -1;
            }
            else
            {
                cbxDepartment.ItemsSource = "";
                cbxDepartment.Items.Refresh();
                cbxDepartment.ItemsSource = listAllDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxDepartment_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxDepartment.SelectedIndex = -1;
                string textSearch = cbxDepartment.Text;
                cbxDepartment.Items.Refresh();
                cbxDepartment.ItemsSource = listAllDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        /*private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dp1.IsDropDownOpen = true;
        }
        private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            dp2.IsDropDownOpen = true;
        }*/
    }
}
