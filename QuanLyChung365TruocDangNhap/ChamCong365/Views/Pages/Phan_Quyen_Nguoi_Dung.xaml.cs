using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Phan_Quyen_Nguoi_Dung.xaml
    /// </summary>
    public partial class Phan_Quyen_Nguoi_Dung : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Phan_Quyen_Nguoi_Dung(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
        }
        public MainWindow Main;
        public int totalEP = 0;
        private int pageEP = 1;
        private List<Employee> _listEmployee;
        public List<Employee> listEmployee
        {
            get { return _listEmployee; }
            set { _listEmployee = value; OnPropertyChanged(); }
        }
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Phan_Quyen_Nguoi_Dung));
        private async Task loadEP(int page = 1, int lenght = 20)
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
            form.Add("pageNumber", page.ToString());
            form.Add("pageSize", lenght.ToString());
            HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
            API_Employee_List nv = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
            if (nv.data != null)
            {
                int.TryParse(nv.data.totalItems, out totalEP);
                listEmployee.AddRange(nv.data.items);
                listEmployee = listEmployee.ToList();
                searchbarEP.Refresh();
            }
        }
        private void sreachbarEP_ScrollToEnd()
        {
            if (listEmployee.Count + (pageEP + 1 * 20) <= totalEP)
            {
                pageEP++;
                loadEP(pageEP, 20);
            }
        }
        private async Task getData()
        {
            listEmployee = new List<Employee>();
            loadEP(1,500);
        }
        public class quyen
        {
            public string permission_id { get; set; }
        }
        private async void searchbarEP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cb1.IsChecked = false;
                cb2.IsChecked = false;
                cb3.IsChecked = false;
                var z = searchbarEP.SelectedItem as Employee;
                if (z != null)
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
                    form.Add("id_us", z.ep_id);
                    HttpResponseMessage respon = await http.PostAsync("https://api.timviec365.vn/api/qlc/permission/check_role", new FormUrlEncodedContent(form));
                    var x = respon.Content.ReadAsStringAsync().Result;
                    API_QuyenNguoiDung q = JsonConvert.DeserializeObject<API_QuyenNguoiDung>(x);
                    if (q != null && !string.IsNullOrEmpty(q.permission_id))
                    {
                        if (q.quyen.Contains("1")) cb1.IsChecked = true;
                        if (q.quyen.Contains("2")) cb2.IsChecked = true;
                        if (q.quyen.Contains("3")) cb3.IsChecked = true;
                    }
                    else
                    {
                        cb1.IsChecked = false;
                        cb2.IsChecked = false;
                        cb3.IsChecked = false;
                    }
                }
            }
            catch { }
            
        }
        private async void Update(object sender, MouseButtonEventArgs e)
        {
            var z = searchbarEP.SelectedItem as Employee;
            List<string> abc = new List<string>();
            if (z != null)
            {
                if (cb1.IsChecked == true)
                {
                    abc.Add("1");
                }
                else
                {
                    abc.Remove("1");
                }
                if (cb2.IsChecked == true)
                {
                    abc.Add("2");
                }
                else
                {
                    abc.Remove("2");
                }
                if (cb3.IsChecked == true)
                {
                    abc.Add("3");
                }
                else
                {
                    abc.Remove("3");
                }
                var q = "[{0}]";
                for (int i = 0; i < abc.Count; i++)
                {
                    abc[i] = string.Format("\"{0}\"", abc[i]);
                }
                abc = abc.ToList();
                q = string.Format(q, string.Join(",", abc));
                HttpClient http = new HttpClient();
                Dictionary<string, string> form = new Dictionary<string, string>();
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                        break;
                    default:
                        break;
                }
                form.Add("id_us", z.ep_id);
                form.Add("list_role", q.ToString());
                HttpResponseMessage respon = await http.PostAsync("https://api.timviec365.vn/api/qlc/permission/add", new FormUrlEncodedContent(form));
                API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true && api != null)
                {
                    if (Success != null) Success();
                    var c = new PopUp.Notify1();
                    c.Type = PopUp.Notify1.NotifyType.Success;
                    c.Message = "Cập nhật thành công";
                    Main.ShowPopup(c);
                }
            }

        }
    }
}
