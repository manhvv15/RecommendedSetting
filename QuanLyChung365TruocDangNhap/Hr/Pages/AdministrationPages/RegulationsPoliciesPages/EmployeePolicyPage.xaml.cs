using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.EmployeePolicy;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.RegulationsPoliciesPages
{
    /// <summary>
    /// Interaction logic for EmployeePolicyPage.xaml
    /// </summary>
    public partial class EmployeePolicyPage : Page, INotifyPropertyChanged
    {
        const int COUNT_RECORD_PER_PAGE = 6;
        int page_now = 1;
        int total;
        int total_page = 1;
        int TypeUser;
        public string token;
        List<DataEntity1> listEmployeePolicy;
        Dictionary<string, string> listAllPolicy = new Dictionary<string, string>();

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

        public delegate void ShowPopup(object obj);
        public static ShowPopup onShowPopup;

        public EmployeePolicyPage(string token, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            TotalPage.Text = total_page.ToString();
            this.token = token;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            GetData();
            GetAllData();
            DataContext = this;

            DeleteEmployeePolicyGroup.hidePopup += HidePopup;
            DeleteEmployeePolicy.hidePopup += HidePopup;
            DetailEmployeePolicyGroup.hidePopup += HidePopup;
            DetailEmployeePolicy.hidePopup += HidePopup;
            AddEmployeePolicyGroup.hidePopup += HidePopup;
            AddEmployeePolicy.hidePopup += HidePopup;
            EditEmployeePolicyGroup.hidePopup += HidePopup;
            EditEmployeePolicy.hidePopup += HidePopup;
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
        private void navigateToWorkingRegulationsPage(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new WorkingRegulationsPage(token, PerAdd, PerEdit, PerDel, TypeUser));
                }
            }
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
                GetAllData();
            }

            onShowPopup("");
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.listEmployeePolicyPage;


                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                /*parameters.Add(new KeyValuePair<string, string>("rowno", ((page_now - 1)*COUNT_RECORD_PER_PAGE).ToString()));
                parameters.Add(new KeyValuePair<string, string>("rowperpage", COUNT_RECORD_PER_PAGE.ToString()));
                parameters.Add(new KeyValuePair<string, string>("keyword", tbSearch.Text));*/
                api = api + string.Format("?page={0}&pageSize={1}&keyWords={2}", page_now, COUNT_RECORD_PER_PAGE.ToString(), tbSearch.Text);
                httpRequestMessage.RequestUri = new Uri(api);

                /*var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;*/

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListEmployeePolicy result = JsonConvert.DeserializeObject<ListEmployeePolicy>(responseContent);

                if (result.success != null)
                {
                    total = result.success.data.total;
                    Paging();
                    listEmployeePolicy = result.success.data.data;
                    foreach (var item in listEmployeePolicy)
                    {
                        item.show = false;
                        Task<List<DataEntity3>> task = GetListPolicyBy(item.id);
                        await task;
                        item.data_detail = task.Result;
                        item.name = "NCS " + item.name; 
                    }
                    icEmployeePolicyPage.ItemsSource = listEmployeePolicy;
                    icEmployeePolicyPage.Items.Refresh();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("error");
            }

        }

        private async Task<List<DataEntity3>> GetListPolicyBy(string id_policy)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.list_employee_policy_by;


                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("id", id_policy));
                api = api + "?id=" + id_policy;
                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Content
                /*var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;*/

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListWorkingRegulationContent result = JsonConvert.DeserializeObject<ListWorkingRegulationContent>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    List<DataEntity3> listRecuitment = result.success.data.data;

                    return listRecuitment;
                }

                return null;

            }
            catch
            {
                return null;
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetAllData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.listEmployeePolicyPage;


                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                /*parameters.Add(new KeyValuePair<string, string>("rowno", "0"));
                parameters.Add(new KeyValuePair<string, string>("rowperpage", "1000"));
                parameters.Add(new KeyValuePair<string, string>("keyword", ""));*/
                api = api + string.Format("?page={0}&pageSize={1}&keyWords={2}", "1", "1000", "");
                httpRequestMessage.RequestUri = new Uri(api);

                /*var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;*/

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListEmployeePolicy result = JsonConvert.DeserializeObject<ListEmployeePolicy>(responseContent);

                if (result.success != null)
                {
                    foreach (var item in result.success.data.data)
                    {
                        listAllPolicy.Add(item.id, item.name);
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("error");
            }
        }

        private void Paging()
        {
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
            IsSetValidBtn();
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

        private void tbSeach_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                PageNow = "1";
                GetData();
            }
        }

        private void ClickSearch(object sender, MouseButtonEventArgs e)
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

        private void page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            double x = p.X;
            double y = p.Y;
            double left = popupOption.Margin.Left;
            double top = popupOption.Margin.Top;
            if ((x < left || x > (left + popupOption.Width)) && (y < top || y > top + popupOption.Height))
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowPopupOption(object sender, MouseButtonEventArgs e)
        {
            if (popupOption.Visibility == Visibility.Collapsed)
            {
                popupOption.Visibility = Visibility.Visible;
            }
            else
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
        }

        private void closeDetail(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity1 dataEntity = (DataEntity1)grid.DataContext;
            dataEntity.show = !dataEntity.show;
            icEmployeePolicyPage.ItemsSource = listEmployeePolicy;
            icEmployeePolicyPage.Items.Refresh();
        }

        private void ShowPopupDeleteGroup(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity1 dataEntity = (DataEntity1)textBlock.DataContext;
            string id = dataEntity.id;
            string name = dataEntity.name + " ?";
            DeleteEmployeePolicyGroup deleteEmployeePolicyGroup = new DeleteEmployeePolicyGroup(token, name, id);
            onShowPopup(deleteEmployeePolicyGroup);
        }

        private void ShowPopupDelete(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity3 dataEntity = (DataEntity3)textBlock.DataContext;
            string id = dataEntity.id;
            string name = dataEntity.name + " ?";
            DeleteEmployeePolicy deleteEmployeePolicy = new DeleteEmployeePolicy(token, name, id);
            onShowPopup(deleteEmployeePolicy);
        }

        private void ShowPopupDetailGroup(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity1 dataEntity = (DataEntity1)textBlock.DataContext;
            string id = dataEntity.id;
            DetailEmployeePolicyGroup detailEmployeePolicyGroup = new DetailEmployeePolicyGroup(token, id);
            onShowPopup(detailEmployeePolicyGroup);
        }

        private void ShowPopupDetail(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity3 dataEntity = (DataEntity3)textBlock.DataContext;
            string id = dataEntity.id;
            DetailEmployeePolicy detailEmployeePolicy = new DetailEmployeePolicy(token, id);
            onShowPopup(detailEmployeePolicy);
        }

        private void ShowPopupAddGroup(object sender, MouseButtonEventArgs e)
        {
            AddEmployeePolicyGroup addEmployeePolicyGroup = new AddEmployeePolicyGroup(token);
            onShowPopup(addEmployeePolicyGroup);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ShowPopupAdd(object sender, MouseButtonEventArgs e)
        {
            AddEmployeePolicy addEmployeePolicy = new AddEmployeePolicy(token,listAllPolicy);
            onShowPopup(addEmployeePolicy);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void scroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer sc = sender as ScrollViewer;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                sc.ScrollToHorizontalOffset(sc.HorizontalOffset - e.Delta);
            }
            else
                scData.ScrollToVerticalOffset(scData.VerticalOffset - e.Delta);
        }

        private void ShowPopupEditGroup(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity1 dataEntity = (DataEntity1)textBlock.DataContext;
            string id = dataEntity.id;
            EditEmployeePolicyGroup editEmployeePolicyGroup = new EditEmployeePolicyGroup(token, id);
            onShowPopup(editEmployeePolicyGroup);
        }

        private void ShowPopupEdit(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity3 dataEntity = (DataEntity3)textBlock.DataContext;
            string id = dataEntity.id;
            EditEmployeePolicy editEmployeePolicy = new EditEmployeePolicy(token, id, listAllPolicy);
            onShowPopup(editEmployeePolicy);
        }
    }
}
