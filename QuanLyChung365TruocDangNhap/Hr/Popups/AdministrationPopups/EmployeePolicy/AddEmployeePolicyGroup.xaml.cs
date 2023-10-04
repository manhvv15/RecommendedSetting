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
    /// Interaction logic for AddEmployeePolicyGroup.xaml
    /// </summary>
    public partial class AddEmployeePolicyGroup : UserControl
    {
        string file_path = "";
        string token;

        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public AddEmployeePolicyGroup(string token)
        {
            InitializeComponent();
            this.token = token;
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private async void AddRegulationGroup()
        {
            try
            {
                var client = new HttpClient();
                var multiForm = new MultipartFormDataContent();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                multiForm.Add(new StringContent(tbName.Text), "name");
                string date = dpTimeStart.SelectedDate.Value.ToString("yyyy-MM-dd");
                multiForm.Add(new StringContent(date), "time_start");
                multiForm.Add(new StringContent(tbSupervisor.Text), "supervisor_name");
                multiForm.Add(new StringContent(StringFromRichTextBox(rtbDescription)), "description");

                //add file and directly upload it
                if (file_path != "")
                {
                    FileStream fs = File.OpenRead(file_path);
                    multiForm.Add(new StreamContent(fs), "employeePolicy", Path.GetFileName(file_path));
                }

                // send request to API
                var url = APIs.API.add_employee_policy_group;
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

        private void AddRegulationGroup(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                AddRegulationGroup();
        }

        private bool ValidateForm()
        {
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên chính sách không được để trống!");
                return false;
            }

            if (dpTimeStart.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền thời gian hiệu lực.");
                return false;
            }

            if (tbSupervisor.Text.Trim() == "")
            {
                MessageBox.Show("Người giám sát không được để trống.");
                return false;
            }

            if (StringFromRichTextBox(rtbDescription).Trim() == "")
            {
                MessageBox.Show("Mô tả không được để trống.");
                return false;
            }

            return true;
        }
    }
}
