using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Portal.PowerBI.Server.Data.Context
{
    public class DesignDbContext : IDesignTimeDbContextFactory<PortalContext>
    {
        public PortalContext CreateDbContext(string[] args)
        {
            string connectionstring = "Host=172.16.34.162;Database=PowerBI-Portal;Username=solmaztms;Password=Solmaz2021-";
            var builder = new DbContextOptionsBuilder<PortalContext>();
            builder.UseNpgsql(connectionstring);
            return new PortalContext(builder.Options);


        }
    }
}
