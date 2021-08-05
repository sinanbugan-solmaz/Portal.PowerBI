using Portal.PowerBI.Server.Data.Context;
using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Services
{
    public class RoleService : Interface.IRoleService
    {
        private readonly PortalContext context;
        public RoleService( PortalContext _context)
        {
            context = _context;
        }
        public List<Roles> GetRoles()
        {
            var roles = new List<Roles>();

            roles = context.Roles.ToList();

            return roles;
        }
    }
}
