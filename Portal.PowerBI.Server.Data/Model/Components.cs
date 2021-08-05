using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Data.Model
{
    public class Components
    {
        public Guid Id { get; set; }
        public string ComponentName { get; set; }
        public string url { get; set; }
        public string MenuName { get; set; }
        public string Content { get; set; }
        public Guid? TopMenu { get; set; }
        public int MenuState { get; set; }

     //   public virtual ComponentRoleMapping ComponentRoleMapping { get; set; }
    }
}
