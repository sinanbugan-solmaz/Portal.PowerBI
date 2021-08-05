using Portal.PowerBI.Server.Data.Context;
using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Server.Interface;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Services
{
    public class ComponentService : IComponentService
    {
        private readonly PortalContext context;
        public ComponentService(PortalContext _context)
        {
            context = _context;
        }
        public ServiceResponse<ComponentCreatedResponseDTO> CreateComponentAndMapping(ComponentCreatedRequestDTO componentCreated, Guid componentId, Guid roleMappingId)
        {
            Components component = new Components();

            component.Id = componentId;
            component.ComponentName = componentCreated.ComponentName;
            component.Content = componentCreated.Content;
            component.MenuName = componentCreated.MenuName;
            component.url = componentCreated.url;
            component.TopMenu = Guid.Parse((from db in context.Components where db.ComponentName == componentCreated.TopMenu select db.Id).ToString());

            context.Components.Add(component);
            context.SaveChanges();
            if (!String.IsNullOrEmpty(componentCreated.RoleName))
            {
                ComponentRoleMapping componentRoleMapping = new ComponentRoleMapping();

                componentRoleMapping.ComponentId = componentId;
                componentRoleMapping.ComponentName = componentCreated.ComponentName;
                componentRoleMapping.Id = roleMappingId;
                componentRoleMapping.RoleName = componentCreated.RoleName;

                context.ComponentRoleMapping.Add(componentRoleMapping);
                context.SaveChanges();
            }
            if (componentCreated.RoleName != "Admin")
            {
                ComponentRoleMapping componentRoleMappingAdmin = new ComponentRoleMapping();

                componentRoleMappingAdmin.ComponentId = componentId;
                componentRoleMappingAdmin.ComponentName = componentCreated.ComponentName;
                componentRoleMappingAdmin.Id = Guid.NewGuid();
                componentRoleMappingAdmin.RoleName = "Admin";

                context.ComponentRoleMapping.Add(componentRoleMappingAdmin);
                context.SaveChanges();
            }


            return new ServiceResponse<ComponentCreatedResponseDTO>
            {
                Message = "Kayıt Başarılı Bir Şekilde Oluşturuldu.",
                Success = true,
                Value = new ComponentCreatedResponseDTO
                {
                    ComponentName = componentCreated.ComponentName

                }
            };
        }

        public Components GetComponent(Guid Id)
        {

            var db = context.Components.FirstOrDefault(x => x.Id == Id);

            return db;
        }
        public ComponentData GetComponentAndRole(Guid Id)
        {
            var db = context.Components.FirstOrDefault(x => x.Id == Id);

            return new ComponentData
            {
                ComponentId = db.Id,
                ComponentName = db.ComponentName,
                Content = db.Content,
                MenuView = db.MenuName,
                Url = db.url,
                RoleMapping = (from c in context.Roles select c.RoleName).ToList(),
                SelectedRole=(from c in context.Roles join m in context.ComponentRoleMapping on c.RoleName equals m.RoleName where m.ComponentId==db.Id select c.RoleName).ToList()
            };
        }

        public List<ComponentData> GetComponents()
        {

            var componentlist = new List<ComponentData>();
            var db = context.Components.ToList();

            foreach (var item in db)
            {
                var components = new ComponentData();
                components.ComponentId = item.Id;
                components.ComponentName = item.ComponentName;
                components.Content = item.Content;
                components.Url = item.url;
                components.MenuView = item.MenuName;
                components.RoleMapping = (from c in context.ComponentRoleMapping where c.ComponentId == item.Id select c.RoleName).ToList();
                components.MenuState = item.MenuState;
                components.TopMenu = item.TopMenu;
                componentlist.Add(components);
            }

            return componentlist;
        }

        public ServiceResponse<ComponentData> Update(ComponentData componentData)
        {

            var cmp = context.Components.FirstOrDefault(x => x.Id == componentData.ComponentId);

            cmp.ComponentName = componentData.ComponentName;
            cmp.Content = componentData.Content;
            cmp.MenuName = componentData.MenuView;
            cmp.url = componentData.Url;
            cmp.TopMenu = Guid.Parse((from db in context.Components where db.ComponentName==componentData.TopMenu.ToString() select db.Id).ToString());
            context.SaveChanges();

            ComponentRoleMapping cmpRoleMapping = new ComponentRoleMapping();

            foreach (var item in context.ComponentRoleMapping.Where(x=>x.ComponentId==componentData.ComponentId))
            {
                if (!item.RoleName.Contains(componentData.SelectedRole.ToString()))
                {
                    context.ComponentRoleMapping.Remove(item);
                    context.SaveChanges();
                }
            }

            foreach (var item in componentData.SelectedRole)
            {
                var cmpMap = context.ComponentRoleMapping.First(x => x.ComponentId == componentData.ComponentId && x.RoleName == item).ToString();
                if (String.IsNullOrEmpty(cmpMap))
                {
                    cmpRoleMapping.RoleName = item;
                    cmpRoleMapping.ComponentId = componentData.ComponentId;
                    cmpRoleMapping.ComponentName = componentData.ComponentName;
                    cmpRoleMapping.Id = Guid.NewGuid();
                    context.SaveChanges();
                }

            }

            return new ServiceResponse<ComponentData>
            {
                Message = "Kayıt başarılı",
                Success = true,
                Value = componentData
            };

        }
    }
}
