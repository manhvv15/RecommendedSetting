using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_Shift_manage
{
    /// <summary>
    /// Interaction logic for Changed_Delete.xaml
    /// </summary>
    public partial class Changed_Delete : Page
    {
        public Changed_Delete(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        public MainWindow Main;
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Add_Shift));
        private async void Delete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HttpClient http = new HttpClient();
                string comid = "";
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        comid = Main.APIStaff.data.data.user_info.com_id;
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        comid = Main.APICompany.data.data.user_info.com_id;
                        break;
                    default:
                        break;
                }
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("shift_id", "");
                form.Add("com_id", comid);
                HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/shift/delete", new FormUrlEncodedContent(form));
                API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    if (Success != null) Success();
                    Main.ClosePopup();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
