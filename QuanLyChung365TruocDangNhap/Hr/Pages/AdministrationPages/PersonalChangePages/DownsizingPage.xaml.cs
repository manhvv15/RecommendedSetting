using QuanLyChung365TruocDangNhap.Hr.Core;
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Downzing;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.PayRoll;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.CodeDom;
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
    /// Interaction logic for DownsizingPage.xaml
    /// </summary>
    public partial class DownsizingPage : Page, INotifyPropertyChanged
    {
        public string Authorization;
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
        public Dictionary<string, string> listShift = new Dictionary<string, string>();


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
        public DownsizingPage(string Authorization, string id,int TypeUser, bool perAdd, bool perEdit, bool perDel, bool perShow)
        {
            InitializeComponent();
            this.Authorization = Authorization;
            this.id = id;
            this.TypeUser = TypeUser;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            this.PerShow = perShow;
            DataContext = this;
            TotalPage.Text = total_page.ToString();

            GetData();
            GetDatalistEmployee();
            GetDatalistDepartment();
            GetDataListShift();
            if (PerShow)
            {
                UpDownSalary.Visibility = Visibility.Visible;
            }
            else
            {
                UpDownSalary.Visibility = Visibility.Collapsed;
            }
            DeleteDown.hidePopup += HidePopup;
            AddNewDown.hidePopup += HidePopup;
            UpdateDown.hidePopup += HidePopup;

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
        private void ShowCombobox1(object sender, MouseButtonEventArgs e)
        {
            cbxTenPhongBan.IsDropDownOpen = !cbxTenPhongBan.IsDropDownOpen;
        }

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
                            (window as HomeView).MainContent.Navigate(new AppointmentPage(Authorization, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "Downsizing":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new DownsizingPage(Authorization, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "UpDownSalary":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new UpDownSalaryPage(Authorization, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "WorkingRotation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new WorkingRotationPage(Authorization, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "BreakTheRules":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new BreakTheRules(Authorization, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
                case "Chart":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ChartPage(Authorization, id, TypeUser, PerAdd, PerEdit, PerDel, PerShow));
                        }
                    };
                    break;
            }
        }
        List<Items3> listEmployee = new List<Items3>();
        private async void GetDatalistEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;

                string api = "http://210.245.108.202:3006/api/hr/personalChange/getEmResign";
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + Authorization);

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
                    tbSearch.Items.Refresh();
                    tbSearch.ItemsSource = listEmployee;
                }
                catch
                {

                }
            }
            catch { }

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
        private async void GetDataListShift()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.list_shift + id;
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + Authorization);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listShift result = JsonConvert.DeserializeObject<listShift>(responseContent);

                if (result.data == null) return;
                foreach (var item in result.data.items)
                {
                    listShift.Add(item.shift_id, item.shift_name);
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("Error");
            }
        }
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

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListQuitJob";
                var form = new Dictionary<string, string>();
                form.Add("page", page_now.ToString());
                form.Add("pageSize", COUNT_RECORD_PER_PAGE.ToString());
                form.Add("fromDate", date1);
                form.Add("toDate", date2);
                if(tbSearch.SelectedValue != null)
                    form.Add("ep_id", tbSearch.SelectedValue.ToString());
                form.Add("winform", "1");
                if(cbxTenPhongBan.SelectedIndex > 0) 
                {
                    form.Add("current_dep_id", cbxTenPhongBan.SelectedValue.ToString());
                }
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + Authorization);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                DownsizingaEntity result = JsonConvert.DeserializeObject<DownsizingaEntity>(responseContent);

                if (result.data != null)
                {
                    total = result.data.totalItems;
                    List<Items1> listDownsizinga = result.data.items;
                    Paging();
                    foreach (var item in listDownsizinga)
                    {
                        item.created_at = ConvertTicksToDate(item.created_at);

                        if (listShift.ContainsKey(item.shift_id))
                        {
                            item.shift_name = listShift[item.shift_id];
                        }
                        else
                        {
                            item.shift_name = "Chưa cập nhập";
                        }

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

        private void ShowPopupDelete(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Items1 items1 = grid.DataContext as Items1;

            string ep_id = items1.ep_id;

            DeleteDown deleteDown = new DeleteDown(Authorization, ep_id);
            onShowPopup(deleteDown);
        }

        private void ShowPopupAddNeww(object sender, MouseButtonEventArgs e)
        {
            AddNewDown addNewDown = new AddNewDown(Authorization, id, TypeUser);
            onShowPopup(addNewDown);
        }

        private void ShowPopupUpdate(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Items1 items1 = grid.DataContext as Items1;
            string ep_id = items1.ep_id;
            string ep_name = items1.ep_name;
            string position_id = items1.position_id;
            string position_name = items1.position_name;
            string created_at = items1.created_at;
            string com_id = items1.com_id;
            string com_name = items1.com_name;
            string dep_id = items1.dep_id;
            string dep_name = items1.dep_name;
            string decision_id = items1.decision_id;
            string note = items1.note;
            string type = items1.type;
            string shift_id = items1.shift_id;


            UpdateDown updateDown = new UpdateDown(Authorization, id, ep_id, ep_name, position_id, position_name, created_at, com_id, com_name, dep_id, dep_name, decision_id, note, type, shift_id, listShift, items1);
            onShowPopup(updateDown);
        }


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
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + Authorization);

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
            catch { }

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
                    TotalPage.Text = total_page.ToString();
                }
                else
                {
                    total_page = total / COUNT_RECORD_PER_PAGE + 1;
                    TotalPage.Text = total_page.ToString();
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

        /*private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dp1.IsDropDownOpen = true;
        }
        private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            dp2.IsDropDownOpen = true;
        }*/

        private void DownsizingPage_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
            {
                scData.ScrollToVerticalOffset(scData.VerticalOffset - e.Delta);
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
                scData.ScrollToVerticalOffset(scData.VerticalOffset - e.Delta);
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
    }
}
