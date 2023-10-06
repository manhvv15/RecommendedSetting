using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_CauTrucPhongBan
{
    /// <summary>
    /// Interaction logic for Add_More_PB.xaml
    /// </summary>
    public partial class Add_More_PB : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Add_More_PB(MainWindow main, string comId)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ComId = comId;
        }
        public Add_More_PB(MainWindow main, Item_List_PhongBan Pb)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Info = Pb;
            Type = EditTypes.edit;
            
            tbDep.Text = Info.dep_name;
        }
        public MainWindow Main;
        private string ComId = "";
        public Item_List_PhongBan Info;
        public enum EditTypes { add, edit }
        private EditTypes _Type;
        public EditTypes Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged(); }
        }
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Add_More_PB));
        private void ExitPopUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }

        private async void AddDep_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            tblDepValidate.Text = "";
            if (string.IsNullOrEmpty(tbDep.Text))
            {
                allow = false;
                tblDepValidate.Text = "Vui lòng nhập tên phòng ban";
            }
            if (allow)
            {
                try
                {
                    
                    switch (Type)
                    {
                        case EditTypes.add:
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
                            form.Add("dep_name", tbDep.Text);
                            form.Add("com_id", ComId);
                            HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/department/create", new FormUrlEncodedContent(form));
                            API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (api.data != null && api.data.result == true)
                            {
                                if (Success != null) Success();
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Success;
                                x.Message = "Thêm Phòng Ban Thành Công";
                                Main.ShowPopup(x);
                            }
                            else
                            {
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Error;
                                x.Message = "Cập nhật Phòng Ban Thất bại";
                                Main.ShowPopup(x);
                            }
                            break;
                        case EditTypes.edit:
                            HttpClient http1 = new HttpClient();
                            switch (Main.Type)
                            {
                                case 1:
                                    http1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                    break;
                                case 2:
                                    http1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                    break;
                                default:
                                    break;
                            }
                            Dictionary<string, string> form1 = new Dictionary<string, string>();
                            form1.Add("dep_name", tbDep.Text);
                            form1.Add("dep_id", Info.dep_id);
                            form1.Add("com_id", Info.com_id);
                            HttpResponseMessage respon1 = await http1.PostAsync("http://210.245.108.202:3000/api/qlc/department/edit", new FormUrlEncodedContent(form1));
                            API_Respon api1 = JsonConvert.DeserializeObject<API_Respon>(respon1.Content.ReadAsStringAsync().Result);
                            if (api1.data != null && api1.data.result == true)
                            {
                                if (Success != null) Success();
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Success;
                                x.Message = "Cập nhật Phòng Ban Thành Công";
                                Main.ShowPopup(x);
                            }
                            else
                            {
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Error;
                                x.Message = "Cập nhật Phòng Ban Thất bại";
                                Main.ShowPopup(x);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
