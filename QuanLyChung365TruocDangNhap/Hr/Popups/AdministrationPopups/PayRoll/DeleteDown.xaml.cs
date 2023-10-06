using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.PayRoll
{
    /// <summary>
    /// Interaction logic for DeleteDown.xaml
    /// </summary>
    public partial class DeleteDown : UserControl
    {
        public string ep_id;
        public string id_com;
        public string current_position;
        public string current_dep_id;
        public string created_at;
        public string decision_id;
        public string note;
        public string type;

        public string Authorization;
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        public DeleteDown(string Authorization, string ep_id, string id_com = "", string current_position = "", string current_dep_id = "", string created_at = "", string decision_id = "", string note = "", string type = "")
        {
            InitializeComponent();
            this.Authorization = Authorization;
            this.ep_id = ep_id;
            this.id_com = id_com;
            this.current_position = current_position;
            this.current_dep_id = current_dep_id;
            this.created_at = created_at;
            this.decision_id = decision_id;
            this.note = note;
            this.type = type;
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
                string api = "http://210.245.108.202:3006/api/hr/personalChange/deleteQuitJob";

                httpRequestMessage.RequestUri = new Uri(api);

                httpRequestMessage.Headers.Add("Authorization", "Bearer " + Authorization);

                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("ep_id", ep_id));
                /*parameters.Add(new KeyValuePair<string, string>("id_com", id_com));
                parameters.Add(new KeyValuePair<string, string>("current_position", current_position));
                parameters.Add(new KeyValuePair<string, string>("current_dep_id", current_dep_id));
                parameters.Add(new KeyValuePair<string, string>("created_at", created_at));
                parameters.Add(new KeyValuePair<string, string>("decision_id", decision_id));
                parameters.Add(new KeyValuePair<string, string>("note", note));
                parameters.Add(new KeyValuePair<string, string>("type", type));*/

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                SuccessDelete result = JsonConvert.DeserializeObject<SuccessDelete>(responseContent);
                if (result.data.result)
                {
                    MessageBox.Show("Xóa thành công!");
                    hidePopup(1);

                }
                //else
                //{
                //    MessageBox.Show("Xóa thất bại, vui lòng thử lại!");
                //}

            }
            catch
            {
                //MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}
