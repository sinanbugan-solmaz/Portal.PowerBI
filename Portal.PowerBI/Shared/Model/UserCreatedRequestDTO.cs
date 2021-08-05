using Portal.PowerBI.Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Shared.Model
{
   public class UserCreatedRequestDTO
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Roles> Roles { get; set; }
    }
}
