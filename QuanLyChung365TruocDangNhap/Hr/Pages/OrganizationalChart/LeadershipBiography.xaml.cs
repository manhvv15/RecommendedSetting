using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart
{
    /// <summary>
    /// Interaction logic for LeadershipBiography.xaml
    /// </summary>
    public partial class LeadershipBiography : Page, INotifyPropertyChanged
    {
        string token;
        string id;
        string pho_to_truong;

        const int COUNT_RECORD_PER_PAGE2 = 5;
        int page_now2 = 1;
        int total2;
        int total_page2 = 1;
        public string PageNow2
        {
            get { return page_now2.ToString(); }
            set
            {
                page_now2 = Convert.ToInt32(value);
                OnPropertyChanged("PageNow2");
            }
        }

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        public LeadershipBiography(string token, string id, string pho_to_truong)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.pho_to_truong = pho_to_truong;
            this.DataContext = this;
            GetAllData();
        }
        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetListLeaderCompany();
            }
            onShowPopup("");
        }

        //get all void
        private void GetAllData()
        {
            GetListLeaderCompany();
        }
        //end getall void

        //next page
        private void NavigateToPage(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string name = textBlock.Name;
            String uri = "OrganizationalChart/" + name + "Page";
            switch (name)
            {
                case "OrganisationalStructureDiagram":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new OrganisationalStructureDiagram(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "PositionChart":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new PositionChart(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "RightToUse":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RightToUse(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "LeadershipBiography":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new LeadershipBiography(token, id, pho_to_truong));
                        }
                    };
                    break;
            }
        }
        //End next page
        //get data table 2
        private async void GetListLeaderCompany()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3006/api/hr/organizationalStructure/listInfoLeader";
                var form = new Dictionary<string, string>();
                form.Add("page", page_now2.ToString());
                form.Add("pageSize", COUNT_RECORD_PER_PAGE2.ToString());
                form.Add("keyword", tbSearch.Text);
                form.Add("winform", "1");
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);

                if (result.data != null)
                {
                    List<Items3> items = result.data.items;
                        foreach (var item in items)
                    {
                        if (item.Url_image == "")
                        {
                            item.Url_image = "https://phanmemnhansu.timviec365.vn/assets/images/t_images/logo_com.png";
                        }

                        if (item.dep_name == null)
                        {
                            item.dep_name = "Chưa cập nhật";
                        }
                    }
                    total2 = result.data.totalItems;
                    Paging2();
                    icListLeader.Items.Refresh();
                    icListLeader.ItemsSource = result.data.items;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lại!");
            }
        }
        //End get data table 2

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PageNow2 = "1";
                GetListLeaderCompany();
            }
        }

        private void NextPage2(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn2();
            int page_next = page_now2 + 1;
            PageNow2 = page_next.ToString();
            GetListLeaderCompany();
        }

        private void PrevPage2(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn2();
            int page_prev = page_now2 - 1;
            PageNow2 = page_prev.ToString();
            GetListLeaderCompany();
        }

        private void InvalidBtn2()
        {
            btnPrev2.IsEnabled = false;
            btnPrev2.Opacity = 0.3;
            btnNext2.IsEnabled = false;
            btnNext2.Opacity = 0.3;
        }

        private void Paging2()
        {
            if (total2 == 0)
            {
                total_page2 = 1;
            }
            else
            {
                if (total2 % COUNT_RECORD_PER_PAGE2 == 0)
                {
                    total_page2 = total2 / COUNT_RECORD_PER_PAGE2;
                }
                else
                {
                    total_page2 = total2 / COUNT_RECORD_PER_PAGE2 + 1;
                }
            }
            IsSetValidBtn2();
        }

        private void IsSetValidBtn2()
        {
            if (page_now2 == 1)
            {
                btnPrev2.IsEnabled = false;
                btnPrev2.Opacity = 0.3;
            }
            else
            {
                btnPrev2.IsEnabled = true;
                btnPrev2.Opacity = 1;
            }

            if (page_now2 == total_page2)
            {
                btnNext2.IsEnabled = false;
                btnNext2.Opacity = 0.3;
            }
            else
            {
                btnNext2.IsEnabled = true;
                btnNext2.Opacity = 1;
            }
        }

        private void clickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow2 = "1";
            GetListLeaderCompany();
        }

        private void scroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
        }
    }
}
