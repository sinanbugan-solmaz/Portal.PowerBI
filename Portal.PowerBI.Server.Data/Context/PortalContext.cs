using Microsoft.EntityFrameworkCore;
using Portal.PowerBI.Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Data.Context
{
    public class PortalContext : DbContext
    {
        public PortalContext(DbContextOptions<PortalContext> options): base(options)
        {

        }        


        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public virtual DbSet<Components> Components { get; set; }

        public virtual DbSet<ComponentRoleMapping> ComponentRoleMapping { get; set; }
        public virtual DbSet<RoleUserMapping> RoleUserMapping { get; set; }
    }
}
