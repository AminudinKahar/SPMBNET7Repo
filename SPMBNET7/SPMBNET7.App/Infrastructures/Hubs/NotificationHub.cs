using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Services;

namespace SPMBNET7.App.Infrastructures.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ApplicationDbContext dbContext;
        protected IHubContext<NotificationHub> _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationHub(ApplicationDbContext context,
                               IHubContext<NotificationHub> hubContext,
                               UserManager<IdentityUser> userManager)
        {
            dbContext = context;
            _context = hubContext;
            _userManager = userManager;
        }
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnConnected");
            return base.OnConnectedAsync();
        }

        public async Task SendNotificationToAll(string message)
        {
            var client = _context.Clients.All;
            await client.SendAsync("ReceivedNotification", message);
        }

        public async Task SendNotificationToClient(string message, string username)
        {
            var hubConnections = dbContext.HubConnection.Where(con => con.Username == username).ToList();
            foreach (var hubConnection in hubConnections)
            {
                var client = _context.Clients.Client(hubConnection.ConnectionId);

                var name = dbContext.applicationUsers.FirstOrDefault(user => user.UserName == username)!.Nama;
                await client.SendAsync("ReceivedPersonalNotification", message, name);
            }
        }

        public async Task InsertPushNotificationToAllSupervisors()
        {
            var users = await _userManager.GetUsersInRoleAsync("Supervisor");
            foreach (var user in users)
            {
                await _context.Clients.All.SendAsync("LoadPushNotification");
            }
            
        }

        public async Task SaveUserConnection(string username)
        {
            var connectionId = Context.ConnectionId;
            HubConnection hubConnection = new HubConnection
            {
                ConnectionId = connectionId,
                Username = username
            };

            dbContext.HubConnection.Add(hubConnection);
            await dbContext.SaveChangesAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var hubConnection = dbContext.HubConnection.FirstOrDefault(con => con.ConnectionId == Context.ConnectionId);
            if (hubConnection != null)
            {
                dbContext.HubConnection.Remove(hubConnection);
                dbContext.SaveChanges();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
