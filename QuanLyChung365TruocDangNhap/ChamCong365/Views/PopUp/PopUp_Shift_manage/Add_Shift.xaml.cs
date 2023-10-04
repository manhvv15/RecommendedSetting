using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_Shift_manage
{
    /// <summary>
    /// Interaction logic for Add_Shift.xaml
    /// </summary>
    public partial class Add_Shift : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Add_Shift(MainWindow main, string comid)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Comid = comid;
            Type = EditTypes.add;
        }
        public Add_Shift(MainWindow main, Shift_Manage_Item shift)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Type = EditTypes.edit;
            info = shift;
            tbShift.Text = info.shift_name;
            cbbTimeIn.Time = info.start_time;
            cbbTimeOut.Time = info.end_time;
            cbbTimeInMoiNhat.Time = info.start_time_latest;
            cbbTimeOutMuonNhat.Time = info.end_time_earliest;
            if (!string.IsNullOrEmpty(info.num_to_calculate)) cbbCongByShift.SelectedIndex = listShift.FindIndex(x =>x.Key==info.num_to_calculate);
            numtbCongByMonney.Value = int.Parse(info.num_to_money);
        }
        public string Comid;
        public class num_to_calculate
        {
            public string Key { get; set; }
            public string Name { get; set; }
        }
        public List<num_to_calculate> listShift { get; set; } = new List<num_to_calculate>
        {
            new num_to_calculate(){ Key="0.25", Name="0,25 công / 1 ca" },
            new num_to_calculate(){ Key="0.5", Name="0,5 công / 1 ca" },
            new num_to_calculate(){ Key="1", Name="1 công / 1 ca" },
            new num_to_calculate(){ Key="1.5", Name="1,5 công / 1 ca" },
            new num_to_calculate(){ Key="2", Name="2 công / 1 ca" },
            new num_to_calculate(){ Key="3", Name="3 công / 1 ca" },
            new num_to_calculate(){ Key="4", Name="4 công / 1 ca" },
        };

        private Shift_Manage_Item info;
        private bool _ShowTimeLimit;

        public bool ShowTimeLimit
        {
            get { return _ShowTimeLimit; }
            set { _ShowTimeLimit = value; OnPropertyChanged(); }
        }
        private bool _CongTheoGio = true;

        public bool CongTheoGio
        {
            get { return _CongTheoGio; }
            set { _CongTheoGio = value; OnPropertyChanged(); }
        }
        private bool _CongTheoCa;

        public bool CongTheoCa
        {
            get { return _CongTheoCa; }
            set { _CongTheoCa = value; OnPropertyChanged(); }
        }

        private bool _CongTheoTien;

        public bool CongTheoTien
        {
            get { return _CongTheoTien; }
            set { _CongTheoTien = value; OnPropertyChanged(); }
        }
        public enum CongType { time, shift, monney }
        private CongType _Choose;

        public CongType Choose
        {
            get { return _Choose; }
            set { _Choose = value; OnPropertyChanged(); }
        }

        public enum EditTypes { add, edit }
        private EditTypes _Type;
        public EditTypes Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged(); }
        }


        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Add_Shift));
        public MainWindow Main;
        private async void Add_Shift_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (string.IsNullOrEmpty(tbShift.Text))
            {
                allow = false;
                tblInputCaValidate.Text = "Vui lòng nhập tên ca";
            }
            if (string.IsNullOrEmpty(cbbTimeIn.Time))
            {
                allow = false;
                tblTimeInValidate.Text = "Vui long nhập giờ vào ca";
            }
            if (string.IsNullOrEmpty(cbbTimeOut.Time))
            {
                allow = false;
                tblTimeOutValidate.Text = "Vui lòng nhập giờ hết ca";
            }
            if (numtbCongByMonney.Value <= 0 && Choose == CongType.monney)
            {
                allow = false;
                tblCongTheoTienValidate.Text = "Vui lòng nhập vào số tiền tương ứng";
            }

            if (allow)
            {
                try
                {
                    switch (Type)
                    {
                        case EditTypes.add:
                            HttpClient http = new HttpClient();
                            string id_Com = "";
                            switch (Main.Type)
                            {
                                case 1:
                                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                    id_Com = Comid;
                                    break;
                                case 2:
                                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                    id_Com = Comid;
                                    break;
                                default:
                                    break;
                            }
                            Dictionary<string, string> form = new Dictionary<string, string>();
                            switch (Choose)
                            {
                                case CongType.time:
                                    form.Add("shift_name", tbShift.Text);
                                    form.Add("start_time", cbbTimeIn.Time);
                                    form.Add("end_time", cbbTimeOut.Time);
                                    form.Add("start_time_latest", cbbTimeInMoiNhat.Time);
                                    form.Add("end_time_earliest", cbbTimeOutMuonNhat.Time);
                                    form.Add("com_id", id_Com);
                                    form.Add("shift_type", "1");
                                    break;
                                case CongType.shift:
                                    form.Add("shift_name", tbShift.Text);
                                    form.Add("start_time", cbbTimeIn.Time);
                                    form.Add("end_time", cbbTimeOut.Time);
                                    form.Add("start_time_latest", cbbTimeInMoiNhat.Time);
                                    form.Add("end_time_earliest", cbbTimeOutMuonNhat.Time);
                                    form.Add("com_id", id_Com);
                                    form.Add("shift_type", "2");
                                    form.Add("num_to_calculate", cbbCongByShift.SelectedValue.ToString());
                                    break;
                                case CongType.monney:
                                    form.Add("shift_name", tbShift.Text);
                                    form.Add("start_time", cbbTimeIn.Time);
                                    form.Add("end_time", cbbTimeOut.Time);
                                    form.Add("start_time_latest", cbbTimeInMoiNhat.Time);
                                    form.Add("end_time_earliest", cbbTimeOutMuonNhat.Time);
                                    form.Add("com_id", id_Com);
                                    form.Add("shift_type", "3");
                                    form.Add("num_to_money", numtbCongByMonney.Value.ToString());
                                    break;
                                default:
                                    break;
                            }

                            HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/shift/create", new FormUrlEncodedContent(form));
                            API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (api.data != null && api.data.result == true)
                            {
                                if (Success != null) Success();
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Success;
                                x.Message = "Thêm ca thành công";
                                Main.ShowPopup(x);
                            }
                            break;
                        case EditTypes.edit:
                            HttpClient httpEdit = new HttpClient();
                            switch (Main.Type)
                            {
                                case 1:
                                    httpEdit.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                    break;
                                case 2:
                                    httpEdit.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                    break;
                                default:
                                    break;
                            }
                            Dictionary<string, string> formEdit = new Dictionary<string, string>();
                            switch (Choose)
                            {
                                case CongType.time:
                                    formEdit.Add("shift_name", tbShift.Text);
                                    formEdit.Add("start_time", cbbTimeIn.Time);
                                    formEdit.Add("end_time", cbbTimeOut.Time);
                                    formEdit.Add("start_time_latest", cbbTimeInMoiNhat.Time);
                                    formEdit.Add("end_time_earliest", cbbTimeOutMuonNhat.Time);
                                    formEdit.Add("shift_id", info.shift_id);
                                    formEdit.Add("shift_type", "1");
                                    break;
                                case CongType.shift:
                                    formEdit.Add("shift_name", tbShift.Text);
                                    formEdit.Add("start_time", cbbTimeIn.Time);
                                    formEdit.Add("end_time", cbbTimeOut.Time);
                                    formEdit.Add("start_time_latest", cbbTimeInMoiNhat.Time);
                                    formEdit.Add("end_time_earliest", cbbTimeOutMuonNhat.Time);
                                    formEdit.Add("shift_id", info.shift_id);
                                    formEdit.Add("shift_type", "2");
                                    formEdit.Add("num_to_calculate", cbbCongByShift.SelectedValue.ToString());
                                    break;
                                case CongType.monney:
                                    formEdit.Add("shift_name", tbShift.Text);
                                    formEdit.Add("start_time", cbbTimeIn.Time);
                                    formEdit.Add("end_time", cbbTimeOut.Time);
                                    formEdit.Add("start_time_latest", cbbTimeInMoiNhat.Time);
                                    formEdit.Add("end_time_earliest", cbbTimeOutMuonNhat.Time);
                                    formEdit.Add("shift_id", info.shift_id);
                                    formEdit.Add("shift_type", "3");
                                    formEdit.Add("num_to_money", numtbCongByMonney.Value.ToString());
                                    break;
                                default:
                                    break;
                            }
                            HttpResponseMessage responEdit = await httpEdit.PostAsync("http://210.245.108.202:3000/api/qlc/shift/edit", new FormUrlEncodedContent(formEdit));
                            API_Respon apiEdit = JsonConvert.DeserializeObject<API_Respon>(responEdit.Content.ReadAsStringAsync().Result);
                            if (apiEdit.data != null && apiEdit.data.result == true)
                            {
                                if (Success != null) Success();
                                Main.ClosePopup();
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }


        }
        private void ExitPopUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();

        }
        //hiển thị giới hạn thời gian
        private void ShowTimeLimit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowTimeLimit = !ShowTimeLimit;
        }

        private void CongTheoGio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CongTheoCa || CongTheoTien)
            {
                CongTheoGio = !CongTheoGio;
                CongTheoCa = false;
                CongTheoTien = false;
                Choose = CongType.time;
            }



        }

        private void CongTheoCa_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CongTheoGio || CongTheoTien)
            {
                CongTheoCa = !CongTheoCa;
                CongTheoGio = false;
                CongTheoTien = false;
                Choose = CongType.shift;
            }


        }

        private void CongTheoTien_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CongTheoCa || CongTheoGio)
            {
                CongTheoTien = !CongTheoTien;
                CongTheoCa = false;
                CongTheoGio = false;
                Choose = CongType.monney;
            }
        }
    }
}
