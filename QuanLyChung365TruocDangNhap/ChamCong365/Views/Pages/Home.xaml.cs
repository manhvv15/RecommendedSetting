using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Home(MainWindow login)
        {
            InitializeComponent();
            this.DataContext = this;
            Login = login;
            getData();

            this.table.AutoReponsiveColumn(1, "IDnTen");
            this.table2.AutoReponsiveColumn(1, "ep_name");
        }
        //
        private int isSmallPage;
        public int IsSmallPage
        {
            get { return isSmallPage; }
            set
            {
                isSmallPage = value;
                OnPropertyChanged("IsSmallPage");
            }
        }
        private MainWindow Login;
        private List<Employee> _listEmployee;
        public List<Employee> listEmployee
        {
            get { return _listEmployee; }
            set { _listEmployee = value; OnPropertyChanged(); }
        }
        private List<Htr_Att_Staff> _ListAttStaffNearly;
        public List<Htr_Att_Staff> ListAttStaffNearly
        {
            get { return _ListAttStaffNearly; }
            set { _ListAttStaffNearly = value; OnPropertyChanged(); }
        }

        private async Task<API_Employee_List> getEp1(int page = 1)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/cycle/list_not_in_cycle";
                var pram = new Dictionary<string, string>();

                switch (Login.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.APIStaff.data.data.access_token);
                        pram.Add("com_id", Login.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                if (page <= 0) page = 1;
                var offset = (page - 1) * 10;
                pram.Add("page", page.ToString());
                pram.Add("apply_month", DateTime.Now.ToString("yyyy-MM-dd"));
                pram.Add("pageSize", "20");
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(pram));
                var check = respon.Content.ReadAsStringAsync().Result;
                API_Employee_List api = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null) return api;
                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private async Task<API_Htr_Att_Staff> getEp2(int page = 1)
        {
            var startdate = DateTime.Today.ToString("yyyy-MM-dd");
            var endate = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company";
                var pram = new List<string>();

                switch (Login.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.APIStaff.data.data.access_token);
                        pram.Add("id_com="+ Login.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                if (page <= 0) page = 1;
                var offset = (page - 1) * 10;
                pram.Add("off_set=" + offset.ToString());

                pram.Add("length=10");
                pram.Add("start_date="+startdate);
                pram.Add("end_date=" + endate);

                if (pram.Count > 0)
                {
                    apilink += "?" + string.Join("&", pram);
                }
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("page", page.ToString());
                form.Add("pageSize", "10");
                form.Add("start_date", startdate);
                form.Add("end_date", endate);
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_Htr_Att_Staff api = JsonConvert.DeserializeObject<API_Htr_Att_Staff>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null) return api;
                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private async Task getData()
        {
            loading.Visibility = Visibility.Visible;
            var ep1 = getEp1().ContinueWith(tt => this.Dispatcher.Invoke(() =>
              {
                  if (tt.Result != null)
                  {
                      listEmployee = tt.Result.data.items;
                  }
                  getEp1().ContinueWith(zz => this.Dispatcher.Invoke(() =>
                  {
                      if (zz.Result != null)
                      {
                          listEmployee = zz.Result.data.items;
                          paginLichLamViec.TotalRecords = int.Parse(zz.Result.data.totalItems);
                      }
                  }));
              }));

            var ep2 = getEp2().ContinueWith(tt => this.Dispatcher.Invoke(() =>
              {
                  if (tt.Result != null)
                  {
                      ListAttStaffNearly = tt.Result.data.items;
                  }
                  getEp2().ContinueWith(zz => this.Dispatcher.Invoke(() =>
                  {
                      if (zz.Result != null)
                      {
                          ListAttStaffNearly = zz.Result.data.items;
                          paginDiemDanh.TotalRecords = int.Parse(zz.Result.data.totalItems);
                      }
                  }));
              }));
            var k = new List<Task> { ep1, ep2 };
            k.ForEach(t =>
            {
                t.ContinueWith(tt =>
                {
                    var ck = new List<bool>();
                    k.ForEach(h => ck.Add(h.IsCompleted));
                    if (!ck.Contains(false))
                    {
                        this.Dispatcher.Invoke(() => loading.Visibility = Visibility.Collapsed);
                    }
                });
            });
        }
        //
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 960)
            {
                IsSmallPage = 0;
            }
            else if (this.ActualWidth <= 960 && this.ActualWidth > 460)
            {
                IsSmallPage = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallPage = 2;
            }
        }
        //Foword History_Attendence_staff
        private void Htr_Att_Staff_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Login.HomeSelectionPage.NavigationService.Navigate(new History_Attendence_Staff(Login));
            if (Login.SideBarItems.Count < 17)
            {

                Login.SideBarIndex = 7;
            }
            else
            {
                Login.SideBarIndex = 12;
            }

        }

        private void Child_Company_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Login.SideBarItems.Count < 17)
                Login.SideBarIndex = 5;
            Login.SideBarIndex = 7;


        }

        private void shift_Manage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Login.SideBarItems.Count < 17)
                Login.SideBarIndex = 5;
            Login.SideBarIndex = 6;
        }

        private void ChamCongQR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login.SideBarIndex = 2;
            Login.HomeSelectionPage.NavigationService.Navigate(new Views.Pages.CauHinhChamCong.CauHinhChamCong_Main(Login, 1));
        }

        private void ThietBiChoDuyet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login.SideBarIndex = 2;
            Login.HomeSelectionPage.NavigationService.Navigate(new Views.Pages.CauHinhChamCong.CauHinhChamCong_Main(Login, 2));
        }

        private void table_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Login.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void paginLichLamViec_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            getEp1(paginLichLamViec.SelectedPage).ContinueWith(zz => this.Dispatcher.Invoke(() =>
            {
                if (zz.Result != null)
                {
                    listEmployee = zz.Result.data.items;
                }
            }));
        }
        private void paginDiemDanh_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            getEp2(paginDiemDanh.SelectedPage).ContinueWith(zz => this.Dispatcher.Invoke(() =>
            {
                if (zz.Result != null)
                {
                    ListAttStaffNearly = zz.Result.data.items;
                }
            }));
        }
    }
}
