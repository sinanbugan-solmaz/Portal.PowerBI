using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Data.Model
{
   public class ComponentRoleMapping
    {
        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public string  ComponentName { get; set; }

        public string RoleName { get; set; }
    }
}
