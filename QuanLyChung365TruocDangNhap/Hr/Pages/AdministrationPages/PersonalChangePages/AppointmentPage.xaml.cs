using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Appointment_Planning;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.PersonalChangePages
{
    /// <summary>
    /// Interaction logic for AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page, INotifyPropertyChanged
    {
        string token;
        public string id;
        public string ep_id;
        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;

        public const int COUNT_RECORD_PER_PAGE = 10;
        public int page_now = 1;
        public int total = 0;
        public int total_page =1;

        int TypeUser;

        bool perAdd, perEdit, perDel, perShow; // quyền thêm, sửa, xóa,xem



        public string PageNow
        {
            get { return page_now.ToString(); }
            set
            {
                page_now = Convert.ToInt32(value);
                OnPropertyChanged("PageNow");
            }
        }

       

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
        public static Dictionary<string, string> listDepartment = new Dictionary<string, string>();
        public AppointmentPage(string token, string id,int TypeUser, bool perAdd, bool perEdit, bool perDel,bool perShow)
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
            GetDatalistEmployee();
            TotalPage.Text = total_page.ToString();
            void1();
            if (PerShow)
            {
                UpDownSalary.Visibility = Visibility.Visible;
            }
            else
            {
                UpDownSalary.Visibility = Visibility.Collapsed;
            }
            DeleteAppointmentPlanning.hidePopup += HidePopup;
            AddNewPoint.hidePopup += HidePopup;
            EditPoint.hidePopup += HidePopup;

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
        List<Items3> listEmployee = new List<Items3>();
        //lấy toàn bộ danh sách nhân viên 
        private async void GetDatalistEmployee()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;


            string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";

            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("pageNumber", page_now.ToString());
            form.Add("pageSize", COUNT_RECORD_PER_PAGE.ToString());
            form.Add("com_id", id.ToString());

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            httpRequestMessage.Content = new FormUrlEncodedContent(form);
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

        public async void void1()
        {
            await GetDatalistDepartment();
            GetData();
        }
        private void ShowCombobox1(object sender, MouseButtonEventArgs e)
        {
            cbxTenPhongBan.IsDropDownOpen = !cbxTenPhongBan.IsDropDownOpen;
        }

        private void NavigateToPage(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string name = textBlock.Name;
            String uri = "PersonalChangePages/" + name + "Page";
            switch (name)
            {
                case "Appointment":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new AppointmentPage(token, id, TypeUser, PerAdd, PerEdit, PerDel,PerShow));
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
        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api;
                string dateFrom = "";
                string dateTo = "";
                Dictionary<string, string> form = new Dictionary<string, string>();
                if (DateGo.SelectedDate != null)
                {
                    dateFrom = DateGo.SelectedDate.Value.ToString("yyyy-MM-dd").ToString();
                }
                if (DateTo.SelectedDate != null)
                {
                    dateTo = DateTo.SelectedDate.Value.ToString("yyyy-MM-dd").ToString();
                }

                api = "http://210.245.108.202:3006/api/hr/personalChange/getListAppoint";
                form.Add("page", page_now.ToString());
                form.Add("pageSize", COUNT_RECORD_PER_PAGE.ToString());
                form.Add("fromDate", dateFrom.ToString());
                form.Add("toDate", dateTo.ToString());
                form.Add("winform", "1");
                if (tbSearch.SelectedValue != null)
                    form.Add("ep_id", tbSearch.SelectedValue.ToString());
                if (cbxTenPhongBan.SelectedIndex > 0)
                {
                    form.Add("update_dep_id", cbxTenPhongBan.SelectedValue.ToString());
                }
                //}

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                PlanningAppointmentList result = JsonConvert.DeserializeObject<PlanningAppointmentList>(responseContent);

                if (result.data != null)
                {
                    total = result.data.totalItems;
                    Paging();
                    /*foreach (var item in result.data.items)
                    {
                        if (listDepartment.ContainsKey(item.old_dep_id))
                        {
                            item.old_dep_name = listDepartment[item.old_dep_id];
                        }



                        item.created_at = ConvertTicksToDate(item.created_at);
                    }*/
                    List<Items2> listPlanningAppointment = result.data.items;
                    icAppointmentPage.Items.Refresh();
                    icAppointmentPage.ItemsSource = listPlanningAppointment;
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }
        }
        private async Task GetDatalistDepartment()
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


                if (result.data == null) return;

                listDepartment = new Dictionary<string, string>();
                listDepartment.Add("", "Tất cả phòng ban");

                foreach (var item in result.data.items)
                {
                    listDepartment.Add(item.dep_id, item.dep_name);
                }
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment;
                return;
            }
            catch (Exception)
            {
                return;
                //MessageBox.Show("Error");
            }
        }

        private void showPopupDelete(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Items2 items2 = grid.DataContext as Items2;
            string ep_id = items2.ep_id;
            string ep_name = items2.ep_name;
            string id_com = items2.com_id;
            DeleteAppointmentPlanning deleteAppointmentPlanning = new DeleteAppointmentPlanning(token, ep_id, ep_name, id_com);
            onShowPopup(deleteAppointmentPlanning);
        }

        private void ShowPopupAddNewAppoint(object sender, MouseButtonEventArgs e)
        {
            AddNewPoint addNew = new AddNewPoint(id, token,TypeUser);
            onShowPopup(addNew);
        }

        private void clickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData();
        }

        private void ShopUpdatePoint(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Items2 items2 = grid.DataContext as Items2;
            string ep_name = items2.ep_name;
            string ep_id = items2.ep_id;
            string old_position_name = items2.old_position_name;
            string old_position_id = items2.old_position_id;
            string old_dep_name = items2.old_dep_name;
            string old_dep_id = items2.old_dep_id;
            string position_id = items2.position_id;
            string update_dep_id = items2.dep_id;
            string created_at = items2.created_at;
            string decision_id = items2.decision_id;
            string note = items2.note;

            EditPoint editPoint = new EditPoint(token, ep_name, ep_id, old_position_id, old_position_name, old_dep_id, old_dep_name, position_id, update_dep_id, created_at, decision_id, note, id, listDepartment);
            onShowPopup(editPoint);
        }

        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
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

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                PageNow = "1";
                GetData();
            }
        }


        private void Paging()
        {
            if (total == 0)
            {
                tbNoData.Visibility = Visibility.Visible;
                scData.Visibility = Visibility.Collapsed;
                total_page = 1;
            }
            else
            {
                tbNoData.Visibility = Visibility.Collapsed;
                scData.Visibility = Visibility.Visible;
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
        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            int page_next = page_now + 1;
            PageNow = page_next.ToString();
            GetData();
        }

        private void PrevPage(object sender, MouseButtonEventArgs e)
        {
            DisableBtnPage();
            int page_prev = page_now - 1;
            PageNow = page_prev.ToString();
            GetData();
        }

        /*private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DateGo.IsDropDownOpen = true;
        }*/
        /*private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            DateTo.IsDropDownOpen = true;
        }*/

        private void scData_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                scData.ScrollToHorizontalOffset(scData.HorizontalOffset - e.Delta);
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                scData.ScrollToHorizontalOffset(scData.HorizontalOffset - e.Delta);
            }
            else
                scData.ScrollToVerticalOffset(scData.VerticalOffset - e.Delta);
        }

        private void DisableBtnPage()
        {
            btnPrev.IsEnabled = false;
            btnPrev.Opacity = 0.3;
            btnNext.IsEnabled = false;
            btnNext.Opacity = 0.3;
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
