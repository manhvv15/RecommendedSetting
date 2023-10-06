using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.TransportPopups
{
    /// <summary>
    /// Interaction logic for ChooseTransportProcess.xaml
    /// </summary>
    public partial class ChooseTransportProcess : UserControl
    {
        List<Items> listOption;

        // deligate event hide popups
        public delegate void HidePopup(int process_id);
        public static event HidePopup hidePopup;

        // deligate event hide popups
        public delegate void HidePopup1(object obj);
        public static event HidePopup1 hidePopup1;

        public ChooseTransportProcess(List<Items> listOption)
        {
            InitializeComponent();
            this.listOption = listOption;
            cbxProcess.ItemsSource = this.listOption;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void ShowTransportProcess(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
            {
                //hidePopup1("");
                //System.Threading.Thread.Sleep(300);
                string id_process = cbxProcess.SelectedValue.ToString();
                hidePopup(Convert.ToInt32(id_process));
            }
        }

        private bool ValidateForm()
        {
            if(cbxProcess.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giai đoạn chuyển.");
                return false;
            }
            return true;
        }
    }
}
