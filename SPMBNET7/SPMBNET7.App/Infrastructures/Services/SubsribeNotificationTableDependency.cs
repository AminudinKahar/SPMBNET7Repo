using Microsoft.AspNetCore.Authorization;
using SPMBNET7.App.Infrastructures.Hubs;
using SPMBNET7.CoreBusiness._Enums;
using SPMBNET7.CoreBusiness.Services;
using SPMBNET7.Infrastructure.Interfaces.Common;
using TableDependency.SqlClient;

namespace SPMBNET7.App.Infrastructures.Services
{
    [AllowAnonymous]
    public class SubscribeNotificationTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Notification> tableDependency;
        NotificationHub notificationHub;

        public SubscribeNotificationTableDependency(NotificationHub notificationHub)
        {
            this.notificationHub = notificationHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Notification>(
                                connectionString,
                                null,
                                null,
                                null,
                                null,
                                null,
                                TableDependency.SqlClient.Base.Enums.DmlTriggerType.All,
                                false,
                                false
                                );

            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();

        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Notification)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Notification> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var notification = e.Entity;
                if (notification.MessageType == "All")
                {
                    await notificationHub.SendNotificationToAll(notification.Message);
                }
                else if (notification.MessageType == "Personal")
                {
                    await notificationHub.SendNotificationToClient(notification.Message, notification.Username);
                }
            }
        }
    }
}
