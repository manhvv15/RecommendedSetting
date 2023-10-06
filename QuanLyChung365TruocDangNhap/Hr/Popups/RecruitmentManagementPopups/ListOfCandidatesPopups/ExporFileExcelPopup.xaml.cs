using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using OfficeOpenXml;
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
    /// Interaction logic for ExporFileExcelPopup.xaml
    /// </summary>
    public partial class ExporFileExcelPopup : UserControl
    {
        string token;
        string process_id;

        List<Items> listItem = new List<Items>();
        //Danh sách nhận hồ sơ
        List<DataEntity> listProfile = new List<DataEntity>();
        //Danh sách nhận việc
        List<DataEntity> listGetJob = new List<DataEntity>();
        //Danh sách trượt
        List<DataEntity> listFailJob = new List<DataEntity>();
        //Danh sách hủy
        List<DataEntity> listCancelJob = new List<DataEntity>();
        //Danh sách kí hợp đồng
        List<DataEntity> listContactJob = new List<DataEntity>();

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public ExporFileExcelPopup(string token, List<Items> listItem, List<DataEntity> listProfile, List<DataEntity> listGetJob, List<DataEntity> listFailJob, List<DataEntity> listCancelJob, List<DataEntity> listContactJob)
        {
            InitializeComponent();
            this.token = token;
            this.listItem = listItem;
            cbxstage.ItemsSource = this.listItem;
            this.listProfile = listProfile;
            this.listGetJob = listGetJob;
            this.listFailJob = listFailJob;
            this.listCancelJob = listCancelJob;
            this.listContactJob = listContactJob;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private bool ValidateExcel()
        {
            if (cbxstage.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn giai đoạn!");
                return false;
            }

            return true;
        }

        private void clickExcel(object sender, MouseButtonEventArgs e)
        {
            if (ValidateExcel())
            {
                Microsoft.Win32.SaveFileDialog sv = new Microsoft.Win32.SaveFileDialog();
                sv.Filter = "Microsoft Excel Worksheet | *.xlsx";
                sv.FileName = "DataCandidate";
                if (sv.ShowDialog() == true)
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    var file = new FileInfo(sv.FileName);
                    if (cbxstage.SelectedIndex.ToString() == "1")
                    {
                        ExportExcel<DataEntity>(listGetJob, file).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            //var x = new Popups.ConfirmBox();
                            //x.ConfirmTitle = "Xuất file excel";
                            //x.Message = "Xuất file excel thành công, bạn có muốn mở file không?";
                            //x.Accept = () =>
                            //{
                            //    System.Diagnostics.Process.Start(file.FullName);
                            //};
                            MessageBox.Show("Xuất file excel thành công!");
                            hidePopup(1);
                        }));
                    }
                    else if (cbxstage.SelectedIndex.ToString() == "2")
                    {
                        ExportExcel<DataEntity>(listFailJob, file).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            //var x = new Popups.ConfirmBox();
                            //x.ConfirmTitle = "Xuất file excel";
                            //x.Message = "Xuất file excel thành công, bạn có muốn mở file không?";
                            //x.Accept = () =>
                            //{
                            //    System.Diagnostics.Process.Start(file.FullName);
                            //};
                            MessageBox.Show("Xuất file excel thành công!");
                            hidePopup(1);
                        }));
                    }
                    else if(cbxstage.SelectedIndex.ToString() == "3")
                    {
                        ExportExcel<DataEntity>(listCancelJob, file).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            //var x = new Popups.ConfirmBox();
                            //x.ConfirmTitle = "Xuất file excel";
                            //x.Message = "Xuất file excel thành công, bạn có muốn mở file không?";
                            //x.Accept = () =>
                            //{
                            //    System.Diagnostics.Process.Start(file.FullName);
                            //};
                            MessageBox.Show("Xuất file excel thành công!");
                            hidePopup(1);
                        }));
                    }
                    else if(cbxstage.SelectedIndex.ToString() == "4")
                    {

                        ExportExcel<DataEntity>(listContactJob, file).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            //var x = new Popups.ConfirmBox();
                            //x.ConfirmTitle = "Xuất file excel";
                            //x.Message = "Xuất file excel thành công, bạn có muốn mở file không?";
                            //x.Accept = () =>
                            //{
                            //    System.Diagnostics.Process.Start(file.FullName);
                            //};
                            MessageBox.Show("Xuất file excel thành công!");
                            hidePopup(1);
                        }));
                    }
                    else
                    {
                        ExportExcel<DataEntity>(listProfile, file).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            //var x = new Popups.ConfirmBox();
                            //x.ConfirmTitle = "Xuất file excel";
                            //x.Message = "Xuất file excel thành công, bạn có muốn mở file không?";
                            //x.Accept = () =>
                            //{
                            //    System.Diagnostics.Process.Start(file.FullName);
                            //};
                            MessageBox.Show("Xuất file excel thành công!");
                            hidePopup(1);
                        }));
                    }

                }
            }
        }

        private async Task ExportExcel<T>(List<T> data, FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("Danh sách ứng viên theo giai đoạn");

                //ws.Cells["A1"].Value = "LỊCH SỬ ĐIỂM DANH";
                ws.Cells["A1:E1"].Merge = true;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(1).Style.Font.Bold = true;
                ws.Row(1).Style.Font.Size = 13;

                ws.Cells["A2:E2"].Merge = true;
                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;
                ws.Row(2).Style.Font.Size = 13;

                ws.Cells["A3"].Offset(0, 0).Value = "Mã Ứng Viên";
                ws.Cells["A3"].Offset(0, 1).Value = "Id";
                ws.Cells["A3"].Offset(0, 2).Value = "Họ và tên";
                ws.Cells["A3"].Offset(0, 3).Value = "Trường học";
                ws.Cells["A3"].Offset(0, 4).Value = "Nguồn ứng viên";
                ws.Cells["A3"].Offset(0, 5).Value = "Ngày/Tháng/Năm";
                //ws.Cells["A3"].Offset(0, 6).Value = "";
                //ws.Cells["A3"].Offset(0, 7).Value = "";
                //ws.Cells["A3"].Offset(0, 8).Value = "";
                //ws.Cells["A3"].Offset(0, 9).Value = "";
                //ws.Cells["A3"].Offset(0, 10).Value = "";
                //ws.Cells["A3"].Offset(0, 11).Value = "";
                //ws.Cells["A3"].Offset(0, 12).Value = "";
                //ws.Cells["A3"].Offset(0, 13).Value = "";
                //ws.Cells["A3"].Offset(0, 14).Value = "";
                //ws.Cells["A3"].Offset(0, 15).Value = "";
                //ws.Cells["A3"].Offset(0, 16).Value = "";
                //ws.Cells["A3"].Offset(0, 17).Value = "";
                //ws.Cells["A3"].Offset(0, 18).Value = "";
                //ws.Cells["A3"].Offset(0, 19).Value = "";
                //ws.Cells["A3"].Offset(0, 20).Value = "";
                ws.Cells["A3"].Offset(0, 21).Value = "Giới tính";
                //ws.Cells["A3"].Offset(0, 22).Value = "";
                //ws.Cells["A3"].Offset(0, 23).Value = "";
                //ws.Cells["A3"].Offset(0, 24).Value = "";
                //ws.Cells["A3"].Offset(0, 25).Value = "";
                //ws.Cells["A3"].Offset(0, 26).Value = "";
                //ws.Cells["A3"].Offset(0, 27).Value = "";
                //ws.Cells["A3"].Offset(0, 28).Value = "";
                //ws.Cells["A3"].Offset(0, 29).Value = "";
                //ws.Cells["A3"].Offset(0, 30).Value = "";
                //ws.Cells["A3"].Offset(0, 31).Value = "";
                //ws.Cells["A3"].Offset(0, 32).Value = "";
                //ws.Cells["A3"].Offset(0, 33).Value = "";
                ws.Cells["A3"].Offset(0, 34).Value = "Nhân viên tuyển dụng";
                //ws.Cells["A3"].Offset(0, 35).Value = "";
                //ws.Cells["A3"].Offset(0, 36).Value = "";
                //ws.Cells["A3"].Offset(0, 37).Value = "";
                //ws.Cells["A3"].Offset(0, 38).Value = "";
                ws.Cells["A3"].Offset(0, 39).Value = "Vị trí";
     

                ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(3).Style.Font.Bold = true;
                ws.Row(3).Style.Font.Size = 13;

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Row(i + 4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(i + 4).Style.Font.Size = 13;
                }

                if(data == null)
                {
                    ws.Cells["A3"].Value = "Không có dữ liệu";
                }
                else
                {
                    var range = ws.Cells["A4"].LoadFromCollection(data);
                    ws.Cells["A3:" + range.End.Address].AutoFitColumns();
                }
                package.Save();
            }
        }
    }
}
