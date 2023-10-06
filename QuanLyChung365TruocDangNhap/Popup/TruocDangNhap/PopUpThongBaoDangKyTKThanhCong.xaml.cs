using Newtonsoft.Json;
using RestSharp;
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

namespace QuanLyChung365TruocDangNhap.Popup.TruocDangNhap
{
    /// <summary>
    /// Interaction logic for PopUpThongBaoDangKyTKThanhCong.xaml
    /// </summary>
    public partial class PopUpThongBaoDangKyTKThanhCong : UserControl
    {
        private string Tk = "";
        private string Mk = "";
        private frmMain frmMainW;
        public PopUpThongBaoDangKyTKThanhCong(string tk, string mk, frmMain frm)
        {
            InitializeComponent();
            Tk = tk;
            Mk = mk;
            frmMainW = frm;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/employee/login")))
                {
                    RestRequest request = new RestRequest();
                    request.Method = Method.Post;
                    request.AlwaysMultipartFormData = true;

                    request.AddParameter("account", Tk);
                    request.AddParameter("password", Mk);
                    request.AddParameter("type", "1");
                    RestResponse resAlbum = restclient.Execute(request);
                    var b = resAlbum.Content;
                    Popup.OOP.Login.clsLogin.Root receivedInfo = JsonConvert.DeserializeObject<Popup.OOP.Login.clsLogin.Root>(b);
                    if (receivedInfo.data != null)
                    {
                        if (receivedInfo.data.data.type == "1")
                        {
                            frmMainW.textThongBao.Visibility = Visibility.Collapsed;
                            frmMainW.pnlMain.Visibility = Visibility.Visible;
                            frmMainW.btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                            frmMainW.btnCollapseSizebar.Visibility = Visibility.Visible;
                            frmMainW.pnlFullSizebar.Visibility = Visibility.Visible;
                            frmMainW.pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                            frmMainW.pnlDangKy.Visibility = Visibility.Collapsed;
                            frmMainW.docListBtn.Visibility = Visibility.Collapsed;
                            frmMainW.btnShowSPN.Visibility = Visibility.Visible;
                            frmMainW.IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                            frmMainW.textUserName.Text = receivedInfo.data.data.user_info.com_name;
                            //LoadTokenTL();
                            if (frmMainW.chkLuuTKMK.IsChecked == true)
                            {
                                Properties.Settings.Default.User = frmMainW.tb_TaiKhoanDangNhap.Text;
                                Properties.Settings.Default.Pass = frmMainW.tb_MatKhauGo.Password;
                                Properties.Settings.Default.Check = "1";
                                Properties.Settings.Default.Save();
                            }
                            else
                            {
                                Properties.Settings.Default.User = "";
                                Properties.Settings.Default.Pass = "";
                                Properties.Settings.Default.Check = "0";
                                Properties.Settings.Default.Save();
                            }
                        }
                        else if (receivedInfo.data.data.type == "2")
                        {
                            frmMainW.textThongBao.Visibility = Visibility.Collapsed;
                            frmMainW.pnlMain.Visibility = Visibility.Visible;
                            frmMainW.btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                            frmMainW.btnCollapseSizebar.Visibility = Visibility.Visible;
                            frmMainW.pnlFullSizebar.Visibility = Visibility.Visible;
                            frmMainW.pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                            frmMainW.pnlDangKy.Visibility = Visibility.Collapsed;
                            frmMainW.docListBtn.Visibility = Visibility.Collapsed;
                            frmMainW.btnShowSPN.Visibility = Visibility.Visible;
                            frmMainW.IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                            frmMainW.textUserName.Text = receivedInfo.data.data.user_info.com_name;
                            if (receivedInfo.data.data.user_info.com_logo != null)
                            {
                                var img = new Uri("https://chamcong.24hpay.vn/upload/employee/" + receivedInfo.data.data.user_info.com_logo);
                                BitmapImage bm = new BitmapImage(img);
                                frmMainW.ImgAc.ImageSource = bm;
                            }

                            if (frmMainW.chkLuuTKMK.IsChecked == true)
                            {
                                Properties.Settings.Default.UserEp = frmMainW.tb_TaiKhoanDangNhap.Text;
                                Properties.Settings.Default.PassEp = frmMainW.tb_MatKhauGo.Password;
                                Properties.Settings.Default.Check = "1";
                                Properties.Settings.Default.Save();
                            }
                            else
                            {
                                Properties.Settings.Default.UserEp = "";
                                Properties.Settings.Default.PassEp = "";
                                Properties.Settings.Default.Check = "0";
                                Properties.Settings.Default.Save();
                            }
                            Properties.Settings.Default.Token = receivedInfo.data.data.access_token;

                        }

                    }
                    else
                    {
                        frmMainW.textThongBao.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {
                frmMainW.textThongBao.Visibility = Visibility.Visible;
            }
            this.Visibility = Visibility.Collapsed;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/employee/login")))
                {
                    RestRequest request = new RestRequest();
                    request.Method = Method.Post;
                    request.AlwaysMultipartFormData = true;

                    request.AddParameter("account", frmMainW.tb_TaiKhoanDangNhap.Text);
                    request.AddParameter("password", frmMainW.tb_MatKhauGo.Password);
                    request.AddParameter("type", "1");
                    RestResponse resAlbum = restclient.Execute(request);
                    var b = resAlbum.Content;
                    Popup.OOP.Login.clsLogin.Root receivedInfo = JsonConvert.DeserializeObject<Popup.OOP.Login.clsLogin.Root>(b);
                    if (receivedInfo.data != null)
                    {
                        if (receivedInfo.data.data.type == "1")
                        {
                            if (frmMainW.textLoaiTK.Text == "TÀI KHOẢN CÔNG TY")
                            {
                                frmMainW.textThongBao.Visibility = Visibility.Collapsed;
                                frmMainW.pnlMain.Visibility = Visibility.Visible;
                                frmMainW.btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                                frmMainW.btnCollapseSizebar.Visibility = Visibility.Visible;
                                frmMainW.pnlFullSizebar.Visibility = Visibility.Visible;
                                frmMainW.pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                                frmMainW.pnlDangKy.Visibility = Visibility.Collapsed;
                                frmMainW.docListBtn.Visibility = Visibility.Collapsed;
                                frmMainW.btnShowSPN.Visibility = Visibility.Visible;
                                frmMainW.IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                                frmMainW.textUserName.Text = receivedInfo.data.data.user_info.com_name;
                                //LoadTokenTL();
                                if (frmMainW.chkLuuTKMK.IsChecked == true)
                                {
                                    Properties.Settings.Default.User = frmMainW.tb_TaiKhoanDangNhap.Text;
                                    Properties.Settings.Default.Pass = frmMainW.tb_MatKhauGo.Password;
                                    Properties.Settings.Default.Check = "1";
                                    Properties.Settings.Default.Save();
                                }
                                else
                                {
                                    Properties.Settings.Default.User = "";
                                    Properties.Settings.Default.Pass = "";
                                    Properties.Settings.Default.Check = "0";
                                    Properties.Settings.Default.Save();
                                }
                                //Properties.Settings.Default.Token = receivedInfo.data.data.access_token;
                                //this.Hide();
                                //MainWindow main = new MainWindow(receivedInfo.data, this);
                                //main.Show();
                                //textThongBao.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                frmMainW.textThongBao.Visibility = Visibility.Visible;
                            }
                        }
                        else if (receivedInfo.data.data.type == "2")
                        {
                            if (frmMainW.textLoaiTK.Text == "TÀI KHOẢN NHÂN VIÊN")
                            {
                                frmMainW.textThongBao.Visibility = Visibility.Collapsed;
                                frmMainW.pnlMain.Visibility = Visibility.Visible;
                                frmMainW.btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                                frmMainW.btnCollapseSizebar.Visibility = Visibility.Visible;
                                frmMainW.pnlFullSizebar.Visibility = Visibility.Visible;
                                frmMainW.pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                                frmMainW.pnlDangKy.Visibility = Visibility.Collapsed;
                                frmMainW.docListBtn.Visibility = Visibility.Collapsed;
                                frmMainW.btnShowSPN.Visibility = Visibility.Visible;
                                frmMainW.IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                                frmMainW.textUserName.Text = receivedInfo.data.data.user_info.com_name;
                                if (receivedInfo.data.data.user_info.com_logo != null)
                                {
                                    var img = new Uri("https://chamcong.24hpay.vn/upload/employee/" + receivedInfo.data.data.user_info.com_logo);
                                    BitmapImage bm = new BitmapImage(img);
                                    frmMainW.ImgAc.ImageSource = bm;
                                }

                                if (frmMainW.chkLuuTKMK.IsChecked == true)
                                {
                                    Properties.Settings.Default.UserEp = frmMainW.tb_TaiKhoanDangNhap.Text;
                                    Properties.Settings.Default.PassEp = frmMainW.tb_MatKhauGo.Password;
                                    Properties.Settings.Default.Check = "1";
                                    Properties.Settings.Default.Save();
                                }
                                else
                                {
                                    Properties.Settings.Default.UserEp = "";
                                    Properties.Settings.Default.PassEp = "";
                                    Properties.Settings.Default.Check = "0";
                                    Properties.Settings.Default.Save();
                                }
                                Properties.Settings.Default.Token = receivedInfo.data.data.access_token;

                                //this.Hide();
                                //MainChamCong main = new MainChamCong(receivedInfo.data, this);
                                //main.Show();
                                //textThongBao.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                frmMainW.textThongBao.Visibility = Visibility.Visible;
                            }

                        }

                    }
                    else
                    {
                        frmMainW.textThongBao.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {
                frmMainW.textThongBao.Visibility = Visibility.Visible;
            }
            this.Visibility = Visibility.Collapsed;
        }
    }
}
