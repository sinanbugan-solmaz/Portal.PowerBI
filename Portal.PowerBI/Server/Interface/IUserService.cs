using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Shared;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Portal.PowerBI.Server.Interface
{
    public interface IUserService
    {
        public List<Users> GetUsers();
        public ServiceResponse<UserLoginResponseDTO> Login(string UserName, string Password);
        public ServiceResponse<UserLoginResponseDTO> ADLogin(string Email, string Password);

        public ServiceResponse<UserAndRoleMappingResponse> CreateUserAndRoleMapping(UserCreatedRequestDTO userCreated, Guid userId, Guid roleMappingId);
        List<Roles> GetRoles();

        public UserCreatedRequestDTO GetUserById(Guid Id);

        public UserCreatedRequestDTO UpdateUser(UserCreatedRequestDTO User);

    }
}
