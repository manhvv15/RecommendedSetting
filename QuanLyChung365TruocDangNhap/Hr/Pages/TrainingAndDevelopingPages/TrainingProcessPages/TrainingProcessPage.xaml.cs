using QuanLyChung365TruocDangNhap.Hr.Entities.TrainingAndDeveloping;
using QuanLyChung365TruocDangNhap.Hr.Popups.TranningAndDeveloper;
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
using static QuanLyChung365TruocDangNhap.Hr.Popups.OverviewPopups.RegisterPopup;

namespace QuanLyChung365TruocDangNhap.Hr.Pages.TrainingAndDevelopingPages.TrainingProcessPages
{
    /// <summary>
    /// Interaction logic for TrainingProcessPage.xaml
    /// </summary>
    public partial class TrainingProcessPage : Page, INotifyPropertyChanged
    {
        const int COUNT_RECORD_PER_PAGE = 6;
        int page_now = 1;
        int total;
        int total_page = 1;
        private bool show;
        public string token;

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
        public TrainingProcessPage(string token, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            GetData();
            TotalPage.Text = total_page.ToString();
            DataContext = this;

            AddProcedure.hidePopup += HidePopup;
            DeleteProcess.hidePopup += HidePopup;

        }
        private void ShowPopupAddNewProcedure(object sender, MouseButtonEventArgs e)
        {
            AddProcedure addProcedure = new AddProcedure(token);
            onShowPopup(addProcedure);
        }
        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }
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
            DataEntity listProcessEntity = grid.DataContext as DataEntity;
            string id = listProcessEntity.id;
            string name = listProcessEntity.name;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new TrainingProcessDetailPage(token, id, name, PerAdd, PerEdit, PerDel));
                }
            }
        }
        private void page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (page.ActualWidth < 800)
            {
                Show = false;
            }
            else
            {
                Show = true;
            }
        }
        //connect api và đổ dữ liệu
        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_tranning;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("page", page_now.ToString()));
                parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE.ToString()));
                parameters.Add(new KeyValuePair<string, string>("name", tbSearch.Text));

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListTrainingProcessEntity result = JsonConvert.DeserializeObject<ListTrainingProcessEntity>(responseContent);

                total = result.success.data.total;
                Paging();

                List<DataEntity> listTrainning = result.success.data.data;

                icListTraining.ItemsSource = listTrainning;

            }
            catch (Exception)
            {

                throw;
            }
        }

        //search
        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PageNow = "1";
                GetData();
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
            ValidBtn();
            int page_next = page_now + 1;
            PageNow = page_next.ToString();
            GetData();
        }
        private void PrevPage(object sender, MouseButtonEventArgs e)
        {
            ValidBtn();
            int page_prev = page_now - 1;
            PageNow = page_prev.ToString();
            GetData();
        }

        private void ValidBtn()
        {
            btnPrev.IsEnabled = false;
            btnPrev.Opacity = 0.3;
            btnNext.IsEnabled = false;
            btnNext.Opacity = 0.3;
        }

        private void ShowPopupDeleteProcess(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity dataEntity = grid.DataContext as DataEntity;
            string id = dataEntity.id;
            string name = dataEntity.name;
            DeleteProcess deleteProcess = new DeleteProcess(token, id, name);
            onShowPopup(deleteProcess);
        }

        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData();
        }
    }
}
