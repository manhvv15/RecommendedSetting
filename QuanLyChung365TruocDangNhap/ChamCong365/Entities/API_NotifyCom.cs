using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data_NotifyCom
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Item_NotifyCom> items { get; set; }
    }

    public class Item_NotifyCom
    {
        public string not_id { get; set; }
        public string user_id { get; set; }
        public string affected_id { get; set; }
        public string not_image { get; set; }
        public BitmapImage logo
        {
            get
            {
                if (!string.IsNullOrEmpty(not_image)) return new Uri("https://chamcong.24hpay.vn/image/time_keeping/" + not_image).GetThumbnail(300);
                else return new Uri("https://chamcong.timviec365.vn/images/logo_com.png").GetThumbnail(300);
            }
        }
        public string not_desc { get; set; }
        public TextBlock ContentNoti
        {
            get
            {
                TextBlock txt = new TextBlock();
                if (!string.IsNullOrEmpty(not_desc))
                {
                    string name = not_desc.Substring(0, not_desc.LastIndexOf("</b>")).Replace("<b>", "");
                    string noti = not_desc.Substring(not_desc.IndexOf("</b>") + 4);
                    txt.FontSize = 16;
                    txt.Foreground = App.Current.Resources["#5E5E5E"] as SolidColorBrush;
                    txt.TextWrapping = TextWrapping.Wrap;

                    txt.Inlines.Add(new Run() { Text = name, FontWeight = FontWeights.Bold });
                    txt.Inlines.Add(new Run() { Text = noti, FontWeight = FontWeights.Regular });
                }
                return txt;
            }
        }





        public string user_type { get; set; }
        public string not_type { get; set; }
        public string time_create { get; set; }
        public string time
        {
            get
            {
                DateTime d;
                if (!String.IsNullOrEmpty(time_create.Trim()) && DateTime.TryParse(time_create,out d))
                {
                    //DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(time_create) + (60 * 60 * 7));
                    Double second = (DateTime.Now - d).TotalSeconds;
                    if (second < 60)
                    {
                        return "( " + Convert.ToInt32(second) + " giây trước )";
                    }
                    else if (second >= 60 && second < (60 * 60))
                    {
                        return "( " + Convert.ToInt32(second / 60) + " phút trước )";
                    }
                    else if (second >= 60 && second < (60 * 60 * 24))
                    {
                        return "( " + Convert.ToInt32(second / 60 / 60) + " giờ trước )";
                    }
                    else if (second >= 1440 && second < 60 * 60 * 24 * 30)
                    {
                        return "( " + Convert.ToInt32(second / 60 / 60 / 24) + " ngày trước )";
                    }
                    else if (second >= 60 * 60 * 24 * 30 && second < 60 * 60 * 24 * 365)
                    {
                        return "( " + Convert.ToInt32(second / 60 / 60 / 24 / 30) + " tháng trước )";
                    }
                    else
                    {
                        return "( " + Convert.ToInt32(second / 60 / 60 / 24 / 365) + " năm trước )";
                    }
                }
                return "chưa cập nhật";
            }
        }
        public string not_active { get; set; }
        public string is_seen { get; set; }
        public string url_notification { get; set; }
        public string ep_name { get; set; }
        public string ep_gender { get; set; }
    }

    public class API_NotifyCom
    {
        public Data_NotifyCom data { get; set; }
        public object error { get; set; }
    }



}
