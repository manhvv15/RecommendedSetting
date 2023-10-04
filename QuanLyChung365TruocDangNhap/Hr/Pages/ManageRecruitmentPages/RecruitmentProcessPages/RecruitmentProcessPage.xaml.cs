using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.RecuitmentProcessEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.RecruitmentProcessPopups;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.RecruitmentProcessPages
{
    /// <summary>
    /// Interaction logic for RecruitmentProcessPage.xaml
    /// </summary>  
    public partial class RecruitmentProcessPage : Page, INotifyPropertyChanged
    {
        const int COUNT_RECORD_PER_PAGE = 5;
        int page_now = 1;
        int total;
        int total_page = 1;
        private bool show;
        string token;

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;

        public bool Show
        {
            get { return show; }
            set
            {
                show = value;
                OnPropertyChanged("Show");
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

        public string PageNow
        {
            get { return page_now.ToString(); }
            set
            {
                page_now = Convert.ToInt32(value);
                OnPropertyChanged("PageNow");
            }
        }

        public RecruitmentProcessPage(string token, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            DataContext = this;
            GetData();
            TotalPage.Text = total_page.ToString();
            DeleteProcessPopup.hidePopup += HidePopup;
            AddProcessPopup.hidePopup += HidePopup;
            EditProcessPopup.hidePopup += HidePopup;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        private void navigateToDetail(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity dataEntity = grid.DataContext as DataEntity;
            string id = dataEntity.id;
            string title = "(QTTD" + id +") " + dataEntity.name;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new RecruitmentProcessDetailPage(title, id, token, PerAdd, PerEdit, PerDel));
                }
            }
        }

        private void page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(page.ActualWidth < 800)
            {
                Show = false;
            }
            else
            {
                Show = true;
            }
        }

        private void HidePopup(int mode)
        {
            if(mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_recuitment;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("page", page_now.ToString()));
                parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE.ToString()));
                parameters.Add(new KeyValuePair<string, string>("name", tbSearch.Text));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                RecruitmentProcessEntity result = JsonConvert.DeserializeObject<RecruitmentProcessEntity>(responseContent);

                total = result.success.data.total;

                Paging();

                List<DataEntity> listRecuitment = result.success.data.data;

                foreach(var item in listRecuitment)
                {
                    item.created_at = ConvertDateTime(item.created_at);
                }

                icListRecuitment.ItemsSource = listRecuitment;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private string ConvertDateTime(string dateTime)
        {
            try
            {
                DateTime date = DateTime.ParseExact(dateTime, "yyyy-M-d HH:mm:ss", CultureInfo.InvariantCulture);
                return date.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch
            {
                return "";
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
            if(total == 0)
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
            if(page_now == 1)
            {
                btnPrev.IsEnabled = false;
                btnPrev.Opacity = 0.3;
            }
            else
            {
                btnPrev.IsEnabled = true;
                btnPrev.Opacity = 1;
            }

            if(page_now == total_page)
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

        private void DeleteRecuitment(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity dataEntity = grid.DataContext as DataEntity;
            string id = dataEntity.id;
            string title = "(QTTD" + id + ") " + dataEntity.name + " ?";
            DeleteProcessPopup deleteProcessPopup = new DeleteProcessPopup(token, id, title);
            onShowPopup(deleteProcessPopup);
        }

        private void ShowPopupAddProcess(object sender, MouseButtonEventArgs e)
        {
            AddProcessPopup addProcessPopup = new AddProcessPopup(token);
            onShowPopup(addProcessPopup);
        }

        private void ShowEditPopup(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity dataEntity = (DataEntity)grid.DataContext;
            string id = dataEntity.id;
            string name = dataEntity.name;
            string _object = dataEntity.apply_for;
            EditProcessPopup editProcessPopup = new EditProcessPopup(token, id, name, _object);
            onShowPopup(editProcessPopup);
        }

        private void ClickIconSearch(object sender, MouseButtonEventArgs e)
        {
            GetData();
        }
    }
}
