using Portal.PowerBI.Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Interface
{
   public interface IRoleService
    {
        List<Roles> GetRoles();

    }
}
