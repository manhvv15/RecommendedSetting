using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.TranningAndDeveloper
{
    /// <summary>
    /// Interaction logic for AddNewLocattion.xaml
    /// </summary>
    public partial class AddNewLocattion : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        string token;
        string file = "";
        public AddNewLocattion(string token)
        {
            InitializeComponent();
            this.token = token;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void AddNewLocationPopup(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                AddNewDataLocation();
        }
        private async void AddNewDataLocation()
        {
            try
            {

                var httpClient = new HttpClient();
                var multiForm = new MultipartFormDataContent();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                multiForm.Add(new StringContent(tbTenViTri.Text), "name");
                multiForm.Add(new StringContent(tbBoPhanTrucThuoc.Text), "depName");
                multiForm.Add(new StringContent(StringFromRichTextBox(tbMotaCongViec)), "des");
                multiForm.Add(new StringContent(StringFromRichTextBox(tbYeuCauCongViec)), "jobRequire");
                //add file and directly upload it
                if (file != "")
                {
                    FileStream fs = File.OpenRead(file);
                    multiForm.Add(new StreamContent(fs), "roadMap", System.IO.Path.GetFileName(file));
                }
                // send request to API
                var url = APIs.API.addJobDescription;
                var response = await httpClient.PostAsync(url, multiForm);
                var responseContent = await response.Content.ReadAsStringAsync();
                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);
                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Thêm mới vị trí thành công!");
                }
                else
                {
                    MessageBox.Show(result.error.message);
                }
            }
            catch (Exception)
            {

                //    //MessageBox.Show("Hihi");
            }
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            //dlg.DefaultExt = ".png";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.SafeFileName;
                file = dlg.FileName;
                txtEditor.Text = filename;
            }
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }

        private bool ValidateForm()
        {
            if (tbTenViTri.Text.Trim() == "")
            {
                MessageBox.Show("Vị trí không được để trống.");
                return false;
            }

            if (tbBoPhanTrucThuoc.Text.Trim() == "")
            {
                MessageBox.Show("Bộ phận trực thuộc không được để trống.");
                return false;
            }

            if (StringFromRichTextBox(tbMotaCongViec).Trim() == "")
            {
                MessageBox.Show("Mô tả công việc không được để trống.");
                return false;
            }

            if (StringFromRichTextBox(tbYeuCauCongViec).Trim() == "")
            {
                MessageBox.Show("Yêu cầu công việc không được để trống.");
                return false;
            }

            return true;
        }
        //private void DeleteFileCV(object sender, MouseButtonEventArgs e)
        //{
        //    txtEditor.Text = "";
        //    file = "";
        //    btnDeleteCV.Visibility = Visibility.Collapsed;
        //}
    }
}
