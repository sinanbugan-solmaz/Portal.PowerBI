using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Shared.Model
{
   public class ComponentCreatedRequestDTO
    {
        public string ComponentName { get; set; }
        public string url { get; set; }
        public string MenuName { get; set; }
        public string Content { get; set; }
        public string RoleName { get; set; }
        public List<string> Roles { get; set; }
        public string TopMenu { get; set; }
    }
}
