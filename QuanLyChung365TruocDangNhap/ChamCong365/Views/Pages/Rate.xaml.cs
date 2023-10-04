using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Rate.xaml
    /// </summary>
    public partial class Rate : Page
    {
        public Rate(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        public int first_star_vote = 0;
        public MainWindow Main;
        private void DisplayFirstStarVote(int star)
        {
            var converter = new BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString("#FFC715");
            switch (star)
            {
                case 0:
                    star1.Fill = null;
                    star2.Fill = null;
                    star3.Fill = null;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 1:
                    star1.Fill = brush;
                    star2.Fill = null;
                    star3.Fill = null;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 2:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = null;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 3:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = brush;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 4:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = brush;
                    star4.Fill = brush;
                    star5.Fill = null;
                    break;
                case 5:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = brush;
                    star4.Fill = brush;
                    star5.Fill = brush;
                    break;
            }
        }
        private void SetStar(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            switch (name)
            {
                case "gstar1":
                    first_star_vote = 1;
                    break;
                case "gstar2":
                    first_star_vote = 2;
                    break;
                case "gstar3":
                    first_star_vote = 3;
                    break;
                case "gstar4":
                    first_star_vote = 4;
                    break;
                case "gstar5":
                    first_star_vote = 5;
                    break;
            }
            DisplayFirstStarVote(first_star_vote);
        }

        private void MouseMoveFirstStar(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            switch (name)
            {
                case "gstar1":
                    DisplayFirstStarVote(1);
                    break;
                case "gstar2":
                    DisplayFirstStarVote(2);
                    break;
                case "gstar3":
                    DisplayFirstStarVote(3);
                    break;
                case "gstar4":
                    DisplayFirstStarVote(4);
                    break;
                case "gstar5":
                    DisplayFirstStarVote(5);
                    break;
            }
        }

        private void MouseLeaveFirstStar(object sender, MouseEventArgs e)
        {
            DisplayFirstStarVote(first_star_vote);
        }

        private async void Rate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (string.IsNullOrEmpty(tbInputRate.Text))
            {
                allow = false;
                tblInputRateValidate.Text = "Hãy nhập đánh giá của bạn";
            }
            if (first_star_vote == 0)
            {
                allow = false;
                tblInputStarValidate.Text = "Hãy vote đánh giá của bạn";
            }
            if (allow)
            {
                tblInputRateValidate.Text = "";
                tblInputStarValidate.Text = "";
                HttpClient http = new HttpClient();
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("feed_back", tbInputRate.Text);
                form.Add("type", Main.Type.ToString());
                form.Add("rating", first_star_vote.ToString());
                HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/Feedback", new FormUrlEncodedContent(form));
                API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    var pop = new Views.PopUp.PopUp_Rate.Noti_Rate(Main);
                    Main.ShowPopup(pop);
                    //Main.ClosePopup();
                }
            }
        }
        private void FocusTXT(object sender, MouseButtonEventArgs e)
        {
            tbInputRate.Focus();
        }
    }
}
