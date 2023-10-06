using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Appointment_Planning
{
    /// <summary>
    /// Interaction logic for DeleteAppointmentPlanning.xaml
    /// </summary>
    public partial class DeleteAppointmentPlanning : UserControl
    {
        string authorization;
        string ep_id;
        string id_com;
        string ep_name;

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        public DeleteAppointmentPlanning(string authorization, string ep_id, string ep_name, string id_com)
        {
            InitializeComponent();
            this.authorization = authorization;
            this.ep_id = ep_id;
            this.ep_name = ep_name;
            this.id_com = id_com;
            txtName.Text = ep_name;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void ClickDelete(object sender, MouseButtonEventArgs e)
        {
            DeleteData();
        }

        private async void DeleteData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3006/api/hr/personalChange/deleteAppoint";

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpRequestMessage.Headers.Add("Authorization", "Bearer " + authorization);
                parameters.Add(new KeyValuePair<string, string>("ep_id", ep_id));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                SuccessDelete result = JsonConvert.DeserializeObject<SuccessDelete>(responseContent);
                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Xóa thành công!");

                }
                else
                {
                    MessageBox.Show("Xóa thất bại, vui lòng thử lại!");
                }

            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}
