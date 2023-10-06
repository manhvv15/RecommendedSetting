using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.PerformRecuitmentEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.PerformRecruitmentPopup;
using QuanLyChung365TruocDangNhap.Hr.View;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.PerformRecruitmentPages
{
    /// <summary>
    /// Interaction logic for RecruitmentInformationPage.xaml
    /// </summary>
    public partial class RecruitmentInformationPage : Page, INotifyPropertyChanged
    {
        string token;
        string id;
        public const int ROW_PER_PAGE = 5;
        public int page_now = 1;
        public int total;
        public int total_page;
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();

        private bool show;

        public bool Show
        {
            get { return show; }
            set
            {
                show = value;
                OnPropertyChanged("Show");
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
        bool perAdd, perEdit, perDel; // quyền thêm, sửa, xóa

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;

        public RecruitmentInformationPage()
        {
            InitializeComponent();
        }


        public RecruitmentInformationPage(string token, string id, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            GetAllEmployee();
            GetData();
            DataContext = this;
            RemoveJobPostingPopup.hidePopup += HidePopup;
            SetupTemplatePopup.hidePopup += HidePopup;
            MoreNewsPopup.hidePopup += HidePopup;
            EditMessagePopup.hidePopup += HidePopup;
        }

        private void GetData()
        {
            GetListNews();
        }

        private async void GetListNews()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_news;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("page", page_now.ToString()));
                parameters.Add(new KeyValuePair<string, string>("pageSize", ROW_PER_PAGE.ToString()));
                parameters.Add(new KeyValuePair<string, string>("title", tbSearch.Text));
                if (dp1.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("M/d/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("M/d/yyyy"), "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string toDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("toDate", toDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("toDate", ""));
                }

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListNewActive result = JsonConvert.DeserializeObject<ListNewActive>(responseContent);
                if (result.result)
                {
                    total = result.success.data.total;
                    Paging();
                    if (result.success.data.data.Count > 0)
                    {
                        foreach (var item in result.success.data.data)
                        {

                            item.recruitment_time = ConvertDate(item.recruitment_time, "dd/MM/yyyy");
                            item.recruitment_time_to = ConvertDate(item.recruitment_time_to, "dd/MM/yyyy");
                            item.sohoso = Decimal(item.sohoso);
                            item.address += ". " + ListItemCombobox.ListProvince.FirstOrDefault(t => t.ID == item.cit_id).value;
                            if (listAllEmployee.ContainsKey(item.hr_name))
                            {
                                item.hr_name_full = listAllEmployee[item.hr_name];
                            }
                            else
                            {
                                item.hr_name_full = "Chưa cập nhật";
                            }
                        }
                    }
                    icListNews.Items.Refresh();
                    icListNews.ItemsSource = result.success.data.data;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private string ConvertDate(string date, string format)
        {
            try
            {
                DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    DateTime myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        DateTime myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        return myDate.ToString(format);
                    }
                    catch
                    {
                        return "";
                    }
                }

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
                form.Add("com_id", id);
                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                foreach (var item in result.data.items)
                {
                    listAllEmployee.Add(item.ep_id, item.ep_name);
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private string Decimal(string num)
        {
            int n;
            bool isNumeric = int.TryParse(num, out n);
            if (isNumeric)
            {
                if (n > 0 && n < 10)
                {
                    return "0" + n.ToString();
                }
                else
                {
                    return n.ToString();
                }
            }
            return "0";
        }

        private void navigateToPage(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            switch (name)
            {
                case "PerformRecruitment":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new PerformRecruitmentPage(token, id, PerAdd, PerEdit, PerDel));
                        }
                    };
                    break;
                case "RecruitmentInformation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentInformationPage(token, id, PerAdd, PerEdit, PerDel));
                        }
                    };
                    break;
                case "RecruitmentConnection":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentConnectionPage());
                        }
                    };
                    break;
                case "RecruitmentFee":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentFeePage());
                        }
                    };
                    break;
            }
        }

        private void navigateToViewDetailItem(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            string hr_name = ((DataEntity)textBlock.DataContext).hr_name_full;
            Navigate(id, hr_name);

        }

        private void Navigate(string id, string hr_name)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new RecruitmentInformationDetailPage(token, id, hr_name));
                }
            }
        }

        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            DisableBtnPage();
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

        private void DisableBtnPage()
        {
            btnPrev.IsEnabled = false;
            btnPrev.Opacity = 0.3;
            btnNext.IsEnabled = false;
            btnNext.Opacity = 0.3;
        }
        private void Paging()
        {
            if (total == 0)
            {
                tbNoData.Visibility = Visibility.Visible;
                total_page = 1;
            }
            else
            {
                tbNoData.Visibility = Visibility.Collapsed;
                if (total % ROW_PER_PAGE == 0)
                {
                    total_page = total / ROW_PER_PAGE;
                    totalPage.Text = total_page.ToString();
                }
                else
                {
                    total_page = total / ROW_PER_PAGE + 1;
                    totalPage.Text = total_page.ToString();
                }
            }

            IsSetValidBtn();
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

        private void page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (page.ActualWidth < 900)
            {
                Show = false;
            }
            else
            {
                Show = true;
            }
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PageNow = "1";
                GetData();
            }
        }

        private void SearchClick(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Children[0].Visibility = Visibility.Visible;
            TextBlock textBlock = (TextBlock)grid.Children[1];
            var converter = new BrushConverter();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#4C5BD4");
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Children[0].Visibility = Visibility.Collapsed;
            TextBlock textBlock = (TextBlock)grid.Children[1];
            var converter = new BrushConverter();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#474747");
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (popupOption.Visibility == Visibility.Visible)
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
            else
            {
                Border border = sender as Border;
                Point p = e.GetPosition(this);
                double x = p.X;
                double y = p.Y - 60;
                if (x + popupOption.Width > page.ActualWidth)
                {
                    x = page.ActualWidth - popupOption.Width - 10;
                }
                Thickness thickness = new Thickness(x, y, 0, 0);
                popupOption.Margin = thickness;
                popupOption.Visibility = Visibility.Visible;
                popupOption.DataContext = border.DataContext;
            }
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            double x = p.X;
            double y = p.Y;
            double left = popupOption.Margin.Left;
            double top = popupOption.Margin.Top;
            if ((x < left || x > (left + popupOption.ActualWidth)) && (y < top || y > top + popupOption.ActualHeight))
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
        }

        private void ClickViewDetail(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string id = ((DataEntity)grid.DataContext).id;
            string hr_name = ((DataEntity)grid.DataContext).hr_name_full;
            Navigate(id, hr_name);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetListNews();
            }
            onShowPopup("");
        }

        private void ClickDeleteNews(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string id = ((DataEntity)grid.DataContext).id;
            RemoveJobPostingPopup removeJobPostingPopup = new RemoveJobPostingPopup(token, id);
            onShowPopup(removeJobPostingPopup);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ClickSetSampleNew(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string id = ((DataEntity)grid.DataContext).id;
            SetupTemplatePopup setupTemplatePopup = new SetupTemplatePopup(token, id);
            onShowPopup(setupTemplatePopup);
            popupOption.Visibility = Visibility.Collapsed;
        }

        /*private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dp1.IsDropDownOpen = true;

        }

        private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            dp2.IsDropDownOpen = true;
        }*/

        private void ShowPopupAddNew(object sender, MouseButtonEventArgs e)
        {
            MoreNewsPopup moreNewsPopup = new MoreNewsPopup(token, listAllEmployee);
            onShowPopup(moreNewsPopup);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ClickEditNew(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string id = ((DataEntity)grid.DataContext).id;
            EditMessagePopup messagePopup = new EditMessagePopup(token, id, listAllEmployee);
            onShowPopup(messagePopup);
            popupOption.Visibility = Visibility.Collapsed;
        }
    }
}
