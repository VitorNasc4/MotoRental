using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Core.DTOs;

namespace MotoRental.Core.Services
{
    public interface INotificationService
    {
        void ProcessNotification(NotificationInfoDTO notificationInfoDTO);
    }
}