using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Interface
{
    public interface IComponentService
    {
        public ServiceResponse<ComponentCreatedResponseDTO> CreateComponentAndMapping(ComponentCreatedRequestDTO componentCreated, Guid componentId, Guid roleMappingId);
        List<ComponentData> GetComponents();

        Components GetComponent(Guid Id);

        ComponentData GetComponentAndRole(Guid Id);

        ServiceResponse<ComponentData> Update(ComponentData componentData);





    }
}
