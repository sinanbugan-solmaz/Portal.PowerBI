using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Data.Model
{
   public class Users
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
        public Boolean IsActive { get; set; }
       
    }
}
