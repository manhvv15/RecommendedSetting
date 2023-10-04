using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Regulation;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Regulation.Policy;
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
    /// Interaction logic for WorkingRegulationsPage.xaml
    /// </summary>
    public partial class WorkingRegulationsPage : Page, INotifyPropertyChanged
    {
        const int COUNT_RECORD_PER_PAGE = 6;
        int page_now = 1;
        int total_page = 1;
        int total;
        string token;
        int TypeUser;
        public List<DataEntity> listEmployeePolicy;
        Dictionary<string, string> listAllRegulation;

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
        public WorkingRegulationsPage(string token, bool perAdd, bool perEdit, bool perDel,int TypeUser)
        {
            InitializeComponent();
            this.token = token;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            this.TypeUser = TypeUser;
            DataContext = this;
            GetData();
            GetAllDataCbx();
            DeleteGroupRegulation.hidePopup += HidePopup;
            DeleteRegulation.hidePopup += HidePopup;
            DetailRegulation.hidePopup += HidePopup;
            DetailPolicy.hidePopup += HidePopup;
            AddRegulation.hidePopup += HidePopup;
            EditRegulation.hidePopup += HidePopup;
            AddPolicy.hidePopup += HidePopup;
            EditPolicy.hidePopup += HidePopup;

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
        private void navigateToEmployeePolicyPage(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new EmployeePolicyPage(token, PerAdd, PerEdit, PerDel));
                }
            }
        }
        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.listWorkingRegulations;

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                /*parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE.ToString()));
                parameters.Add(new KeyValuePair<string, string>("page", PageNow));
                parameters.Add(new KeyValuePair<string, string>("keyWords", tbSeach.Text));*/
                api = api + string.Format("?page={0}&pageSize={1}&keyWords={2}", PageNow, COUNT_RECORD_PER_PAGE.ToString(), tbSeach.Text);
                httpRequestMessage.RequestUri = new Uri(api);
                //var content = new FormUrlEncodedContent(parameters);
                //httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listWorkingRegulations result = JsonConvert.DeserializeObject<listWorkingRegulations>(responseContent);

                if (result.success != null)
                {
                    total = result.success.data.total;
                    Paging();
                    listEmployeePolicy = result.success.data.data;
                    foreach(var item in listEmployeePolicy)
                    {
                        item.name = "NQĐ - " + item.name;
                        item.show = false;
                        Task<List<DataEntity3>> task = GetListPolicyBy(item.id);
                        await task;

                        if(item.data_detail == null)
                        {
                        }
                        item.data_detail = task.Result;
                    }
                    icWorkingRegulationsPage.ItemsSource = listEmployeePolicy;
                    icWorkingRegulationsPage.Items.Refresh();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("error");
            }

        }

        private async void GetAllDataCbx()
        {
            Task<Dictionary<string, string>> task = GetAllData();
            await task;
            listAllRegulation = task.Result;
        }

        private async Task<Dictionary<string, string>> GetAllData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.listWorkingRegulations;


                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                /*parameters.Add(new KeyValuePair<string, string>("pageSize", "1000"));
                parameters.Add(new KeyValuePair<string, string>("page", "1"));
                parameters.Add(new KeyValuePair<string, string>("keyWords", ""));*/
                api = api + "?page=1&pageSize=1000&keyWords=";
                httpRequestMessage.RequestUri = new Uri(api);
                //var content = new FormUrlEncodedContent(parameters);
                //httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listWorkingRegulations result = JsonConvert.DeserializeObject<listWorkingRegulations>(responseContent);
                Dictionary<string, string> listAllRegulation = new Dictionary<string, string>();

                if (result.success != null)
                {
                    foreach(var item in result.success.data.data)
                    {
                        listAllRegulation.Add(item.id, item.name);
                    }
                }

                return listAllRegulation;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private async Task<List<DataEntity3>> GetListPolicyBy(string id_policy)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.list_policy_by;


                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("id", id_policy));
                api = api + "?id=" + id_policy;
                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Content
                //var content = new FormUrlEncodedContent(parameters);
                //httpRequestMessage.Content = content;

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
                    totalpgae.Text = total_page.ToString();
                }
                else
                {
                    total_page = total / COUNT_RECORD_PER_PAGE + 1;
                    totalpgae.Text = total_page.ToString();
                }
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

        private void DisableBtnPage()
        {
            btnPrev.IsEnabled = false;
            btnPrev.Opacity = 0.3;
            btnNext.IsEnabled = false;
            btnNext.Opacity = 0.3;
        }

        private void PrevPage(object sender, MouseButtonEventArgs e)
        {
            DisableBtnPage();
            int page_prev = page_now - 1;
            PageNow = page_prev.ToString();
            GetData();
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

        private void HidePopup(int mode)
        {
            if(mode == 1)
            {
                GetData();
                GetAllDataCbx();
            }

            onShowPopup("");
        }

        private void clickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData();
        }

        private void tbSeach_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                PageNow = "1";
                GetData();
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

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Children[0].Visibility = Visibility.Collapsed;
            TextBlock textBlock = (TextBlock)grid.Children[1];
            var converter = new BrushConverter();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#474747");
        }

        private void closeDetail(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity dataEntity = (DataEntity)grid.DataContext;
            dataEntity.show = !dataEntity.show;
            icWorkingRegulationsPage.ItemsSource = listEmployeePolicy;
            icWorkingRegulationsPage.Items.Refresh();
            var x = Mouse.GetPosition(icWorkingRegulationsPage);
            var y = x.Y - 20;
            scroll.ScrollToVerticalOffset(y);
        }

        private void ShowPopupDeleteGroup(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity dataEntity = (DataEntity)textBlock.DataContext;
            string name = dataEntity.name + " ?";
            string id = dataEntity.id;
            DeleteGroupRegulation deleteGroupRegulation = new DeleteGroupRegulation(token, name, id);
            onShowPopup(deleteGroupRegulation);
        }

        private void ShowPopupDelete(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity3 dataEntity = (DataEntity3)textBlock.DataContext;
            string name = dataEntity.name + " ?";
            string id = dataEntity.id;
            DeleteRegulation deleteGroupRegulation = new DeleteRegulation(token, name, id);
            onShowPopup(deleteGroupRegulation);
        }

        private void ShowPopupDetailGroup(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity dataEntity = (DataEntity)textBlock.DataContext;
            string id = dataEntity.id;
            DetailRegulation detailRegulation = new DetailRegulation(token, id);
            onShowPopup(detailRegulation);
        }

        private void ShowPopupDetail(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity3 dataEntity = (DataEntity3)textBlock.DataContext;
            string id = dataEntity.id;
            DetailPolicy detailPolicy = new DetailPolicy(token, id,TypeUser);
            onShowPopup(detailPolicy);
        }


        private void ShowPopupOption(object sender, MouseButtonEventArgs e)
        {
            if(popupOption.Visibility == Visibility.Collapsed)
            {
                popupOption.Visibility = Visibility.Visible;
            }
            else
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void ShowPopupAddRegulationGroup(object sender, MouseButtonEventArgs e)
        {
            AddRegulation addRegulation = new AddRegulation(token);
            onShowPopup(addRegulation);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ShowPopupEditGroup(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity dataEntity = (DataEntity)textBlock.DataContext;
            string id = dataEntity.id;
            EditRegulation editRegulation = new EditRegulation(token, id);
            onShowPopup(editRegulation);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer sc = sender as ScrollViewer;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                sc.ScrollToHorizontalOffset(sc.HorizontalOffset - e.Delta);
            }
            else
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
        }

        private void ShowPopupAddRegulation(object sender, MouseButtonEventArgs e)
        {
            AddPolicy addPolicy = new AddPolicy(token, listAllRegulation);
            onShowPopup(addPolicy);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ShowPopupEdit(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataEntity3 dataEntity = (DataEntity3)textBlock.DataContext;
            string id = dataEntity.id;
            EditPolicy editPolicy = new EditPolicy(token, id, listAllRegulation, this);
            onShowPopup(editPolicy);
        }
    }
}
