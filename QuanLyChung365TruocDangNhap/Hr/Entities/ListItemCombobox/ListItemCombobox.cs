using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox
{
    public class ListItemCombobox
    {
        public static List<Item> ListCbxGender = new List<Item>()
        {
            new Item(){ID = "1", value = "Nam"},
            new Item(){ID = "2", value = "Nữ"},
            new Item(){ID = "3", value = "Khác"}
        };

        public static List<Item> ListCbxGender2 = new List<Item>()
        {
            new Item(){ID = "1", value = "Nam"},
            new Item(){ID = "2", value = "Nữ"},
            new Item(){ID = "3", value = "Không yêu cầu"}
        };

        public static List<Item> ListRegulations = new List<Item>()
        {
            new Item(){ID = "1", value = "Quy định bổ ngiệm số 003/HHP-P1"},
            new Item(){ID = "2", value = "Quy định bổ ngiệm số 003/HHP-P2"},
        };

        public static List<Item> ListCbxEducation = new List<Item>()
        {
            new Item(){ID = "1", value = "THPT trở lên"},
            new Item(){ID = "2", value = "Trung học trở lên"},
            new Item(){ID = "3", value = "Chứng chỉ"},
            new Item(){ID = "4", value = "Trung cấp trở lên"},
            new Item(){ID = "5", value = "Cao đẳng trở lên"},
            new Item(){ID = "6", value = "Cử nhân trở lên"},
            new Item(){ID = "7", value = "Đại học trở lên"},
            new Item(){ID = "8", value = "Thạc sĩ trở lên"},
            new Item(){ID = "9", value = "Thạc sĩ Nghệ thuật"},
            new Item(){ID = "10", value = "Thạc sĩ Thương mại"},
            new Item(){ID = "11", value = "Thạc sĩ Khoa học"},
            new Item(){ID = "12", value = "Thạc sĩ Kiến trúc"},
            new Item(){ID = "13", value = "Thạc sĩ QTKD"},
            new Item(){ID = "14", value = "Thạc sĩ Kỹ thuật ứng dụng"},
            new Item(){ID = "15", value = "Thạc sĩ Luật"},
            new Item(){ID = "16", value = "Thạc sĩ Y học"},
            new Item(){ID = "17", value = "Thạc sĩ Dược phẩm"},
            new Item(){ID = "18", value = "Tiến sĩ"},
            new Item(){ID = "19", value = "Không yêu cầu"}
        };

        public static List<Item> ListHinhThuc = new List<Item>()
        {
            new Item(){ID = "1", value = "Giảm biên chế"},
            new Item(){ID = "2", value = "Nghỉ việc"},
        };

        public static List<Item> ListCbxEducationEmployee = new List<Item>()
        {
            new Item(){ID = "0", value = "Chọn trình độ học vấn"},
            new Item(){ID = "1", value = "Trên Đại học"},
            new Item(){ID = "2", value = "Đại học"},
            new Item(){ID = "3", value = "Cao đẳng"},
            new Item(){ID = "4", value = "Trung cấp"},
            new Item(){ID = "5", value = "Đào tạo nghề"},
            new Item(){ID = "6", value = "Trung học phổ thông"},
            new Item(){ID = "7", value = "Trung học cơ sở"},
            new Item(){ID = "8", value = "Tiểu học"}
        };

        public static List<Item> ListCbxExp = new List<Item>()
        {
            new Item(){ID = "0", value = "Chưa có kinh nghiệm"},
            new Item(){ID = "1", value = "0 - 1 năm kinh nghiệm"},
            new Item(){ID = "2", value = "1 - 2 năm kinh nghiệm"},
            new Item(){ID = "3", value = "2 - 5 năm kinh nghiệm"},
            new Item(){ID = "4", value = "5 - 10 năm kinh nghiệm"},
            new Item(){ID = "5", value = "Hơn 10 năm kinh nghiệm"}
        };

        public static List<Item> ListCbxExpEmployee = new List<Item>()
        {
            new Item(){ID = "1", value = "Chưa có kinh nghiệm"},
            new Item(){ID = "2", value = "Dưới 1 năm kinh nghiệm"},
            new Item(){ID = "3", value = "1 năm kinh nghiệm"},
            new Item(){ID = "4", value = "2 năm kinh nghiệm"},
            new Item(){ID = "5", value = "3 năm kinh nghiệm"},
            new Item(){ID = "6", value = "4 năm kinh nghiệm"},
            new Item(){ID = "7", value = "5 năm kinh nghiệm"},
            new Item(){ID = "8", value = "Trên 5 năm kinh nghiệm"}
        };


        public static List<Item> ListMarried = new List<Item>()
        {
            new Item(){ID = "1", value = "Độc thân"},
            new Item(){ID = "2", value = "Đã lập gia đình"},
        };

        public static List<Item> ListPositionApply = new List<Item>()
        {
            new Item(){ID = "1", value = "SINH VIÊN THỰC TẬP"},
            new Item(){ID = "9", value = "NHÂN VIÊN PART TIME"},
            new Item(){ID = "2", value = "NHÂN VIÊN THỬ VIỆC"},
            new Item(){ID = "3", value = "NHÂN VIÊN CHÍNH THỨC"},
            new Item(){ID = "4", value = "TRƯỞNG NHÓM"},
            new Item(){ID = "20", value = "NHÓM PHÓ"},
            new Item(){ID = "13", value = "TỔ TRƯỞNG"},
            new Item(){ID = "12", value = "PHÓ TỔ TRƯỞNG"},
            new Item(){ID = "11", value = "TRƯỞNG BAN DỰ ÁN"},
            new Item(){ID = "10", value = "PHÓ BAN DỰ ÁN"},
            new Item(){ID = "6", value = "TRƯỞNG PHÒNG"},
            new Item(){ID = "5", value = "PHÓ TRƯỞNG PHÒNG"},
            new Item(){ID = "8", value = "GIÁM ĐỐC"},
            new Item(){ID = "7", value = "PHÓ GIÁM ĐỐC"},
            new Item(){ID = "16", value = "TỔNG GIÁM ĐỐC"},
            new Item(){ID = "14", value = "PHÓ TỔNG GIÁM ĐỐC"},
            new Item(){ID = "21", value = "TỔNG GIÁM ĐỐC TẬP ĐOÀN"},
            new Item(){ID = "22", value = "PHÓ TỔNG GIÁM ĐỐC TẬP ĐOÀN"},
            new Item(){ID = "19", value = "CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ"},
            new Item(){ID = "18", value = "PHÓ CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ"},
            new Item(){ID = "17", value = "THÀNH VIÊN HỘI ĐỒNG QUẢN TRỊ"},
        };

        public static List<Item> ListSalary = new List<Item>()
        {
            //new Item(){ID = "0", value = "-- Vui lòng chọn --"},
            new Item(){ID = "1", value = "Thỏa thuận"},
            new Item(){ID = "2", value = "1 - 3 triệu"},
            new Item(){ID = "3", value = "3 - 5 triệu"},
            new Item(){ID = "4", value = "5- 7 triệu"},
            new Item(){ID = "5", value = "7 - 10 triệu"},
            new Item(){ID = "6", value = "10 -15 triệu"},
            new Item(){ID = "7", value = "15- 20 triệu"},
            new Item(){ID = "8", value = "20 -30 triệu"},
            new Item(){ID = "9", value = "Trên 30 triệu"},
            new Item(){ID = "10", value = "Trên 50 triệu"},
            new Item(){ID = "11", value = "Trên 100 triệu"},
        };

        public static List<Item> ListMethodWork = new List<Item>()
        {
            //new Item(){ID = "0", value = "-- Vui lòng chọn --"},
            new Item(){ID = "1", value = "Toàn thời gian cố định"},
            new Item(){ID = "2", value = "Toàn thời gian tạm thời"},
            new Item(){ID = "3", value = "Bán thời gian"},
            new Item(){ID = "4", value = "Bán thời gian tạm thời"},
            new Item(){ID = "5", value = "Hợp đồng"},
            new Item(){ID = "6", value = "Khác"},
        };

        public static List<Item> ListPositionRecruit = new List<Item>()
        {
            //new Item(){ID = "0", value = "-- Vui lòng chọn --"},    
            new Item(){ID = "1", value = "Mới tốt nghiệp"},    
            new Item(){ID = "2", value = "Thực tập sinh"},    
            new Item(){ID = "3", value = "Nhân viên"},    
            new Item(){ID = "4", value = "Trưởng phòng"},    
        };

        public static List<Item> ListCareer = new List<Item>()
        {
            new Item(){ID ="1", value = "Kế toán - Kiểm toán"},
            new Item(){ID ="2", value = "Hành chính - Văn phòng"},
            new Item(){ID ="83", value = "Việc làm thời vụ"},
            new Item(){ID ="3", value = "Sinh viên làm thêm"},
            new Item(){ID ="4", value = "Xây dựng"},
            new Item(){ID ="5", value = "Điện - Điện tử"},
            new Item(){ID ="6", value = "Làm bán thời gian"},
            new Item(){ID ="7", value = "Vận tải - Lái xe"},
            new Item(){ID ="8", value = "Khách sạn - Nhà hàng"},
            new Item(){ID ="9", value = "Nhân viên kinh doanh"},
            new Item(){ID ="10", value = "Việc làm bán hàng"},
            new Item(){ID ="11", value = "Cơ khí - Chế tạo"},
            new Item(){ID ="12", value = "Lao động phổ thông"},
            new Item(){ID ="13", value = "IT phần mềm"},
            new Item(){ID ="14", value = "Marketing - PR"},
            new Item(){ID ="43", value = "Nhập liệu"},
            new Item(){ID ="17", value = "Giáo dục - Đào tạo"},
            new Item(){ID ="18", value = "Kỹ thuật"},
            new Item(){ID ="19", value = "Y tế - Dược"},
            new Item(){ID ="20", value = "Quản trị kinh doanh"},
            new Item(){ID ="21", value = "Dịch vụ"},
            new Item(){ID ="22", value = "Biên - Phiên dịch"},
            new Item(){ID ="23", value = "Dệt may - Da giày"},
            new Item(){ID ="87", value = "Tìm việc làm thêm"},
            new Item(){ID ="24", value = "Kiến trúc - Tk nội thất"},
            new Item(){ID ="25", value = "Xuất - nhập khẩu"},
            new Item(){ID ="26", value = "IT Phần cứng - mạng"},
            new Item(){ID ="27", value = "Nhân sự"},
            new Item(){ID ="28", value = "Thiết kế - Mỹ thuật"},
            new Item(){ID ="29", value = "Tư vấn"},
            new Item(){ID ="30", value = "Bảo vệ"},
            new Item(){ID ="31", value = "Ô tô - xe máy"},
            new Item(){ID ="32", value = "Thư ký - Trợ lý"},
            new Item(){ID ="33", value = "KD bất động sản"},
            new Item(){ID ="34", value = "Du lịch"},
            new Item(){ID ="35", value = "Báo chí - Truyền hình"},
            new Item(){ID ="36", value = "Thực phẩm - Đồ uống"},
            new Item(){ID ="37", value = "Ngành nghề khác"},
            new Item(){ID ="38", value = "Vật tư - Thiết bị"},
            new Item(){ID ="39", value = "Thiết kế web"},
            new Item(){ID ="40", value = "In ấn - Xuất bản"},
            new Item(){ID ="41", value = "Nông - Lâm - Ngư - Nghiệp"},
            new Item(){ID ="42", value = "Thương mại điện tử"},
            new Item(){ID ="44", value = "Việc làm thêm tại nhà"},
            new Item(){ID ="45", value = "Chăm sóc khách hàng"},
            new Item(){ID ="46", value = "Sinh viên mới tốt nghiệp - Thực tập"},
            new Item(){ID ="47", value = "Kỹ thuật ứng dụng"},
            new Item(){ID ="48", value = "Bưu chính viễn thông"},
            new Item(){ID ="49", value = "Dầu khí - Địa chất"},
            new Item(){ID ="50", value = "Giao thông vận tải - Thủy lợi - Cầu đường"},
            new Item(){ID ="51", value = "Khu chế xuất - Khu công nghiệp"},
            new Item(){ID ="52", value = "Làm đẹp - Thể lực - Spa"},
            new Item(){ID ="53", value = "Luật - Pháp lý"},
            new Item(){ID ="54", value = "Môi trường - Xử lý chất thải"},
            new Item(){ID ="55", value = "Mỹ phẩm - Thời trang - Trang sức"},
            new Item(){ID ="56", value = "Ngân hàng - Chứng khoán - Đầu tư"},
            new Item(){ID ="57", value = "Nghệ thuật - Điện ảnh"},
            new Item(){ID ="58", value = "Phát triển thị trường"},
            new Item(){ID ="59", value = "Phục vụ - Tạp vụ"},
            new Item(){ID ="60", value = "Quan hệ đối ngoại"},
            new Item(){ID ="61", value = "Quản lý điều hành"},
            new Item(){ID ="62", value = "Sản xuất - Vận hành sản xuất"},
            new Item(){ID ="63", value = "Thẩm định - Giám thẩm định - Quản lý chất lượng"},
            new Item(){ID ="64", value = "Thể dục - Thể thao"},
            new Item(){ID ="65", value = "Hóa học - Sinh học"},
            new Item(){ID ="66", value = "Bảo hiểm"},
            new Item(){ID ="67", value = "Freelancer"},
            new Item(){ID ="68", value = "Công chức - Viên chức "},
            new Item(){ID ="71", value = "Điện tử viễn thông"},
            new Item(){ID ="73", value = "Hoạch định - Dự án"},
            new Item(){ID ="75", value = "Lương cao"},
            new Item(){ID ="77", value = "Tiếp thị - Quảng cáo"},
            new Item(){ID ="79", value = "Việc làm Tết"},
            new Item(){ID ="81", value = "Giúp việc"},
            new Item(){ID ="89", value = "Thủy sản"},
            new Item(){ID ="91", value = "Công nghệ thực phẩm"},
            new Item(){ID ="93", value = "Chăn nuôi - Thú y"},
            new Item(){ID ="95", value = "An toàn lao động"},
            new Item(){ID ="97", value = "Hàng không"},
            new Item(){ID ="131", value = "Tài chính"},
            new Item(){ID ="101", value = "Tổ chức sự kiện"},
            new Item(){ID ="103", value = "Trắc địa"},
            new Item(){ID ="107", value = "Bảo trì"},
            new Item(){ID ="109", value = "Hàng hải"},
            new Item(){ID ="111", value = "Đầu bếp - phụ bếp"},
            new Item(){ID ="113", value = "Truyền thông"},
            new Item(){ID ="115", value = "Startup"},
            new Item(){ID ="119", value = "Thư viện"},
            new Item(){ID ="121", value = "Thống kê"},
            new Item(){ID ="123", value = "Copywriter"},
            new Item(){ID ="125", value = "Xuất khẩu lao động"},
            new Item(){ID ="127", value = "Công nghệ cao"},
            new Item(){ID ="137", value = "Pha chế - Bar"},
            new Item(){ID ="135", value = "Lễ tân - PG - PB"},
            new Item(){ID ="129", value = "Logistic"},
            new Item(){ID ="133", value = "Vận chuyển giao nhận"},
            new Item(){ID ="139", value = "Quản lý đơn hàng"},
            new Item(){ID ="141", value = "Thu ngân "},
            new Item(){ID ="145", value = "Telesales"},
        };


        public static List<Item> ListProvince = new List<Item>()
        {
            new Item(){ID = "1", value = "Hà Nội"},
            new Item(){ID = "2", value = "Hải Phòng"},
            new Item(){ID = "3", value = "Bắc Giang"},
            new Item(){ID = "4", value = "Bắc Kạn"},
            new Item(){ID = "5", value = "Bắc Ninh"},
            new Item(){ID = "6", value = "Cao Bằng"},
            new Item(){ID = "7", value = "Điện Biên"},
            new Item(){ID = "8", value = "Hòa Bình"},
            new Item(){ID = "9", value = "Hải Dương"},
            new Item(){ID = "10", value = "Hà Giang"},
            new Item(){ID = "11", value = "Hà Nam"},
            new Item(){ID = "12", value = "Hưng Yên"},
            new Item(){ID = "13", value = "Lào Cai"},
            new Item(){ID = "14", value = "Lai Châu"},
            new Item(){ID = "15", value = "Lạng Sơn"},
            new Item(){ID = "16", value = "Ninh Bình"},
            new Item(){ID = "17", value = "Nam Định"},
            new Item(){ID = "18", value = "Phú Thọ"},
            new Item(){ID = "19", value = "Quảng Ninh"},
            new Item(){ID = "20", value = "Sơn La"},
            new Item(){ID = "21", value = "Thái Bình"},
            new Item(){ID = "22", value = "Thái Nguyên"},
            new Item(){ID = "23", value = "Tuyên Quang"},
            new Item(){ID = "24", value = "Vĩnh Phúc"},
            new Item(){ID = "25", value = "Yên Bái"},
            new Item(){ID = "26", value = "Đà Nẵng"},
            new Item(){ID = "27", value = "Thừa Thiên Huế"},
            new Item(){ID = "28", value = "Khánh Hòa"},
            new Item(){ID = "29", value = "Lâm Đồng"},
            new Item(){ID = "30", value = "Bình Định"},
            new Item(){ID = "31", value = "Bình Thuận"},
            new Item(){ID = "32", value = "Đắk Lắk"},
            new Item(){ID = "33", value = "Đắk Nông"},
            new Item(){ID = "34", value = "Gia Lai"},
            new Item(){ID = "35", value = "Hà Tĩnh"},
            new Item(){ID = "36", value = "Kon Tum"},
            new Item(){ID = "37", value = "Nghệ An"},
            new Item(){ID = "38", value = "Ninh Thuận"},
            new Item(){ID = "39", value = "Phú Yên"},
            new Item(){ID = "40", value = "Quảng Bình"},
            new Item(){ID = "41", value = "Quảng Nam"},
            new Item(){ID = "42", value = "Quảng Ngãi"},
            new Item(){ID = "43", value = "Quảng Trị"},
            new Item(){ID = "44", value = "Thanh Hóa"},
            new Item(){ID = "45", value = "Hồ Chí Minh"},
            new Item(){ID = "46", value = "Bình Dương"},
            new Item(){ID = "47", value = "Bà Rịa Vũng Tàu"},
            new Item(){ID = "48", value = "Cần Thơ"},
            new Item(){ID = "49", value = "An Giang"},
            new Item(){ID = "50", value = "Bạc Liêu"},
            new Item(){ID = "51", value = "Bình Phước"},
            new Item(){ID = "52", value = "Bến Tre"},
            new Item(){ID = "53", value = "Cà Mau"},
            new Item(){ID = "54", value = "Đồng Tháp"},
            new Item(){ID = "55", value = "Đồng Nai"},
            new Item(){ID = "56", value = "Hậu Giang"},
            new Item(){ID = "57", value = "Kiên Giang"},
            new Item(){ID = "58", value = "Long An"},
            new Item(){ID = "59", value = "Sóc Trăng"},
            new Item(){ID = "60", value = "Tiền Giang"},
            new Item(){ID = "61", value = "Tây Ninh"},
            new Item(){ID = "62", value = "Trà Vinh"},
            new Item(){ID = "63", value = "Vĩnh Long"},
        };

        public ListItemCombobox()
        {

        }
    }

    public class Item
    {
        public string ID { get; set; }
        public string value { get; set; }
        public string Department { get; set; }

    }
}
