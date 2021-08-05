using Portal.PowerBI.Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Shared.Model
{
   public class ComponentData
    {
        public Guid ComponentId { get; set; }
        public string ComponentName { get; set; }

        public string Url { get; set; }
        public string MenuView { get; set; }
        public string Content { get; set; }
        public List<string> RoleMapping { get; set; }
        public List<string> SelectedRole { get; set; }
        public int MenuState { get; set; }
        public Guid? TopMenu { get; set; }
    }
}
