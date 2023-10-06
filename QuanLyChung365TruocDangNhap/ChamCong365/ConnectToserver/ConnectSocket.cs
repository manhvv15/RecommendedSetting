using SocketIOClient;
using SocketIOClient.Windows7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.ConnectToserver
{
    public delegate void checkEro(string ex);

    public class ConnectSocket
    {
        public SocketIO WIO { get; set; }
        public event checkEro CheckError;
        public ConnectSocket()
        {
            try
            {
                var uri = new Uri("http://43.239.223.142:3000/");
                WIO = new SocketIO(uri, new SocketIOOptions
                {
                    Reconnection = true,
                    //ReconnectionAttempts = 100,
                    Query = new Dictionary<string, string>
                    {
                        {"token", "V3" }
                    },
                });
                WIO.ClientWebSocketProvider = () => new ClientWebSocketManaged(3);
                WIO.ConnectAsync();
            }
            catch (Exception ex)
            {
                CheckError(ex.Message);
            }
        }

        public class objectData : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public objectData(string chatUserId, string userName)
            {
                this.chatUserId = chatUserId;
                this.userName = userName;
            }
            private string _chatUserId;
            public string chatUserId { get => _chatUserId; set { _chatUserId = value; OnPropertyChanged(); } }

            private string _userName;
            public string userName { get => _userName; set { _userName = value; OnPropertyChanged(); } }
        }
        public class dataToAddUser
        {
            public string userId { get; set; }
            public string name { get; set; }
        }

        public void LoginSuccess(int userId, int companyId, string fromWeb)
        {
            try
            {
                //WIO.EmitAsync("LoginWithIdDevice", "8f04b95f-a397-465a-8536-1304167b2014", "work247");
                WIO.EmitAsync("Login", userId, "chat365");
                if (fromWeb != "chat365") WIO.EmitAsync("Login", userId, fromWeb);
            }
            catch (Exception ex)
            {

            }
        }

        public void SendInforQrCode(string QrId, string Email, string UserPassword)
        {
            try
            {
                WIO.EmitAsync("QRLogin", QrId, Email, UserPassword);
            }
            catch (Exception ex)
            {

            }
        }

        //public void LoginWarning(int userId)
        //{
        //    try
        //    {
        //        WIO.EmitAsync("LoginWarning", userId);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
