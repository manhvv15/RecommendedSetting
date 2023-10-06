using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.EmployeePolicy
{
    /// <summary>
    /// Interaction logic for AddEmployeePolicy.xaml
    /// </summary>
    public partial class AddEmployeePolicy : UserControl
    {
        string token;
        string file_path = "";
        Dictionary<string, string> listAllRegulation;

        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public AddEmployeePolicy(string token, Dictionary<string, string> listAllRegulation)
        {
            InitializeComponent();
            this.token = token;
            this.listAllRegulation = listAllRegulation;
            cbxpolicy.ItemsSource = listAllRegulation;

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void ChooseCV(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.Filter = "PDF Files (*.pdf)|*.pdf|XLSX Files (*.xlsx)|*.xlsx|XLS Files (*.xls)|*.xls|DOC Files (*.doc)|*.doc|DOCX Files (*.docx)|*.docx|PPT Files (*.ppt)|*.ppt|PPTX Files (*.pptx)|*.pptx|TXT Files (*.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.SafeFileName;
                file_path = dlg.FileName;
                tbFileName.Text = filename;
                btnDeleteCV.Visibility = Visibility.Visible;
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

        private void DeleteFileCV(object sender, MouseButtonEventArgs e)
        {
            tbFileName.Text = "";
            file_path = "";
            btnDeleteCV.Visibility = Visibility.Collapsed;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void AddPolicyHandler(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                AddPolicyHandler();
        }

        private async void AddPolicyHandler()
        {
            try
            {
                var client = new HttpClient();
                var multiForm = new MultipartFormDataContent();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                multiForm.Add(new StringContent(tbName.Text), "name");
                multiForm.Add(new StringContent(dpTimeStart.SelectedDate.Value.ToString("yyyy-MM-dd")), "time_start");
                multiForm.Add(new StringContent(tbSupervisor.Text), "supervisor_name");
                multiForm.Add(new StringContent(tbApllyFor.Text), "apply_for");
                multiForm.Add(new StringContent(StringFromRichTextBox(rtbDescription)), "content");
                if (cbxpolicy.SelectedIndex > -1)
                {
                    multiForm.Add(new StringContent(cbxpolicy.SelectedValue.ToString()), "employe_policy_id");
                }

                //add file and directly upload it
                if (file_path != "")
                {
                    FileStream fs = File.OpenRead(file_path);
                    multiForm.Add(new StreamContent(fs), "file", Path.GetFileName(file_path));
                }

                // send request to API
                var url = APIs.API.add_employee_policy;
                var response = await client.PostAsync(url, multiForm);
                var responseContent = await response.Content.ReadAsStringAsync();
                Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity>(responseContent);
                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Thêm mới thành công!");
                }
                else
                {
                    MessageBox.Show("Thêm mới không thành công!");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    return "";
                }
            }

        }

        private bool ValidateForm()
        {
            if(tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên chính sách không được để trống!");
                return false;
            }

            if(cbxpolicy.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhóm chính sách.");
                return false;
            }

            if(dpTimeStart.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền thời gian hiệu lực.");
                return false;
            }

            if(tbSupervisor.Text.Trim() == "")
            {
                MessageBox.Show("Người giám sát không được để trống.");
                return false;
            }

            if(tbApllyFor.Text.Trim() == "")
            {
                MessageBox.Show("Đối tượng thi hành không được để trống.");
                return false;
            }

            if(StringFromRichTextBox(rtbDescription).Trim() == "")
            {
                MessageBox.Show("Nội dung chính sách không được để trống.");
                return false;
            }

            return true;
        }
        private void cbxpolicy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxpolicy.SelectedIndex = -1;
                string textSearch = cbxpolicy.Text;
                cbxpolicy.Items.Refresh();
                cbxpolicy.ItemsSource = listAllRegulation.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxpolicy_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxpolicy.SelectedIndex = -1;
            string textSearch = cbxpolicy.Text + e.Text;
            cbxpolicy.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxpolicy.Text = "";
                cbxpolicy.Items.Refresh();
                cbxpolicy.ItemsSource = listAllRegulation;
                cbxpolicy.SelectedIndex = -1;
            }
            else
            {
                cbxpolicy.ItemsSource = "";
                cbxpolicy.Items.Refresh();
                cbxpolicy.ItemsSource = listAllRegulation.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
