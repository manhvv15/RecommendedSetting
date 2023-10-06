using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.RecuitmentProcessEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.RecruitmentProcessPopups;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.RecruitmentProcessPages
{
    /// <summary>
    /// Interaction logic for RecruitmentProcessDetailPage.xaml
    /// </summary>
    public partial class RecruitmentProcessDetailPage : Page, INotifyPropertyChanged
    {
        string id;
        string token;
        string title;

        // deligate event show popups
        public delegate void ShowPopup(object obj);

        public static event ShowPopup onShowPopup;
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

        public RecruitmentProcessDetailPage(string title, string id, string token, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.title = title;
            this.id = id;
            this.token = token;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            DataContext = this;
            txtTitle.Text = this.title;
            GetData();

            DeleteStagePopup.hidePopup += HidePopup;
            ProcessEditingPopup.hidePopup += HidePopup;
            MoreStagePopup.hidePopup += HidePopup;
        }

        private void Vali()
        {
            if (!perAdd && !perEdit)
            {
                borderSide.Visibility = Visibility.Collapsed;
            }
        }

        private void NaigateBack(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.NavigationService.GoBack();
                }
            }
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_recruitment_stage;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("recruitmentId", id));
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListRecruitmentStageEntity result = JsonConvert.DeserializeObject<ListRecruitmentStageEntity>(responseContent);

                List<Data2Entity> listRecuitment = result.success.data.data;

                if(listRecuitment == null)
                {
                    tbNoData.Visibility = Visibility.Visible;
                }
                else
                {
                    tbNoData.Visibility = Visibility.Collapsed;
                    icDataDetail.ItemsSource = listRecuitment;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
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
            if(popupOption.Visibility == Visibility.Visible)
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
            else
            {
                Grid grid = sender as Grid;
                Point p = e.GetPosition(this);
                double x = p.X;
                double y = p.Y - 0;
                if (x + popupOption.Width > page.ActualWidth)
                {
                    x = page.ActualWidth - popupOption.Width - 40;
                }
                Thickness thickness = new Thickness(x, y, 0, 0);
                popupOption.Margin = thickness;
                popupOption.Visibility = Visibility.Visible;
                popupOption.DataContext = grid.DataContext;
            }
            Vali();
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

        private void ClickDeleteProcess(object sender, MouseButtonEventArgs e)
        {
            Data2Entity dataEntity = (Data2Entity)popupOption.DataContext;
            string id = dataEntity.id;
            string name = dataEntity.name;
            DeleteStagePopup deleteStagePopup = new DeleteStagePopup(token, id, name);
            onShowPopup(deleteStagePopup);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ShowEditPopup(object sender, MouseButtonEventArgs e)
        {
            Data2Entity dataEntity = (Data2Entity)popupOption.DataContext;
            string id = dataEntity.id;
            string name = dataEntity.name;
            string time = dataEntity.complete_time;
            string description = dataEntity.description;
            string target = dataEntity.target;
            string job = dataEntity.position_assumed;
            ProcessEditingPopup processEditingPopup = new ProcessEditingPopup(token, job, name, target, time, description, id);
            onShowPopup(processEditingPopup);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ShowPopupAddStage(object sender, MouseButtonEventArgs e)
        {
            MoreStagePopup moreStagePopup = new MoreStagePopup(token, id);
            onShowPopup(moreStagePopup);
        }
    }
}
