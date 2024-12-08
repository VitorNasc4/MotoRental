using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.DTOs
{
    public class NotificationInfoDTO
    {
        public NotificationInfoDTO(int idUser, string fullName, string email)
        {
            IdUser = idUser;
            FullName = fullName;
            Email = email;
        }
        public int IdUser { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}