using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.PowerBI.Server.Data.Context;
using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Server.Interface;
using Portal.PowerBI.Server.Services;
using Portal.PowerBI.Shared;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace Portal.PowerBI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;


        public UserController(IUserService UserService)
        {
            userService = UserService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ServiceResponse<UserLoginResponseDTO> Login(UserLoginRequestDTO userLoginRequestDTO)
        {
            ServiceResponse<UserLoginResponseDTO> response = new ServiceResponse<UserLoginResponseDTO>();

            response = userService.Login(userLoginRequestDTO.UserName, userLoginRequestDTO.Password);

            return response;
        }


        [HttpGet("Users")]
        public List<Users> GetUsers()
        {
            var db = userService.GetUsers();
            return db;
        }

        [HttpPost("UserById")]
        [AllowAnonymous]
        public UserCreatedRequestDTO GetUserById([FromBody] Guid Id)
        {
            var db = userService.GetUserById(Id);
            //ServiceResponse<UserCreatedRequestDTO> res = new ServiceResponse<UserCreatedRequestDTO>();
            //res.Value = db;

            return db;
        }
        [HttpPost("Update")]
        public async Task<ServiceResponse<UserCreatedRequestDTO>> UpdateUser([FromBody] UserCreatedRequestDTO User)
        {
            return new ServiceResponse<UserCreatedRequestDTO>()
            {
                Value = userService.UpdateUser(User)
            };
        }
        [HttpGet("Roles")]
        [AllowAnonymous]
        public List<Roles> GetRoles()
        {

            var response = userService.GetRoles();

            return response;
        }

        [HttpPost("UserCreate")]
        public ServiceResponse<UserAndRoleMappingResponse> UserAndRoleMapping(UserCreatedRequestDTO userCreated)
        {
            Guid userId = Guid.NewGuid();
            Guid roleMappingId = Guid.NewGuid();
            var response = userService.CreateUserAndRoleMapping(userCreated, userId, roleMappingId);

            return response;
        }



        [HttpPost("get-login")]
        [AllowAnonymous]
        public IActionResult GetUserInfo([FromBody] UserLoginRequestDTO userLoginRequestDTO)
        {

            ServiceResponse<UserLoginResponseDTO> response = new ServiceResponse<UserLoginResponseDTO>();

            response = userService.ADLogin(userLoginRequestDTO.UserName, userLoginRequestDTO.Password);



            return Ok(response);



        }



    }
}
