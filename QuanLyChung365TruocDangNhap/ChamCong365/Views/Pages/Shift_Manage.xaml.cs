using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
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
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Shift_Manage.xaml
    /// </summary>
    //public class ItemsShift
    //{
    //    public bool IsAddItem { get; set; }
    //    public string Shift { get; set; }
    //    public string Time { get; set; }
    //}
    public partial class Shift_Manage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Shift_Manage(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData().GetAwaiter();
        }
        private MainWindow Main;
        private List<Shift_Manage_Item> _ItemsShift;
        public List<Shift_Manage_Item> ShiftItems
        {
            get { return _ItemsShift; }
            set
            {
                _ItemsShift = value;
                if(value==null)_ItemsShift = new List<Shift_Manage_Item>();
                _ItemsShift.Add(new Shift_Manage_Item() { IsAddItem = true});
                OnPropertyChanged();
            }
        }
        private List<ChildCompany> _listCom;

        public List<ChildCompany> listCom
        {
            get { return _listCom; }
            set { _listCom = value; OnPropertyChanged(); }
        }
        private int isSmallSize;
        public int IsSmallSize
        {
            get { return isSmallSize; }
            set { isSmallSize = value; OnPropertyChanged("IsSmallSize"); }
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
        //get shift data from apo
        private async Task getData()
        {
            HttpClient http = new HttpClient();
            var apilink = "http://210.245.108.202:3000/api/qlc/shift/list";
            var pram = new Dictionary<string,string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    apilink = apilink + "?com_id=" + Main.APIStaff.data.data.user_info.com_id; 
                    pram.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    break;
                default:
                    break;
            }
            getChildCom().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                listCom = tt.Result;
                listCom.Insert(0, new ChildCompany() { com_id = "-1", com_name = "Tất cả công ty" });
                listCom = listCom.ToList();
                cbbCom.SelectedIndex = listCom.Count - 1;
            }));
            HttpResponseMessage respon = await http.GetAsync(apilink);
            API_Shift_Manage api = JsonConvert.DeserializeObject<API_Shift_Manage>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null)
            {
                ShiftItems = api.data.items;
            }
        }
        private async Task getDataAfter()
        {
            HttpClient http = new HttpClient();
            var z = cbbCom.SelectedItem as ChildCompany;
            var apilink = "http://210.245.108.202:3000/api/qlc/shift/list?com_id=" + z.com_id;
            var pram = new Dictionary<string,string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    pram.Add("com_id", z.com_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    pram.Add("com_id", z.com_id);
                    break;
                default:
                    break;
            }
            HttpResponseMessage respon = await http.GetAsync(apilink);
            API_Shift_Manage api = JsonConvert.DeserializeObject<API_Shift_Manage>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null)
            {
                ShiftItems = api.data.items;
            }
        }
        //Size changed
        private void lv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 910)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 910 && this.ActualWidth > 550)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
        }
        // lăn chuột để cuộn 
        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);

        }
        private void AddItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;

            var x = new PopUp.PopUp_Shift_manage.Add_Shift(Main, z.com_id);
            Main.ShowPopup(x);

            x.Success = () =>
            {
                getDataAfter();
            };
        }

        private void DeleteShift_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var z = (sender as StackPanel).Tag as Shift_Manage_Item;
            if (z != null)
            {
                try
                {
                    var c = new PopUp.Noti_Delete(Main);
                    c.Message = "Bạn có chắc chắn muốn xóa ca?";
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
                        form.Add("shift_id", z.shift_id);
                        form.Add("com_id", z.com_id);
                        HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/shift/delete", new FormUrlEncodedContent(form));
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
            var z = (sender as StackPanel).Tag as Shift_Manage_Item;
            if (z != null)
            {
                var x = new PopUp.PopUp_Shift_manage.Add_Shift(Main, z);
                Main.ShowPopup(x);

                x.Success = () =>
                {
                    getDataAfter();
                };
            }
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        public async Task<List<Shift_Manage_Item>> getShift(string idcom, bool full = false)
        {
            try
            {
                string apilink = "http://210.245.108.202:3000/api/qlc/shift/list?com_id=" + idcom;
                HttpClient httpClient = new HttpClient();
                var form = new Dictionary<string, string>();
                switch (Main.Type)
                {
                    case 1:
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                form.Add("com_id", idcom);
                List<Shift_Manage_Item> list = new List<Shift_Manage_Item>();
                var respon = await httpClient.GetAsync(apilink);
                var check = respon.Content.ReadAsStringAsync().Result;
                API_Shift_Manage ts = JsonConvert.DeserializeObject<API_Shift_Manage>(respon.Content.ReadAsStringAsync().Result);
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
                getShift(z.com_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    ShiftItems = tt.Result;
                }));
            }
        }
    }
}
