using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Popups.OrganizationalChartPopup.RightToUse;
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
    /// Interaction logic for RightToUse.xaml
    /// </summary>
    public partial class RightToUse : Page, INotifyPropertyChanged
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
        public static Dictionary<string, string> listDepartment = new Dictionary<string, string>(); // save list Department

        public RightToUse(string token, string id, string pho_to_truong)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.pho_to_truong = pho_to_truong;
            DataContext = this;
            GetAllData();
            AddNewRightToUse.hidePopup += HidePopup;
            DeleteRightToUse.hidePopup += HidePopup;
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetListEmployee();
            }
            onShowPopup("");
        }

        //get all void
        private void GetAllData()
        {
            GetDatalistDepartment();
            GetListEmployee();
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
                            (window as HomeView).MainContent.Navigate(new LeadershipBiography(token,id, pho_to_truong));
                        }
                    };
                    break;
            }
        }
        //End next page


        //get list Department
        private async void GetDatalistDepartment()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listDepartment;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listDepartment result = JsonConvert.DeserializeObject<listDepartment>(responseContent);

                if (result.data == null) return;

                listDepartment = new Dictionary<string, string>();
                listDepartment.Add("", "Tất cả phòng ban");

                foreach (var item in result.data.items)
                {
                    listDepartment.Add(item.dep_id, item.dep_name);
                }
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan1.Items.Refresh();

                cbxTenPhongBan.ItemsSource = listDepartment;
                cbxTenPhongBan1.ItemsSource = listDepartment;
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lại!");
            }
        }
        //End list Department


        //get data table 1
        private async void GetListEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;

                string api = "";

                /*if(cbxTenPhongBan.SelectedIndex > 0)
                {
                    api = "http://210.245.108.202:3000/api/qlc/managerUser/list" + cbxTenPhongBan.SelectedValue.ToString() + "&filter_by[search]=" + tbSearch.Text;
                }
                else
                {
                    api = "http://210.245.108.202:3000/api/qlc/managerUser/list" + "&filter_by[search]=" + tbSearch.Text;
                }*/
                api = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                if(cbxTenPhongBan.SelectedIndex > 0)
                    form.Add("dep_id", cbxTenPhongBan.SelectedValue.ToString());
                form.Add("ep_signature", "1");
                form.Add("findbyNameUser", tbSearch.Text);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);

                if (result.data != null)
                {
                    icListStamp.Items.Refresh();
                    icListStamp.ItemsSource = result.data.items;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lại!");
            }
        }
        //End get data table 1


        //get data table 2
        private async void GetListLeaderCompany()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;

                string api = "";
                api = "http://210.245.108.202:3006/api/hr/organizationalStructure/listInfoLeader";
                var form = new Dictionary<string, string>();
                form.Add("page", page_now2.ToString());
                form.Add("pageSize", COUNT_RECORD_PER_PAGE2.ToString());
                form.Add("keyword", tbSearch1.Text);
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
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].dep_name == null)
                        {
                            items[i].dep_name = "Chưa cập nhật";
                        }
                        if(cbxTenPhongBan1.SelectedIndex > -1)
                        {
                            if (items[i].dep_name != cbxTenPhongBan1.Text)
                            {
                                items.RemoveAt(i);
                                i--;
                            }
                        }
                    }

                    total2 = result.data.totalItems;
                    Paging2();
                    icListSampleSignature.Items.Refresh();
                    icListSampleSignature.ItemsSource = items;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lại!");
            }
        }
        //End get data table 2


        //click search 
        private void clickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow2 = "1";
            GetListEmployee();
        }
        //End click search



        //search input cbx 1

        private void ShowCombobox(object sender, MouseButtonEventArgs e)
        {
            cbxTenPhongBan.IsDropDownOpen = !cbxTenPhongBan.IsDropDownOpen;
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PageNow2 = "1";
                GetListEmployee();
            }

        }

        private void cbxTenPhongBan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenPhongBan.SelectedIndex = -1;
            string textSearch = cbxTenPhongBan.Text + e.Text;
            cbxTenPhongBan.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenPhongBan.Text = "";
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment;
                cbxTenPhongBan.SelectedIndex = -1;
            }
            else
            {
                cbxTenPhongBan.ItemsSource = "";
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenPhongBan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenPhongBan.SelectedIndex = -1;
                string textSearch = cbxTenPhongBan.Text;
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
        //End search input cbx 1


        //search input cbx 2
        private void ShowCombobox1(object sender, MouseButtonEventArgs e)
        {
            cbxTenPhongBan1.IsDropDownOpen = !cbxTenPhongBan1.IsDropDownOpen;
        }

        private void cbxTenPhongBan_KeyUp1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenPhongBan1.SelectedIndex = -1;
                string textSearch1 = cbxTenPhongBan1.Text;
                cbxTenPhongBan1.Items.Refresh();
                cbxTenPhongBan1.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch1.ToLower()));
            }
        }

        private void cbxTenPhongBan_PreviewTextInput1(object sender, TextCompositionEventArgs e)
        {
            cbxTenPhongBan1.SelectedIndex = -1;
            string textSearch1 = cbxTenPhongBan1.Text + e.Text;
            cbxTenPhongBan1.IsDropDownOpen = true;
            if (textSearch1 == "")
            {
                cbxTenPhongBan1.Text = "";
                cbxTenPhongBan1.Items.Refresh();
                cbxTenPhongBan1.ItemsSource = listDepartment;
                cbxTenPhongBan1.SelectedIndex = -1;
            }
            else
            {
                cbxTenPhongBan1.ItemsSource = "";
                cbxTenPhongBan1.Items.Refresh();
                cbxTenPhongBan1.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch1.ToLower()));
            }
        }

        private void tbSearch_KeyUp1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PageNow2 = "1";
                GetListLeaderCompany();
            }

        }
        //End search input cbx

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

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void clickSearch1(object sender, MouseButtonEventArgs e)
        {
            PageNow2 = "1";
            GetListLeaderCompany();
        }


        //Show popup add new
        private void ShowPopupAddNew(object sender, MouseButtonEventArgs e)
        {
            AddNewRightToUse addNew = new AddNewRightToUse(token, id);
            onShowPopup(addNew);
        }
        //end show popup add new

        //Show popup delete
        private void showPopupDelete(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Items3 items3 = grid.DataContext as Items3;
            string ep_id = items3.ep_id;
            DeleteRightToUse deleteRightToUse = new DeleteRightToUse(token, id, ep_id);
            onShowPopup(deleteRightToUse);
        }

        private void ShopUpdatePopup(object sender, MouseButtonEventArgs e)
        {

        }

        private void icListSampleSignature_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                scData.ScrollToHorizontalOffset(scData.HorizontalOffset - e.Delta);
            }
            else
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
        }
        //end show popup delete
    }
}
