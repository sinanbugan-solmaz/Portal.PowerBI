using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Data.Model
{
    public class RoleUserMapping
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }


    }
}
