using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups
{
    /// <summary>
    /// Interaction logic for CvCandidatePopup.xaml
    /// </summary>
    public partial class CvCandidatePopup : UserControl
    {
        // deligate event hide popups

        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        string token;
        string id;
        string url_cv;
        string cv;
        CandidateDetailEntity candidateDetailEntity;

        public CvCandidatePopup(string token, string id, string url_cv,string cv)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.url_cv = url_cv;
            this.cv = cv;

            DisplayCVCandidate();
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e) 
        {
            hidePopup(0);
        }

        public void DisplayCVCandidate()
        {
            //Initialize PdfViewerControl
            //PdfViewerControl pdfViewer = new PdfViewerControl();
            ////Load the input PDF file.
            //PdfLoadedDocument loadedDocument = new PdfLoadedDocument(@"C:\Users\Nguyen Ngoc Nhat\Downloads\" + cv);
            //pdfViewer.Load(loadedDocument);
            //BitmapSource[] listVC = pdfViewer.ExportAsImage(0, loadedDocument.Pages.Count - 1);
            //listImg.ItemsSource = listVC;
            //imgBack.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), @"C:\Users\Nguyen Ngoc Nhat\Downloads\" + ".png"));
            //loadedDocument.Dispose();
            //loadedDocument = null;
        }

    }
}
