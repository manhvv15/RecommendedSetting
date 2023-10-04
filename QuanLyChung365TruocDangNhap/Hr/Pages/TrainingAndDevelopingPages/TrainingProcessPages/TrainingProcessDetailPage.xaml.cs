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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.TrainingAndDevelopingPages.TrainingProcessPages
{
    /// <summary>
    /// Interaction logic for TrainingProcessDetailPage.xaml
    /// </summary>
    public partial class TrainingProcessDetailPage : Page, INotifyPropertyChanged
    {
        string token;
        string id;
        string name;

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
        public TrainingProcessDetailPage(string token, string id, string name, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.name = name;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            GetData();
            txtId.Text = "(QTTD" + id + ")";
            txtName.Text = name;
            DataContext = this;
            AddTrainingPhase.hidePopup += HidePopup;
            DeletePhase.hidePopup += HidePopup;
            UpdatePhase.hidePopup += HidePopup;

        }
        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }
        private void NavigateBack(object sender, MouseButtonEventArgs e)
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
                string api = APIs.API.listStageTraining;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("processTrainId", id));
                parameters.Add(new KeyValuePair<string, string>("winform", "1"));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                StageList result = JsonConvert.DeserializeObject<StageList>(responseContent);

                List<ListStageTraining> listStage = result.success.data.data;

                if (listStage == null)
                {
                    tbNoData.Visibility = Visibility.Visible;
                }
                else
                {
                    tbNoData.Visibility = Visibility.Collapsed;
                    icDataDetail.ItemsSource = listStage;
                }

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void ShowPopupAddTrain(object sender, MouseButtonEventArgs e)
        {

            AddTrainingPhase addProcedure = new AddTrainingPhase(token, id);
            onShowPopup(addProcedure);
        }

        private void ClickDelete(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            ListStageTraining listStageTraining = grid.DataContext as ListStageTraining;
            string name = listStageTraining.name;
            string id = listStageTraining.id;
            DeletePhase deletePhase = new DeletePhase(token, name, id);
            onShowPopup(deletePhase);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ClickUpdate(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            ListStageTraining listTraining = (ListStageTraining)grid.DataContext;

            string id = listTraining.id;
            string name = listTraining.name;
            string object_training = listTraining.object_training;
            string content = listTraining.content;

            UpdatePhase updatePhasePopup = new UpdatePhase(token, id, name, object_training, content);
            onShowPopup(updatePhasePopup);
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

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Children[0].Visibility = Visibility.Collapsed;
            TextBlock textBlock = (TextBlock)grid.Children[1];
            var converter = new BrushConverter();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#474747");
        }

        private void MouseLeftButtonDown_showOption(object sender, MouseButtonEventArgs e)
        {
            if (popupOption.Visibility == Visibility.Visible)
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
            else
            {
                Border border = sender as Border;
                Point p = e.GetPosition(this);
                double x = p.X - 20;
                double y = p.Y - 50;
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

        //private void MouseLeftButtonDown_showOption(object sender, MouseButtonEventArgs e)
        //{
        //    if (popupOption.Visibility == Visibility.Collapsed)
        //    {
        //        popupOption.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        popupOption.Visibility = Visibility.Collapsed;
        //    }

        //}
    }
}
