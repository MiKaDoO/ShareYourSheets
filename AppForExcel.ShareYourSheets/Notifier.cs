using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForExcel.ShareYourSheets
{
    public class Notifier : Hub
    {
        private static List<string> users = new List<string>();

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled).ContinueWith((param) =>
            {
                if (users.Contains(Context.ConnectionId))
                {
                    users.Remove(Context.ConnectionId);
                    Clients.All.Users(users);
                }
            });
        }

        public override Task OnConnected()
        {
            return base.OnConnected().ContinueWith((param) =>
            {
                users.Add(Context.ConnectionId);
                Clients.All.Users(users);
            });
        }

        public void NotifyUser(string connectionId, string datas)
        {
            Clients.Client(connectionId).ReceivedData(Context.ConnectionId, datas, DateTime.Now.ToString("dd/MM/yyyy"));
        }
    }
}
