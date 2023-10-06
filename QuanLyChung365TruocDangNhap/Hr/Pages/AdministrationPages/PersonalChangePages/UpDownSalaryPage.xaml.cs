using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
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
    /// Interaction logic for UpDownSalaryPage.xaml
    /// </summary>
    public partial class UpDownSalaryPage : Page, INotifyPropertyChanged
    {
        public int totalItems;
        const int COUNT_RECORD_PER_PAGE = 20;
        int page_now = 1;
        int total_page = 1;
        int total;
        string token;
        string id;
        int TypeUser; // 1:nhân viên, 2:công ty

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
        public Dictionary<string, string> getListEmployee = new Dictionary<string, string>();
        public Dictionary<string, string> listDepartment = new Dictionary<string, string>(); // danh sách phòng ban
        public Dictionary<string, string> listEpIDDepID = new Dictionary<string, string>(); // dict id nhân viên - phòng ban
        public Dictionary<string, string> listEpIDPositionId = new Dictionary<string, string>(); // dict id nhân viên - chức vụ
        public UpDownSalaryPage(string token, string id,int TypeUser, bool perAdd, bool perEdit, bool perDel, bool perShow)
        {
            InitializeComponent();
            TotalPage.Text = total_page.ToString();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            this.PerShow = perShow;
            DataContext = this;
            if (!PerShow)
            {
                UpDownSalary.Visibility = Visibility.Collapsed;
            }
            if (PerShow)
            {
                UpDownSalary.Visibility = Visibility.Visible;
            }

            if(id == "5719")
            {
                UpDownSalary.Visibility = Visibility.Visible;
            }
            void1();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
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
                            (window as HomeView).MainContent.Navigate(new WorkingRotationPage(token, id, TypeUser,PerAdd, PerEdit, PerDel, PerShow));
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
        public void void1()
        {
            GetDataListEmployee();
        }

        private async void GetDataListEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.getlistEmplouyee;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);


                if (result.data == null) return;
                getListEmployee.Add("", "Chọn nhân viên");
                foreach (var item in result.data.items)
                {
                    //string dep_name = " (Chưa cập nhật)";
                    //if (item.dep_name != null && item.dep_name != "")
                    //{
                    //    dep_name = " (" + item.dep_name + ")";
                    //}
                    //item.ep_name = item.ep_name + dep_name;
                    getListEmployee.Add(item.ep_id, item.ep_name);
                    listEpIDDepID.Add(item.ep_id, item.dep_name);
                    listEpIDPositionId.Add(item.ep_id, item.position_name);
                }
                cbx1.ItemsSource = getListEmployee;
                GetData();
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
                string date1 = "";
                string date2 = "";
                if (dp1.Text != "")
                {
                    date1 = Convert.ToDateTime(dp1.SelectedDate).ToString("yyyy-MM-dd");
                }
                if (dp2.Text != "")
                {
                    date2 = Convert.ToDateTime(dp2.SelectedDate).ToString("yyyy-MM-dd");
                }
                string ep_id = "";
                if (cbx1.SelectedIndex > -1)
                {
                    ep_id = cbx1.SelectedValue.ToString();
                }
                string api = "http://210.245.108.202:3006/api/hr/report/reportDetail";

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new Dictionary<string, string>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add("link", "bieu-do-danh-sach-nhan-vien-tang-giam-luong.html");
                parameters.Add("winform", "1");
                parameters.Add("page", PageNow);
                parameters.Add("pageSize", COUNT_RECORD_PER_PAGE.ToString());
                parameters.Add("from_date", date1);
                parameters.Add("to_date", date2);
                parameters.Add("idnv", ep_id);
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                UpDownSalaryEntity result = JsonConvert.DeserializeObject<UpDownSalaryEntity>(responseContent);

                if (result.data != null)
                {
                    total = result.totalRecord;
                    Paging();
                    List<Data3> listUpDownSalaryEntity = result.data;
                    foreach (var item in listUpDownSalaryEntity)
                    {
                        if (getListEmployee.ContainsKey(item.sb_id_user))
                        {
                            item.sb_ep_name = getListEmployee[item.sb_id_user];
                        }
                        else
                        {
                            item.sb_ep_name = "Chưa cập nhật";
                        }

                        /*if (listEpIDDepID.ContainsKey(item.sb_id_user))
                        {
                            if (listEpIDDepID[item.sb_id_user] != null)
                            {
                                item.sb_dep_name = listEpIDDepID[item.sb_id_user];
                            }
                            else
                            {
                                item.sb_dep_name = "Chưa cập nhật";
                            }
                        }
                        else
                        {
                            item.sb_dep_name = "Chưa cập nhật";
                        }*/
                        if (string.IsNullOrEmpty(item.position_name)) item.position_name = "Chưa cập nhật";
                        if (string.IsNullOrEmpty(item.sb_dep_name)) item.sb_dep_name = "Chưa cập nhật";
                        /*if (listEpIDPositionId.ContainsKey(item.sb_id_user))
                        {
                            item.position_name = listEpIDPositionId[item.sb_id_user];
                        }
                        else
                        {
                            item.position_name = "Chưa cập nhật";
                        }*/

                        if (item.sb_tanggiam > 0)
                        {
                            item.salary_up = item.sb_tanggiam;
                            item.salary_down = 0;
                        }
                        else
                        {
                            item.salary_down = -item.sb_tanggiam;
                            item.salary_up = 0;
                        }

                        if (item.sb_quyetdinh == "")
                        {
                            item.sb_quyetdinh = "Chưa cập nhật";
                        }
                        //item.sb_time_up = ConvertDate(item.sb_time_up);
                    }
                    icUpDownSalaryPage.Items.Refresh();
                    icUpDownSalaryPage.ItemsSource = listUpDownSalaryEntity;
                }

            }
            catch (Exception)
            {

                //MessageBox.Show("error");
            }

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

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
            }
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


        private string ConvertDate(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString("dd/MM/yyyy");
                }
                catch
                {
                    try
                    {
                        myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                      System.Globalization.CultureInfo.InvariantCulture);
                        return myDate.ToString("dd/MM/yyyy");
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
        }

        private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            dp2.IsDropDownOpen = true;
        }
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dp1.IsDropDownOpen = true;
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
            else scData.ScrollToVerticalOffset(scData.VerticalOffset - e.Delta);
        }

        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData();
        }

        private void cbx1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbx1.SelectedIndex = -1;
                string textSearch = cbx1.Text;
                cbx1.Items.Refresh();
                cbx1.ItemsSource = getListEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbx1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbx1.SelectedIndex = -1;
            string textSearch = cbx1.Text + e.Text;
            cbx1.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbx1.Text = "";
                cbx1.Items.Refresh();
                cbx1.ItemsSource = getListEmployee;
                cbx1.SelectedIndex = -1;
            }
            else
            {
                cbx1.ItemsSource = "";
                cbx1.Items.Refresh();
                cbx1.ItemsSource = getListEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
