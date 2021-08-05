using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Shared.Model
{
    public class UserAndRoleMappingResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid RoleMappingId { get; set; }
        public Guid RoleId { get; set; }
    }
}
