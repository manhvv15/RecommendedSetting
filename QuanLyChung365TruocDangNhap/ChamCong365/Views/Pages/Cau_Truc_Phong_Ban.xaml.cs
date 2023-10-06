using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using System.Drawing;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Cau_Truc_Phong_Ban.xaml
    /// </summary>
    public partial class Cau_Truc_Phong_Ban : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Cau_Truc_Phong_Ban(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData().GetAwaiter();
            getChildCom().ContinueWith(x => this.Dispatcher.Invoke(() =>
            {
                listCom = x.Result;
                if (listCom.Count > 0)
                {
                    cbbCom.SelectedIndex = listCom.Count - 1;
                }
            }));

        }
        //
        public MainWindow Main;
        public class ItemsPhongBan
        {
            public string PhongBan { get; set; }

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

        private List<ChildCompany> _listCom;

        public List<ChildCompany> listCom
        {
            get { return _listCom; }
            set { _listCom = value; OnPropertyChanged();}
        }


        private int _IsSmailSize;
        public int IsSmailSize
        {
            get { return _IsSmailSize; }
            set
            {
                _IsSmailSize = value;
                OnPropertyChanged("IsSmailSize");
            }
        }
        //SizeChanged
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 910)
            {
                IsSmailSize = 0;
            }
            else if (this.ActualWidth <= 910 && this.ActualWidth > 500)
            {
                IsSmailSize = 1;
            }
            else
            {
                IsSmailSize = 2;
            }
        }

        // scroll when mouse wheel
        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        //get data phongban from api
        private async Task getData()
        {
            string apilink = "http://210.245.108.202:3000/api/qlc/department/list";
            HttpClient httpClient = new HttpClient();
            Dictionary<string, string> form = new Dictionary<string, string>();
            switch (Main.Type)
            {
                case 1:
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);

                    break;
                case 2:
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                    break;
                default:
                    break;
            }
            int i = 0;
            List<Item_List_PhongBan> list = new List<Item_List_PhongBan>();

            var respon = await httpClient.PostAsync(apilink, new FormUrlEncodedContent(form));

            API_List_PhongBan api = JsonConvert.DeserializeObject<API_List_PhongBan>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null)
            {
                PhongBanItems = api.data.items;
            }
        }
        private async Task getDataAfter()
        {
            HttpClient http = new HttpClient();
            var z = cbbCom.SelectedItem as ChildCompany;
            string apilink = "http://210.245.108.202:3000/api/qlc/department/list";
            Dictionary<string, string> form = new Dictionary<string, string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    form.Add("com_id", z.com_id);

                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    form.Add("com_id", z.com_id);
                    break;
                default:
                    break;
            }
            var respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));

            API_List_PhongBan api = JsonConvert.DeserializeObject<API_List_PhongBan>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null)
            {
                PhongBanItems = api.data.items;
            }
        }


        private void PopUpAddPB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;
            var x = new PopUp.PopUp_CauTrucPhongBan.Add_More_PB(Main, z.com_id);
            Main.ShowPopup(x);

            x.Success = () =>
            {
                getDataAfter();
            };
        }

        public async Task<List<ChildCompany>> getChildCom(bool full = false)
        {
            try
            {
                string apilink = "http://210.245.108.202:3000/api/qlc/company/child/list";
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> form = new Dictionary<string, string>();
                switch (Main.Type)
                {
                    case 1:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                        break;
                    default:
                        break;
                }
                int i = 0;
                List<ChildCompany> list = new List<ChildCompany>();
                
                var respon = await httpClient.PostAsync(apilink,new FormUrlEncodedContent(form));
                API_Company_List ts = JsonConvert.DeserializeObject<API_Company_List>(respon.Content.ReadAsStringAsync().Result);
                if (ts != null)
                {
                    if (ts.data != null && ts.data.items != null)
                    {
                        ts.data.items.RemoveAll(item => item == null);
                        list = ts.data.items;
                        list.Add(new ChildCompany() { com_id = Main.APICompany.data.data.user_info.com_id, com_name = Main.APICompany.data.data.user_info.com_name, com_logo = Main.APICompany.data.data.user_info.com_logo, com_address = Main.APICompany.data.data.user_info.com_address, com_email = Main.APICompany.data.data.user_info.com_email, com_parent_id = "", com_phone = Main.APICompany.data.data.user_info.com_phone, com_role_id = Main.APICompany.data.data.user_info.com_role_id });
                    }
                }
                return list;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void DeleteShift_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var z = (sender as StackPanel).Tag as Item_List_PhongBan;
            if (z != null)
            {
                try
                {
                    var c = new PopUp.Noti_Delete(Main);
                    c.Message = "Bạn có chắc chắn muốn xóa Phòng ban này?";
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
                        form.Add("dep_id", z.dep_id);
                        form.Add("com_id", z.com_id);
                        HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, "http://210.245.108.202:3000/api/qlc/department/del");
                        request.Content = new FormUrlEncodedContent(form);
                        var respon = await http.SendAsync(request);
                        var check = respon.Content.ReadAsStringAsync().Result;
                        API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                        if (api.data != null && api.data.result == true)
                        {
                            getDataAfter();
                        }
                    };

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void EditShift_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var z = (sender as StackPanel).Tag as Item_List_PhongBan;
            if (z != null)
            {
                var x = new PopUp.PopUp_CauTrucPhongBan.Add_More_PB(Main, z);
                Main.ShowPopup(x);

                x.Success = () =>
                {
                    getDataAfter();
                };
            }
        }

        private void ChiTietShift_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var z = (sender as StackPanel).Tag as Item_List_PhongBan;
            if (z != null)
            {
                var x = new PopUp.PopUp_CauTrucPhongBan.Chi_Tiet_PB(Main, z);
                Main.NewPage(x);

                x.Success = () =>
                {
                    getDataAfter();
                };
            }
        }
        public async Task<List<Item_List_PhongBan>> getDep(string idcom, bool full = false)
        {
            try
            {
                string apilink = "http://210.245.108.202:3000/api/qlc/department/list";
                HttpClient httpClient = new HttpClient();
                switch (Main.Type)
                {
                    case 1:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                List<Item_List_PhongBan> list = new List<Item_List_PhongBan>();
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", idcom);
                var respon = await httpClient.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_List_PhongBan ts = JsonConvert.DeserializeObject<API_List_PhongBan>(respon.Content.ReadAsStringAsync().Result);
                if (ts != null)
                {
                    if (ts.data != null && ts.data.items != null)
                    {
                        ts.data.items.RemoveAll(item => item == null);
                        list = ts.data.items;
                    }
                }
                return list;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private void cbbCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;
            if (z != null)
            {
                getDep(z.com_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    PhongBanItems = tt.Result;
                    //PhongBanItems.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Phòng ban" });
                    PhongBanItems = PhongBanItems.ToList();
                }));
            }
        }
    }
}
