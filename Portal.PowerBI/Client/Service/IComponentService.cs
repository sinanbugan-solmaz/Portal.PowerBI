using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.PowerBI.Client
{
    public interface IComponentServiceasd
    {
         Task<Components>  GetComponent(string Id);
    }
}
