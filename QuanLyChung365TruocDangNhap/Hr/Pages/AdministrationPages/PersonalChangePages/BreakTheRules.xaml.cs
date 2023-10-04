using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.BreakTheRules;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.PersonalChangePages
{
    /// <summary>
    /// Interaction logic for BreakTheRules.xaml
    /// </summary>
    public partial class BreakTheRules : Page, INotifyPropertyChanged
    {
        public string token;
        public int totalItems;
        string id;
        const int COUNT_RECORD_PER_PAGE = 10;
        int page_now = 1;
        int total_page = 1;
        int total;
        int TypeUser;

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;
        Dictionary<string, string> listDepartment = new Dictionary<string, string>();

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

        public BreakTheRules(string token, string id, int TypeUser, bool perAdd, bool perEdit, bool perDel, bool perShow)
        {
            InitializeComponent();
            TotalPgae.Text = total_page.ToString();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            this.PerShow = perShow;
            DataContext = this;
            GetDatalistEmployee();
            GetData();
            GetDatalistDepartment();
            if (PerShow)
            {
                UpDownSalary.Visibility = Visibility.Visible;
            }
            else
            {
                UpDownSalary.Visibility = Visibility.Collapsed;
            }

            AddNewPopup.hidePopup += HidePopup;
            DeletePopup.hidePopup += HidePopup;
            UpdatePopup.hidePopup += HidePopup;

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }

        //Chuyển page

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
                            (window as HomeView).MainContent.Navigate(new AppointmentPage(token, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
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

        List<Items3> listEmployee = new List<Items3>();

        private async void GetDatalistEmployee()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;


            string api = "http://210.245.108.202:3006/api/hr/personalChange/getEmResign";



            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
            try
            {
                listEmployee = result.data.items;
                foreach (var item in listEmployee)
                {
                    string dep_name = " (Chưa cập nhật)";
                    if (item.dep_name != null && item.dep_name != "")
                    {
                        dep_name = " (" + item.dep_name + " - ID: " + item.ep_id + ")";
                    }
                    item.ep_name = item.ep_name + dep_name;
                }
                tbSearch.ItemsSource = listEmployee;
            }
            catch
            {

            }

        }

        private void cbxTenNV_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tbSearch.SelectedIndex = -1;
            string textSearch = tbSearch.Text + e.Text;
            tbSearch.IsDropDownOpen = true;
            if (textSearch == "")
            {
                tbSearch.Text = "";
                tbSearch.Items.Refresh();
                tbSearch.ItemsSource = listEmployee;
                tbSearch.SelectedIndex = -1;
            }
            else
            {
                tbSearch.ItemsSource = "";
                tbSearch.Items.Refresh();
                tbSearch.ItemsSource = listEmployee.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenNV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                tbSearch.SelectedIndex = -1;
                string textSearch = tbSearch.Text;
                tbSearch.Items.Refresh();
                tbSearch.ItemsSource = listEmployee.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        //lấy dữ liệu nghỉ việc sai quy định
        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "";
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

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListQuitJobNew";
                var form = new Dictionary<string, string>();
                form.Add("page", page_now.ToString());
                form.Add("pageSize", COUNT_RECORD_PER_PAGE.ToString());
                form.Add("winform", "1");
                form.Add("fromDate", date1);
                form.Add("toDate", date2);
                if(tbSearch.SelectedValue != null)
                    form.Add("ep_id", tbSearch.SelectedValue.ToString());
                if (cbxTenPhongBan.SelectedValue != null)
                    form.Add("current_dep_id", cbxTenPhongBan.SelectedValue.ToString());
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                DownsizingaEntity result = JsonConvert.DeserializeObject<DownsizingaEntity>(responseContent);

                if (result.data != null)
                {
                    total = result.data.totalItems == 0 ? result.data.totalCount: result.data.totalItems;
                    List<Items1> listDownsizinga = result.data.items == null? result.data.data : result.data.items;
                    Paging();
                    foreach (var item in listDownsizinga)
                    {
                        item.created_at = ConvertTicksToDate(item.created_at);
                        if (item.dep_name == null)
                        {
                            item.dep_name = "Chưa cập nhập";
                        }
                    }
                    icDownsizingPage.ItemsSource = listDownsizinga;
                    icDownsizingPage.Items.Refresh();
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }
        }

        //Định dạng tick sang date
        private string ConvertTicksToDate(string ticks)
        {
            try
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(ticks)).ToLocalTime();
                return dtDateTime.ToString("dd/MM/yyyy");
            }
            catch
            {
                return "";
            }
        }
        //lấy toàn bộ danh sách phòng ban
        private async void GetDatalistDepartment()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listDepartment;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listDepartment result = JsonConvert.DeserializeObject<listDepartment>(responseContent);
                try
                {
                    listDepartment = new Dictionary<string, string>();
                    listDepartment.Add("", "Tất cả phòng ban");
                    foreach (var item in result.data.items)
                    {
                        listDepartment.Add(item.dep_id, item.dep_name);
                    }
                    cbxTenPhongBan.ItemsSource = listDepartment;
                }
                catch
                {

                }
            }
            catch
            {

            }

        }
        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData();
        }

        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            DisableBtn();
            int page_next = page_now + 1;
            PageNow = page_next.ToString();
            GetData();
        }

        private void PrevPage(object sender, MouseButtonEventArgs e)
        {
            DisableBtn();
            int page_prev = page_now - 1;
            PageNow = page_prev.ToString();
            GetData();
        }

        private void DisableBtn()
        {
            btnPrev.IsEnabled = false;
            btnPrev.Opacity = 0.3;
            btnNext.IsEnabled = false;
            btnNext.Opacity = 0.3;
        }

        private void IsSetValidBtn()
        {
            if (page_now == 1)
            {
                btnPrev.IsEnabled = false;
                btnPrev.Opacity = 0.3;
            }
            else
            {
                btnPrev.IsEnabled = true;
                btnPrev.Opacity = 1;
            }

            if (page_now == total_page)
            {
                btnNext.IsEnabled = false;
                btnNext.Opacity = 0.3;
            }
            else
            {
                btnNext.IsEnabled = true;
                btnNext.Opacity = 1;
            }
        }

        private void Paging()
        {
            if (total == 0)
            {
                txtNoData.Visibility = Visibility.Visible;
                total_page = 1;
            }
            else
            {
                txtNoData.Visibility = Visibility.Collapsed;
                if (total % COUNT_RECORD_PER_PAGE == 0)
                {
                    total_page = total / COUNT_RECORD_PER_PAGE;
                    TotalPgae.Text = total_page.ToString();
                }
                else
                {
                    total_page = total / COUNT_RECORD_PER_PAGE + 1;
                    TotalPgae.Text = total_page.ToString();
                }
            }

            IsSetValidBtn();
        }
        public string PageNow
        {
            get { return page_now.ToString(); }
            set
            {
                page_now = Convert.ToInt32(value);
                OnPropertyChanged("PageNow");
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

        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        private void ShowCombobox1(object sender, MouseButtonEventArgs e)
        {

        }

        private void ShowPopupAddNew(object sender, MouseButtonEventArgs e)
        {
            AddNewPopup addNew = new AddNewPopup(token,id,TypeUser);
            onShowPopup(addNew);
        }

        private void ShowPopupDelete(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Items1 items1 = grid.DataContext as Items1;
            string ep_id = items1.ep_id;
            string id_com = items1.com_id;
            DeletePopup deleteAppointmentPlanning = new DeletePopup(token, ep_id, id_com);
            onShowPopup(deleteAppointmentPlanning);
        }

        private void ShowPopupUpdate(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Items1 items1 = grid.DataContext as Items1;
            string ep_id = items1.ep_id;
            string ep_name = items1.ep_name;
            string dep_name = items1.dep_name;
            string dep_id = items1.dep_id;
            string position_name = items1.position_name;
            string position_id = items1.position_id;
            string created_at = items1.create_time;
            string note = items1.note;
            string id_com = items1.com_id;
            UpdatePopup deleteAppointmentPlanning = new UpdatePopup(token, id_com,ep_id, ep_name,dep_name,position_name,created_at,note, dep_id, position_id);
            onShowPopup(deleteAppointmentPlanning);
        }

        private DateTime ConvertDate(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate;
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate;
                }
                catch
                {
                    return new DateTime();
                }
            }
        }
        private void cbxTenPhongBan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenPhongBan.SelectedIndex = -1;
                string textSearch = cbxTenPhongBan.Text;
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void scroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
                scData.ScrollToVerticalOffset(scData.VerticalOffset - e.Delta);
        }

        private void cbxTenPhongBan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenPhongBan.SelectedIndex = -1;
            string textSearch = cbxTenPhongBan.Text + e.Text;
            cbxTenPhongBan.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenPhongBan.Text = "";
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment;
                cbxTenPhongBan.SelectedIndex = -1;
            }
            else
            {
                cbxTenPhongBan.ItemsSource = "";
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
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
