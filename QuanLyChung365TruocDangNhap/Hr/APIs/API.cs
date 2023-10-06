using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.APIs
{
    public class API
    {
        // api đăng nhập
        public const string login_employee_api = "http://210.245.108.202:3000/api/qlc/employee/login";
        public const string login_company_api = "http://210.245.108.202:3000/api/qlc/employee/login"; //api đăng nhập công ty
        public const string send_otp_password_api = "https://chamcong.24hpay.vn/service/send_otp.php"; //api lấy mã xác nhận
        public const string forgot_password_employee_api = "https://chamcong.24hpay.vn/service/forget_pass_employee.php"; //api đặt lại mật khẩu nhân viên
        public const string forgot_password_company_api = "https://chamcong.24hpay.vn/service/forget_pass_company.php"; //api đặt lại mật khẩu công ty
        public const string verify_otp = "https://chamcong.24hpay.vn/service/verify_otp.php";

        // api trang chủ
        public const string avatar_uri = "https://chamcong.24hpay.vn/upload/employee/";
        public const string logo_uri = "https://chamcong.24hpay.vn/upload/company/logo/";
        public const string achievements_total_per_month = "http://210.245.108.202:3006/api/hr/home/totalAchievement";
        public const string violation_total_per_month = "http://210.245.108.202:3006/api/hr/home/totalInfringe";
        public const string candidate_total = "http://210.245.108.202:3006/api/hr/home/totalCandidate";
        public const string interview_total = "http://210.245.108.202:3006/api/hr/home/totalInterview";
        public const string offerjob_total = "http://210.245.108.202:3006/api/hr/home/totalOfferJob";
        public const string weather_api = "https://phanmemnhansu.timviec365.vn/apiWeather";

        // api quản lý tuyển dụng
        // api quy trình tuyển dụng
        public const string list_recuitment = "http://210.245.108.202:3006/api/hr/recruitment/getRecruitment";
        public const string list_recruitment_stage = "http://210.245.108.202:3006/api/hr/recruitment/getStage";
        public const string delete_recuitment = "http://210.245.108.202:3006/api/hr/recruitment/softDelete"; // xóa quy trình tuyển dụng
        public const string delete_stage = "http://210.245.108.202:3006/api/hr/recruitment/softDeleteStage"; // xóa giai đoạn tuyển dụng
        public const string edit_recruitment = "http://210.245.108.202:3006/api/hr/recruitment/updateRecruitment"; // chỉnh sửa quy trình
        public const string edit_stage = "http://210.245.108.202:3006/api/hr/recruitment/updateStage"; // chỉnh sửa giai đoạn tuyển dụng
        public const string add_stage = "http://210.245.108.202:3006/api/hr/recruitment/createStage"; // thêm mới giai đoạn tuyển dụng

        // api thực hiện tuyển dụng
        public const string total_candidate_by = "http://210.245.108.202:3006/api/hr/recruitment/totalCandi";
        public const string lits_new_active = "http://210.245.108.202:3006/api/hr/recruitment/listNewActive";
        public const string list_schedule = "http://210.245.108.202:3006/api/hr/recruitment/listSchedule";
        public const string list_news = "http://210.245.108.202:3006/api/hr/recruitment/listNews";
        public const string perform_detail = "http://210.245.108.202:3006/api/hr/recruitment/detailNews";
        public const string removeRcruitmentNew = "http://210.245.108.202:3006/api/hr/recruitment/softDeleteNews"; // gỡ tin tuyển dụng
        public const string setSampleNew = "http://210.245.108.202:3006/api/hr/recruitment/createSampleNews"; // thiết lập tin mẫu
        public const string addRecuitmentNew = "http://210.245.108.202:3006/api/hr/recruitment/createNews"; // thêm mới tin tuyển dụng
        public const string editRecruitmentNew = "http://210.245.108.202:3006/api/hr/recruitment/updateNews"; // cập nhật tin tuyển dụng

        // api danh sách ứng viên
        public const string list_candidate = "http://210.245.108.202:3006/api/hr/recruitment/wflistCandidate";
        public const string list_candidate_schedule = "http://210.245.108.202:3006/api/hr/recruitment/wflistCandidateSchedule";
        public const string list_candidate_get_job = "http://210.245.108.202:3006/api/hr/recruitment/wflistCandidateGetJob";
        public const string list_candidate_fail_job = "http://210.245.108.202:3006/api/hr/recruitment/wflistCandidateFailJob";
        public const string list_candidate_cancel_job = "http://210.245.108.202:3006/api/hr/recruitment/wflistCandidateCancelJob";
        public const string list_candidate_contact_job = "http://210.245.108.202:3006/api/hr/recruitment/wflistCandidateContactJob";
        public const string detail_candidate = "http://210.245.108.202:3006/api/hr/recruitment/detailCandidateV2";
        public const string detail_candidate_get_job = "http://210.245.108.202:3006/api/hr/recruitment/detailCandidateGetJob";
        public const string detail_candidate_fail_job = "http://210.245.108.202:3006/api/hr/recruitment/detailCandidateFailJob";
        public const string detail_candidate_cancel_job = "http://210.245.108.202:3006/api/hr/recruitment/detailCandidateCancelJob";
        public const string detail_candidate_contract_job = "http://210.245.108.202:3006/api/hr/recruitment/detailCandidateContactJob";
        public const string api_cv = "https://phanmemnhansu.timviec365.vn/upload/cv/"; // api cv ứng viên
        public const string delete_candidate = "http://210.245.108.202:3006/api/hr/recruitment/softDeleteCandi"; // api xóa ứng viên
        public const string list_all_employee = "http://210.245.108.202:3000/api/qlc/managerUser/list"; // danh sách nhân viên active
        public const string list_all_employee2 = "http://210.245.108.202:3000/api/qlc/managerUser/listAll"; // danh sách nhân viên active + not active
        public const string list_process_interview = "http://210.245.108.202:3006/api/hr/recruitment/listProcessInterview"; // danh sách giai đoạn tuyển dụng
        public const string listProcessInterviewGetJob = "http://210.245.108.202:3006/api/hr/recruitment/listProcessInterviewGetJob"; // danh sách nhận việc
        public const string listProcessInterviewFailJob = "http://210.245.108.202:3006/api/hr/recruitment/listProcessInterviewFailJob"; // danh sách trượt
        public const string listProcessInterviewCancelJob = "http://210.245.108.202:3006/api/hr/recruitment/listProcessInterviewCancelJob"; // danh sách hủy
        public const string listProcessInterviewContactJob = "http://210.245.108.202:3006/api/hr/recruitment/listProcessInterviewContactJob"; // danh sách ký hợp đồng
        public const string detail_candidate_process = "http://210.245.108.202:3006/api/hr/recruitment/detailCandidateV2"; // chi tiết ứng viên giai đoạn tuyển dụng
        public const string add_process_interview = "http://210.245.108.202:3006/api/hr/recruitment/createProcess"; // thêm mới giai đoạnt tuyển dụng
        public const string delete_process_interview = "http://210.245.108.202:3006/api/hr/recruitment/deleteProcess"; // xóa giai đoạn tuyển dụng
        public const string edit_process_interview = "http://210.245.108.202:3006/api/hr/recruitment/updateProcess"; // sửa giai đoạn tuyển dụng
        public const string add_candidate = "http://210.245.108.202:3006/api/hr/recruitment/createCandidate"; // thêm mới ứng viên
        public const string list_all_new = "http://210.245.108.202:3006/api/hr/recruitment/listNews"; // lấy tất cả tin tuyển dụng
        public const string edit_candidate_send_cv = "http://210.245.108.202:3006/api/hr/recruitment/updateCandidate"; // sửa ứng viên gửi hồ sơ
        public const string edit_candidate_interview = "http://210.245.108.202:3006/api/hr/recruitment/scheduleInter"; // sửa ứng viên giai đoạn phỏng vấn
        public const string edit_candidate_get_job = "http://210.245.108.202:3006/api/hr/recruitment/addCandidateGetJob"; // sửa ứng viên giai đoạn nhận việc
        public const string edit_candidate_fail_job = "http://210.245.108.202:3006/api/hr/recruitment/FailJob"; // sửa ứng viên giai đoạn trượt
        public const string edit_candidate_cancel_job = "http://210.245.108.202:3006/api/hr/recruitment/cancelJob"; // sửa ứng viên giai đoạn hủy
        public const string edit_candidate_contract_job = "http://210.245.108.202:3006/api/hr/recruitment/contactJob"; // sửa ứng viên giai đoạn kí hợp đồng

        //chuyển giai đoạn danh sách ứng viên
        public const string addCandidateGetJob = "http://210.245.108.202:3006/api/hr/recruitment/addCandidateGetJob"; //nhận việc
        public const string addCandidateFailJob = "http://210.245.108.202:3006/api/hr/recruitment/FailJob";  //trượt
        public const string addCandidateCancelJob = "http://210.245.108.202:3006/api/hr/recruitment/cancelJob"; //hủy
        public const string addCandidateContactJob = "http://210.245.108.202:3006/api/hr/recruitment/contactJob"; //ký hợp đồng
        public const string addCandidateProcessInterview = "http://210.245.108.202:3006/api/hr/recruitment/scheduleInter"; //chuyển hồ sơ sang phỏng vấn
        // api kho ứng viên
        public const string list_candidate_depot = "http://210.245.108.202:3006/api/hr/recruitment/listCandi"; // lấy danh sách ứng viên 

        //Đào tạo và phát triển
        public const string list_tranning = "http://210.245.108.202:3006/api/hr/training/listProcessTrain"; //api quy trình đào tạo
        public const string list_JobPositon = "http://210.245.108.202:3006/api/hr/training/listJob"; //api danh sách vị trí công việc
        public const string updateStageTrainingProcess = "http://210.245.108.202:3006/api/hr/training/updateStage"; //api cập nhập giai đoạn

        //Lương thưởng và phúc lợi 
        public const string listAchievement = "http://210.245.108.202:3006/api/hr/welfare/listAchievement"; //api khen thưởng
        public const string listInfringes = "http://210.245.108.202:3006/api/hr/welfare/listInfinges"; //api vi phạm

        //Quản lý hành chính
            // Quản lý nhân viên
        public const string delete_employee = "http://210.245.108.202:3000/api/qlc/managerUser/del"; // xóa nhân viên
        public const string all_branch = "http://210.245.108.202:3000/api/qlc/company/child/list"; // lấy tất cả chi nhánh của công ty
        public const string edit_employee = "http://210.245.108.202:3000/api/qlc/employee/updateInfoEmployeeComp"; //sửa thông tin nhân viên

            // quy định chính sách
        public const string listWorkingRegulations = "http://210.245.108.202:3006/api/hr/administration/listProvision"; // danh  sách nhóm quy định việc làm
        public const string list_policy_by = "http://210.245.108.202:3006/api/hr/administration/listPolicy"; // danh sách quy định làm việc
        public const string delete_provision = "http://210.245.108.202:3006/api/hr/administration/deleteProvision"; // xóa nhóm quy định
        public const string delete_policy = "http://210.245.108.202:3006/api/hr/administration/deletePolicy"; // xóa quy định
        public const string detail_provison = "http://210.245.108.202:3006/api/hr/administration/detailProvision"; // chi tiết nhóm quy định
        public const string add_provision = "http://210.245.108.202:3006/api/hr/administration/addProvision"; // thêm mới nhóm quy định
        public const string preview_file_policy = "https://docs.google.com/viewerng/viewer?url=https://phanmemnhansu.timviec365.vn/upload/policy/"; // preview file
        public const string detail_policy = "http://210.245.108.202:3006/api/hr/administration/detailPolicy"; // chi tiết quy định
        public const string update_provision = "http://210.245.108.202:3006/api/hr/administration/updateProvision"; // update nhóm quy định
        public const string add_policy = "http://210.245.108.202:3006/api/hr/administration/addPolicy"; // thêm quy định
        public const string edit_policy = "http://210.245.108.202:3006/api/hr/administration/updatePolicy"; // sửa quy định


        public const string listEmployeePolicyPage = "http://210.245.108.202:3006/api/hr/administration/listEmpoyePolicy"; //chính sách nhân viên
        public const string list_employee_policy_by = "http://210.245.108.202:3006/api/hr/administration/listEmployeePolicySpecific"; // danh sách chính sách nhân viên
        public const string delete_employee_policy = "http://210.245.108.202:3006/api/hr/administration/deleteEmployeePolicy"; // xóa nhóm chính sách nhân viên
        public const string delete_employee_policy2 = "http://210.245.108.202:3006/api/hr/administration/deleteEmployeePolicySpecific"; // xóa chính sách nhân viên
        public const string detial_employee_policy_group = "http://210.245.108.202:3006/api/hr/administration/getDetailPolicy"; // chi tiết nhóm chính sách nhân viên
        public const string detail_employee_policy = "http://210.245.108.202:3006/api/hr/administration/detailEmployeePolicySpecific"; // chi tiết chính sách nhân viên
        public const string add_employee_policy_group = "http://210.245.108.202:3006/api/hr/administration/addEmployeePolicy"; // thêm mới nhóm chinh sách nhân viên
        public const string add_employee_policy = "http://210.245.108.202:3006/api/hr/administration/addEmpoyePolicySpecific"; // thêm mới chính sách nhân viên
        public const string edit_employee_policy_group = "http://210.245.108.202:3006/api/hr/administration/updateEmployeePolicy"; // cập nhật nhóm chính sách nhân viên
        public const string edit_employee_policy = "http://210.245.108.202:3006/api/hr/administration/updateEmployeePolicySpecific"; // cập nhật chính sách nhân viên

            //Biến động nhân sự
        public const string appointment_list = "http://210.245.108.202:3006/api/hr/personalChange/getListAppoint"; // bổ nhiệm, quy hoạch

        public const string up_down_salary = "http://210.245.108.202:3006/api/hr/report/reportDetail"; // tăng giảm lương
        public const string list_job_rotation = "http://210.245.108.202:3006/api/hr/personalChange/getListTranferJob"; // luân chuyển công tác



        public const string list_downsizing = "http://210.245.108.202:3006/api/hr/personalChange/getListQuitJob"; // giảm biên chế và nghỉ việc

        public const string getlistEmplouyee = "http://210.245.108.202:3000/api/qlc/managerUser/list"; //api lấy toàn bộ danh sách nhân viên 
        public const string listDepartment = "http://210.245.108.202:3000/api/qlc/department/list"; //api lấy toàn bộ danh sách phòng ban
        public const string listCompany = "http://210.245.108.202:3000/api/qlc/company/child/list"; //lấy toàn bộ danh sách công ty
        public const string listRecruitmentReport = "http://210.245.108.202:3006/api/hr/report/reportDetailRecruitment"; //báo cáo chi tiết theo tin tuyển dụng
        public const string listRecruitmentReportEmployee = "http://210.245.108.202:3006/api/hr/report/reportHr"; //báo cáo chi tiết theo tin tuyển dụng theo nhân viên
        public const string reportlistCandidateRcm = "http://210.245.108.202:3006/api/hr/report/reportDetailHRAndAchievements"; //Báo cáo chi tiết theo nhân viên giới thiệu ứng viên và tiền thưởng trực tiếp
        public const string StatisticalReport = "http://210.245.108.202:3006/api/hr/report/reportRecruitment"; //api thống kê kê báo cáo


        public const string AddAchievementsPopup = "http://210.245.108.202:3006/api/hr/welfare/addAchievement"; //api thêm thành tích cá nhân
        public const string UpdateAchievementsPopup = "http://210.245.108.202:3006/api/hr/welfare/updateAchievement"; // api sửa thành tích
        public const string AddAchievementsGroup = "http://210.245.108.202:3006/api/hr/welfare/addAchievementGroup"; // api thêm thành tích tập thể
        public const string updateAchievementGroup = "http://210.245.108.202:3006/api/hr/welfare/updateAchievement"; // api sửa thành tích tập thể
        public const string updateInfingesGroup = "http://210.245.108.202:3006/api/hr/welfare/updateInfinges"; //api thêm mới vi phạm tập thể
        public const string updateInfinges = "http://210.245.108.202:3006/api/hr/welfare/updateInfinges"; //api Chỉnh sửa vi phạm cá nhân
        public const string addInfinges = "http://210.245.108.202:3006/api/hr/welfare/addInfinges"; //api thêm mới vi phạm cá nhân
        public const string addInfingesGroup = "http://210.245.108.202:3006/api/hr/welfare/addInfingesGroup"; //api thêm mới vi phạm tập thể

        public const string deleteJobDescription = "http://210.245.108.202:3006/api/hr/training/jobSoftDelete"; //Api xóa vị trí công việc
        public const string addJobDescription = " http://210.245.108.202:3006/api/hr/training/createJob"; //api thêm mới vị trí công việc
        public const string addTrainingProcess = "http://210.245.108.202:3006/api/hr/training/process"; //api thêm mới quy trình đào tạo
        public const string listStageTraining = "http://210.245.108.202:3006/api/hr/training/detailProcess"; //api danh sách giai đoạn
        public const string deleteTrainingProcess = "http://210.245.108.202:3006/api/hr/training/softDeleteProcess"; //api xóa quy trình đào tạo
        public const string addStageTrainingProcess = "http://210.245.108.202:3006/api/hr/training/stage";//api thêm giai đoạn
        public const string deleteStageTrainingProcess = "http://210.245.108.202:3006/api/hr/training/softDeleteStage"; //api xóa giai đoạn

        // api báo cáo nhân sự
        public const string list_report_new_active = "http://210.245.108.202:3006/api/hr/report/reportDetailRecruitment"; // api danh sách chi tiết theo tin tuyển dụng
        public const string reportListRecruitmentStaff = "http://210.245.108.202:3006/api/hr/report/reportHr"; // api danh sách chi tiết theo nhân viên tuyển dụng

        // dữ liệu đã xóa gần đây
        public const string listDetailDelete = "http://210.245.108.202:3006/api/hr/forceDelete/listDetailDelete"; // danh sách dữ liệu đã xóa gần đây
        public const string deleteRecentData = "http://210.245.108.202:3006/api/hr/forceDelete/delete"; // xóa dữ liệu đã xóa gần đây
        public const string restoreRecentData = "http://210.245.108.202:3006/api/hr/forceDelete/restoreDelete"; // xóa dữ liệu đã xóa gần đây


        public const string addAppoint = "http://210.245.108.202:3006/api/hr/personalChange/updateAppoint"; // thêm mới quy hoạch bổ nhiệm
        public const string update_employe = "http://210.245.108.202:3006/api/hr/personalChange/updateAppoint"; //chỉnh sửa quy hoạch bổ nhiệm
        public const string add_working = "http://210.245.108.202:3006/api/hr/personalChange/updateTranferJob"; //thêm mới luân chuyển công tác

        public const string list_shift = "http://210.245.108.202:3000/api/qlc/shift/list?com_id="; //lấy danh sách ca
        public const string addDownzing = "http://210.245.108.202:3006/api/hr/personalChange/updateQuitJob"; //thêm mới giảm biên chế nghỉ việc
        public const string updateDownzing = "http://210.245.108.202:3006/api/hr/personalChange/updateQuitJob"; //cập nhập giảm biên chế nghỉ việc

        // setting
        public const string company_info = "http://210.245.108.202:3000/api/qlc/company/info"; // chi tiết công ty
        public const string update_company_info = "http://210.245.108.202:3000/api/qlc/Company/updateInfoCompany"; // cập nhật thông tin công ty
        public const string update_employee_info = "https://chamcong.24hpay.vn/service/update_user_info_employee.php"; // cập nhật thông tin nhân viên
        public const string change_pass_company = "http://210.245.108.202:3000/api/qlc/Company/updateNewPassword"; // đổi mật khẩu công ty
        public const string change_pass_employee = "http://210.245.108.202:3000/api/qlc/employee/updatePassword"; // đổi mật khẩu công ty
        public const string apiCheckRole = "http://210.245.108.202:3006/api/hr/setting/listPermision"; // check quyền nhân viên
        public const string updateRole = "http://210.245.108.202:3006/api/hr/setting/permision"; // cấp quyền cho nhân viên




        public const string updateAchievement = "http://210.245.108.202:3006/api/hr/welfare/updateAchievement";

        //api thông báo luân chuyển
        public const string NotificationPersonnelChange = "https://mess.timviec365.vn/Notification/NotificationPersonnelChange";
        //api thông báo ken thưởng
        public const string NotificationRewardDiscipline = "https://mess.timviec365.vn/Notification/NotificationRewardDiscipline";
    }
}
