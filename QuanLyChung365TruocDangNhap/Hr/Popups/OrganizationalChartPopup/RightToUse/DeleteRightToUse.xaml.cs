using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.OrganizationalChartPopup.RightToUse
{
    /// <summary>
    /// Interaction logic for DeleteRightToUse.xaml
    /// </summary>
    public partial class DeleteRightToUse : UserControl
    {
        string token;
        string id;
        string ep_id;


        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        public DeleteRightToUse(string token, string id, string ep_id)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.ep_id = ep_id;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void DeletePopup(object sender, MouseButtonEventArgs e)
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
                string api = "http://210.245.108.202:3006/api/hr/organizationalStructure/deleteSignature";

                httpRequestMessage.RequestUri = new Uri(api);

                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("empId", ep_id));
                //parameters.Add(new KeyValuePair<string, string>("ep_signature", "0"));

                //string ep_signature = "";
                //if (ep_signature == "0")
                //{

                //}
                //else
                //{
                //    parameters.Add(new KeyValuePair<string, string>("ep_signature", "1"));
                //    //getImageSignature();
                //}

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                DeleteEntity result = JsonConvert.DeserializeObject<DeleteEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show(result.data.message);
                }



            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra!");
            }

        }


        //private async void getImageSignature()
        //{
        //    try
        //    {
        //        var httpClient = new HttpClient();
        //        var httpRequestMessage = new HttpRequestMessage();
        //        httpRequestMessage.Method = HttpMethod.Post;
        //        string api = "https://phanmemnhansu.timviec365.vn/getImageSignature";

        //        httpRequestMessage.RequestUri = new Uri(api);
        //        var parameters = new List<KeyValuePair<string, string>>();

        //        parameters.Add(new KeyValuePair<string, string>("token", token));
        //        parameters.Add(new KeyValuePair<string, string>("id_ep", ep_id));


        //        // Thiết lập Content
        //        var content = new FormUrlEncodedContent(parameters);
        //        httpRequestMessage.Content = content;

        //        // Thực hiện Post
        //        var response = await httpClient.SendAsync(httpRequestMessage);

        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        getImageEntity result = JsonConvert.DeserializeObject<getImageEntity>(responseContent);

        //        if (result.result)
        //        {
        //            hidePopup(1);
        //            MessageBox.Show(result.success.message);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Có lỗi xảy ra!");
        //    }
        //}
    }
}
