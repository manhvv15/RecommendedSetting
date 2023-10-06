using QuanLyChung365TruocDangNhap.Hr.Entities.TrainingAndDeveloping.JobPosition;
using QuanLyChung365TruocDangNhap.Hr.Popups.TranningAndDeveloper;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.TrainingAndDevelopingPages.JobPositionPages
{
    /// <summary>
    /// Interaction logic for JobPositionPage.xaml
    /// </summary>
    public partial class JobPositionPage : Page, INotifyPropertyChanged
    {
        const int COUNT_RECORD_PER_PAGE = 7;
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
        public JobPositionPage(string token, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            GetData();
            DataContext = this;
            TotalPage.Text = total_page.ToString();
            DeleteLocation.hidePopup += HidePopup;
            AddNewLocattion.hidePopup += HidePopup;
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

        private void HidePopup(int mode)
        {
            if (mode == 1)
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
                string api = APIs.API.list_JobPositon;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("page", page_now.ToString()));
                parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE.ToString()));

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListJobPositions result = JsonConvert.DeserializeObject<ListJobPositions>(responseContent);

                total = result.success.data.total;

                Paging();
                List<DataEntity> listJobPositons = result.success.data.data;
                foreach (var item in listJobPositons)
                {
                    if (item.road_map == null || item.road_map == "")
                    {
                        item.isFile = false;
                    }
                    else
                    {
                        item.isFile = true;
                    }
                }
                icListJobPositons.ItemsSource = listJobPositons;
            }
            catch (Exception)
            {

                //MessageBox.Show("hihiih");
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

        private void DisableBtn()
        {
            btnPrev.IsEnabled = false;
            btnPrev.Opacity = 0.3;
            btnNext.IsEnabled = false;
            btnNext.Opacity = 0.3;
        }
        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            int page_next = page_now + 1;
            PageNow = page_next.ToString();

            GetData();
        }

        private void PrevPage(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn();
            int page_prev = page_now - 1;
            PageNow = page_prev.ToString();
            GetData();
        }

        private void Paging()
        {
            if (total == 0)
            {
                total_page = 1;
            }
            else
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
            }
            IsSetValidBtn();
        }

        private void InvalidBtn()
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

        private void ShowPopupDeleteJob(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity dataEntity = grid.DataContext as DataEntity;
            string id = dataEntity.id;
            string name = dataEntity.name;
            DeleteLocation deleteLocation = new DeleteLocation(token, id, name);

            onShowPopup(deleteLocation);
        }

        private void scroll1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll1.ScrollToVerticalOffset(scroll1.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
                scroll1.ScrollToVerticalOffset(scroll1.VerticalOffset - e.Delta);
        }

        private void ShowAddNewLocationPopUp(object sender, MouseButtonEventArgs e)
        {
            AddNewLocattion addNewLocattion = new AddNewLocattion(token);
            onShowPopup(addNewLocattion);
        }

        private void ToDownload(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string file = ((DataEntity)textBlock.DataContext).road_map;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FileName = file;
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, file);
        }
    }
}
