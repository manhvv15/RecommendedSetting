    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Send_Error.xaml
    /// </summary>
    public partial class Send_Error : Page,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Send_Error(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            this.DataContext = this;
        }
        public MainWindow Main;
        string linkFile = "";
        private async void SendErr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tblImgValidate.Text = "";
            tblInputErrValidate.Text = "";
            bool allow = true;
            if (string.IsNullOrEmpty(tbInputErr.Text))
            {
                allow = false;
                tblInputErrValidate.Text = "Vui lòng nhập vào lỗi của bạn";
            }
            if ((ImgErr.Source) == null)
            {
                allow = false;
                tblInputErrValidate.Text = "Vui lòng chọn ảnh lỗi";
            }
            if (allow)
            {
                try
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
                    var content = new MultipartFormDataContent();
                    FileInfo file = new FileInfo(linkFile);
                    content.Add(new StringContent("App Winform"), "from_source");
                    content.Add(new StringContent(tbInputErr.Text), "detail_error");
                    content.Add(new StringContent(System.Environment.MachineName), "device_id");
                    content.Add(new StreamContent(new FileStream(file.FullName, FileMode.Open, FileAccess.Read)), "gallery_image_error", file.Name);
                    HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/ReportError", content);
                    API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null && api.data.result == true)
                    {
                        var pop = new Views.PopUp.PopUp_SendError.Noti_Error(Main);
                        Main.ShowPopup(pop);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        private void ChooseImgErr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            if (op.ShowDialog()==true)
            {
                ImgErr.Source = op.FileName.GetThumbnail(300);
                linkFile = op.FileName;
            }
        }
    }
}
