using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages
{
    /// <summary>
    /// Interaction logic for CandidateWarehousePage.xaml
    /// </summary>
    public partial class CandidateWarehousePage : Page, INotifyPropertyChanged
    {
        string token;
        string id;
        string type;
        Dictionary<string, string> listRecuitmentNew = new Dictionary<string, string>();
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();

        public const int ROW_PER_PAGE = 5;
        public int page_now = 1;
        public int total = 0;
        public int total_page;

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



        // deligate event show popups
        public delegate void ShowPopup(object obj);

        public static event ShowPopup onShowPopup;

        public CandidateWarehousePage(string token, string id, Dictionary<string, string> listRecuitmentNew, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            DataContext = this;
            this.listRecuitmentNew = this.listRecuitmentNew.Concat(listRecuitmentNew).ToDictionary(k => k.Key, v => v.Value);
            cbxPosition.ItemsSource = this.listRecuitmentNew;
            GetAllEmployee();
            GetListRecruitmentNew();
            GetData();

            DeleteCandidatePopup.hidePopup += HidePopup;
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }


        private async void GetListRecruitmentNew()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_all_new;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.ListNewActive result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.ListNewActive>(responseContent);
                if (result.success == null || result.success.data.data.Count == 0)
                {
                    return;
                }

                listRecuitmentNew.Add("", "Vị trí tuyển dụng");

                foreach (var item in result.success.data.data)
                {
                    listRecuitmentNew.Add(item.id, item.title);
                }
                //cbxRecruitment.ItemsSource = listRecruitmentNew;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        // lấy toàn bộ nhân viên trong công ty
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
                listAllEmployee.Add("", "Tất cả");
                if (result.data == null) return;
                foreach (var item in result.data.items)
                {
                    listAllEmployee.Add(item.ep_id, item.ep_name);
                }

                //cbxUserHiring.ItemsSource = listAllEmployee;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetData(int lenght = 5)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_candidate_depot;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("name", tbSearch.Text));
                string gender = GetIDGender();
                parameters.Add(new KeyValuePair<string, string>("gender", gender));
                if (cbxPosition.SelectedIndex > -1)
                {
                    string new_id = cbxPosition.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", new_id));
                }
                parameters.Add(new KeyValuePair<string, string>("page", page_now.ToString()));
                parameters.Add(new KeyValuePair<string, string>("pageSize", lenght.ToString()));
                if (dp1.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("MM/dd/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("MM/dd/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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

                ListCandidateEntity result = JsonConvert.DeserializeObject<ListCandidateEntity>(responseContent);

                if (result.result && result.success.data != null)
                {
                    List<DataEntity> listRecuitment = result.success.data.data;
                    total = result.success.data.total;
                    Paging();
                    foreach (var item in listRecuitment)
                    {
                        item.time_send_cv = ConvertDate(item.time_send_cv, "HH:mm dd/MM/yyyy");
                        if (item.cv == null || item.cv == "")
                        {
                            item.isCv = false;
                        }
                        else
                        {
                            item.isCv = true;
                        }

                        if (item.time_send_cv == null || item.time_send_cv == "")
                        {
                            item.isTime = false;
                        }
                        else
                        {
                            item.isTime = true;
                        }
                        if(item.recruitment_news_id!=null)
                        if (listRecuitmentNew.ContainsKey(item.recruitment_news_id))
                        {
                            item.recruitment_news_name = "TD" + item.recruitment_news_id + "(" + listRecuitmentNew[item.recruitment_news_id] + ")";
                        }
                        if(item.user_hiring!=null)
                        if (listAllEmployee.ContainsKey(item.user_hiring))
                        {
                            item.hr_name = listAllEmployee[item.user_hiring];
                        }
                        item.id_process_interview = "0";
                    }
                    icListCandidate.Items.Refresh();
                    icListCandidate.ItemsSource = listRecuitment;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        myDate = DateTime.ParseExact(date, "yyyy/MM/dd",
                                                      System.Globalization.CultureInfo.InvariantCulture);
                        return myDate.ToString(format);
                    }
                    catch
                    {
                        try
                        {
                            myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                          System.Globalization.CultureInfo.InvariantCulture);
                            return myDate.ToString(format);
                        }
                        catch
                        {
                            try
                            {
                                myDate = DateTime.ParseExact(date, "MM/dd/yyyy",
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
            }

        }

        private string GetIDGender()
        {
            switch (cbxGender.Text)
            {
                case "Nam":
                    return "1";
                case "Nữ":
                    return "2";
                case "Khác":
                    return "3";
                default:
                    return "";
            }
        }

        private void HidePopup(int mode, int id_process)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }

        private void NavigateToDetail(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity dataEntity = (DataEntity)textBlock.DataContext;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateProfileDetailsPage());
                }
            }
        }

        private void NavigateToListCandidate(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new ListOfCandidatesPage(token, id, type, PerAdd, PerEdit, PerDel));
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

        private void ViewCVCandidate(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string url_cv = APIs.API.api_cv + ((DataEntity)textBlock.DataContext).cv;
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url_cv,
                UseShellExecute = true
            });
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
                if (total % ROW_PER_PAGE == 0)
                {
                    total_page = total / ROW_PER_PAGE; 
                    TotalPage.Text = total_page.ToString();
                }
                else
                {
                    total_page = total / ROW_PER_PAGE + 1;
                    TotalPage.Text = total_page.ToString();
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

        private void SearchClick(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData();
        }

        private void ClickDelete(object sender, MouseButtonEventArgs e)
        {
            DataEntity dataEntity = (DataEntity)popupOption.DataContext;
            string id_candidate = dataEntity.id;
            string id_process = dataEntity.id_process_interview;
            string name = dataEntity.name;
            DeleteCandidatePopup deleteCandidatePopup = new DeleteCandidatePopup(token, id_candidate, name, id_process);
            onShowPopup(deleteCandidatePopup);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void page_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Children[0].Visibility = Visibility.Visible;
            TextBlock textBlock = (TextBlock)grid.Children[1];
            var converter = new BrushConverter();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#4C5BD4");
        }

        private void cbxPosition_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxPosition.SelectedIndex = -1;
            string textSearch = cbxPosition.Text + e.Text;
            cbxPosition.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxPosition.Text = "";
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = listRecuitmentNew;
                cbxPosition.SelectedIndex = -1;
            }
            else
            {
                cbxPosition.ItemsSource = "";
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = listRecuitmentNew.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxPosition_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxPosition.SelectedIndex = -1;
                string textSearch = cbxPosition.Text;
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = listRecuitmentNew.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void NavigateToCandidateDetail(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetail(id);
        }

        private void NavigateToCandidateDetail(string id)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailPage(token, id, listAllEmployee, listRecuitmentNew, PerEdit));
                }
            }
        }

        private void NavigateToCandidateDetailGetJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailGetJob(id);
        }

        private void NavigateToCandidateDetailGetJob(string id)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailGetJobPage(token, id, listAllEmployee, listRecuitmentNew, PerEdit,null));
                }
            }
        }

        private void NavigateToCandidateDetailFailJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailFailJob(id);
        }

        private void NavigateToCandidateDetailFailJob(string id)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailFailJobPage(token, id, listAllEmployee, listRecuitmentNew, PerEdit,null));
                }
            }
        }

        private void NavigateToCandidateDetailCancelJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailCancelJob(id);
        }

        private void NavigateToCandidateDetailCancelJob(string id)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailCancelJobPage(token, id, listAllEmployee, listRecuitmentNew, PerEdit,null));
                }
            }
        }

        private void NavigateToCandidateDetailContractJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailContractJob(id);
        }

        private void NavigateToCandidateDetailContractJob(string id)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailContractJobPage(token, id, listAllEmployee, listRecuitmentNew, PerEdit,null));
                }
            }
        }

        private void NavigateToCandidateProcessDetail(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity dataEntity = (DataEntity)textBlock.DataContext;
            string id = dataEntity.id;
            string process_id = dataEntity.id_process_interview;
            string name_process = dataEntity.name;
            NavigateToCandidateProcessDetail(id, process_id, name_process);
        }

        private void NavigateToCandidateProcessDetail(string id, string process_id, string name_process)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailProcessInterviewPage(token, id, listAllEmployee, process_id, name_process, listRecuitmentNew, PerEdit,null));
                }
            }
        }

        private void ClickViewDetail(object sender, MouseButtonEventArgs e)
        {
            DataEntity dataEntity = (DataEntity)popupOption.DataContext;
            string id_process = dataEntity.id_process_interview;
            string id_candidate = dataEntity.id;
            string name_process = dataEntity.name;
            //string id_can = dataEntity.id
            switch (id_process)
            {
                case "0":
                    NavigateToCandidateDetail(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                case "1":
                    NavigateToCandidateDetailGetJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;

                case "-2":
                    NavigateToCandidateDetailFailJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                case "-3":
                    NavigateToCandidateDetailCancelJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                case "-4":
                    NavigateToCandidateDetailContractJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                default:
                    NavigateToCandidateProcessDetail(id_candidate, id_process, name_process);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        // item combobox4
        public class Items
        {
            public string ID { get; set; }
            public string Name { get; set; }
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
                double y = p.Y - 30;
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
        private void select_pagani(object sender, SelectionChangedEventArgs e)
        {
            GetData((int)cbxPage.SelectedItem);
        }

        /*private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dp1.IsDropDownOpen = true;

        }
        private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            dp2.IsDropDownOpen = true;
        }*/

        private void scData_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
                scData.ScrollToVerticalOffset(scData.VerticalOffset - e.Delta);
        }

        private List<int> _Records = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        public List<int> Records
        {
            get { return _Records; }
        }
    }
}
