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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Company_Child_Manage.xaml
    /// </summary>
    public partial class Company_Child_Manage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Company_Child_Manage(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            this.DataContext = this;

            getData().GetAwaiter();
        }
        public MainWindow Main;
        private List<Item_Data_Com_Child_Manage> _List_ComChild;
        public List<Item_Data_Com_Child_Manage> List_ComChild
        {
            get { return _List_ComChild; }
            set
            {
                _List_ComChild = value;
                OnPropertyChanged();
            }
        }
        //get data table com child from api
        private async Task getData()
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

            var respon = await httpClient.PostAsync(apilink, new FormUrlEncodedContent(form));

            API_Com_Child_Manage api = JsonConvert.DeserializeObject<API_Com_Child_Manage>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null)
            {
                api.data.items.Add(new Item_Data_Com_Child_Manage() { com_id = Main.APICompany.data.data.user_info.com_id, com_name = Main.APICompany.data.data.user_info.com_name, com_logo = Main.APICompany.data.data.user_info.com_logo, com_address = Main.APICompany.data.data.user_info.com_address, com_email = Main.APICompany.data.data.user_info.com_email, com_parent_id = "", com_phone = Main.APICompany.data.data.user_info.com_phone, com_role_id = Main.APICompany.data.data.user_info.com_role_id });
                List_ComChild = api.data.items;
            }
        }
        //Scroll when wheel mouse
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        //add company
        private void EditComChild_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var z = row.Item as Item_Data_Com_Child_Manage;

                    if (z != null)
                    {
                        var x = new PopUp.PopUp_Company_Child.Add_More_Company(Main, z);
                        Main.ShowPopup(x);

                        x.Success = () =>
                        {
                            getData();
                        };
                    }
                    break;
                }
        }
        private void Add_Com_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var x = new PopUp.PopUp_Company_Child.Add_More_Company(Main);
            Main.ShowPopup(x);

            x.Success = () =>
            {
                getData();
            };
        }

        
    }
}
