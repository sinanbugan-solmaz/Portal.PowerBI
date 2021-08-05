using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Server.Interface;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : ControllerBase
    {

        private readonly IComponentService componentService;

        public ComponentController(IComponentService _componentservice)
        {
            componentService = _componentservice;
        }


        [HttpPost("ComponentCreate")]
        public ServiceResponse<ComponentCreatedResponseDTO> ComponentAndRoleMapping(ComponentCreatedRequestDTO componentCreated)
        {
            Guid componentId = Guid.NewGuid();
            Guid roleMappingId = Guid.NewGuid();
            var response = componentService.CreateComponentAndMapping(componentCreated, componentId, roleMappingId);

            return response;
        }

        [HttpGet("Components")]
        [AllowAnonymous]
        public List<ComponentData> GetComponents()
        {

            var response = componentService.GetComponents();

            return response;
        }

        [HttpPost("ComponentData")]
        [AllowAnonymous]
        public Components GetComponent([FromBody] Guid Id)
        {
           // Guid cmpId = Guid.Parse(Id);
            var response = componentService.GetComponent(Id);

            return response;
        }

        [HttpPost("ComponentAndRole")]
        [AllowAnonymous]
        public ComponentData GetComponentAndRole([FromBody] Guid Id)
        {
            // Guid cmpId = Guid.Parse(Id);
            var response = componentService.GetComponentAndRole(Id);

            return response;
        }
        [HttpPost("Update")]
        public async Task<ServiceResponse<ComponentData>> UpdateUser([FromBody] ComponentData componentData)
        {
            var result = componentService.Update(componentData);

            return result;
        }

    }
}
