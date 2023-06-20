using SPMBNET7.CoreBusiness._Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Services
{
    public class PushNotification
    {
        public int id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public JenisPemberitahuan MessageType { get; set; } = JenisPemberitahuan.All;
        public DateTime NotificationDateTime { get; set; } = DateTime.Now;
    }
}
