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
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_CauTrucPhongBan
{
    /// <summary>
    /// Interaction logic for Chi_Tiet_PB.xaml
    /// </summary>
    public partial class Chi_Tiet_PB : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Chi_Tiet_PB(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            this.DataContext = this;
        }
        public Chi_Tiet_PB(MainWindow main, Item_List_PhongBan Pb)
        {
            InitializeComponent();
            Main = main;
            this.DataContext = this;
            infoPb = Pb;
            getData();
        }
        private Item_List_PhongBan infoPb;
        private MainWindow Main;
        private List<Item_List_Staff_All> _List_Staff_All;
        public List<Item_List_Staff_All> List_Staff_All
        {
            get { return _List_Staff_All; }
            set
            {
                _List_Staff_All = value;
                OnPropertyChanged();
            }
        }
        private List<Item_List_PhongBan> _PhongBanItems;
        public List<Item_List_PhongBan> PhongBanItems
        {
            get { return _PhongBanItems; }
            set
            {
                _PhongBanItems = value;
                OnPropertyChanged();
            }
        }
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Chi_Tiet_PB));
        private async Task getData()
        {
            HttpClient http = new HttpClient();
            var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
            var pram = new List<string>();
            Dictionary<string, string> form = new Dictionary<string, string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                    break;
                default:
                    break;
            }
            form.Add("dep_id", infoPb.dep_id);
            HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
            var check = respon.Content.ReadAsStringAsync().Result;
            API_List_Staff_All api = JsonConvert.DeserializeObject<API_List_Staff_All>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null)
            {
                List_Staff_All = api.data.items;
            }
        }
        private void table_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }

        private async void DeleteStaffOutOFDep_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        var row = (DataGridRow)vis;
                        var z = row.Item as Item_List_Staff_All;

                        if (z != null)
                        {
                            var c = new PopUp.Noti_Delete(Main);
                            c.Message = "Bạn có chắc chắn muốn xóa Nhân viên khỏi phòng ban này?";
                            Main.ShowPopup(c);
                            c.Accept = async () =>
                            {
                                HttpClient http = new HttpClient();
                                switch (Main.Type)
                                {
                                    case 1:
                                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                        break;
                                    case 2:
                                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                        break;
                                    default:
                                        break;
                                }

                                Dictionary<string, string> form = new Dictionary<string, string>();
                                form.Add("_id", z._id);
                                //form.Add("id_com", z.com_id);
                                
                                HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/managerUser/del/dep", new FormUrlEncodedContent(form));
                                API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                                if (api.data != null && api.data.result == true)
                                {
                                    if (Success != null) Success();
                                    var ep = List_Staff_All.Find(i => i.ep_id == z.ep_id);
                                    ep.dep_id = null;
                                    List_Staff_All = List_Staff_All.ToList();
                                    MessageBox.Show("Xoá nhân viên khỏi phòng ban thành công");
                                    getData();
                                }
                            };
                            
                        }
                        break;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Goback_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (Main.Type)
            {
                case 2:
                    //Main.SideBarIndex = 8;
                    Main.HomeSelectionPage.NavigationService.GoBack();
                    break;
                default:
                    break;
            }
            Main.ClosePopup();
            
        }
    }
}
