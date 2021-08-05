using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portal.PowerBI.Server.Data.Context;
using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Server.Interface;
using Portal.PowerBI.Shared;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace Portal.PowerBI.Server.Services
{
    public class UserService : IUserService
    {


        private readonly PortalContext context;
        private readonly IConfiguration configuration;

        public UserService(PortalContext Context, IConfiguration Configuration)
        {
            //  mapper = Mapper;
            context = Context;
            configuration = Configuration;
        }

        public ServiceResponse<UserAndRoleMappingResponse> CreateUserAndRoleMapping(UserCreatedRequestDTO userCreated, Guid userId, Guid roleMappingId)
        {
            Users user = new Users();

            user.Id = userId;
            user.Email = userCreated.Email;
            user.UserName = userCreated.UserName;
            user.Password = userCreated.Password;
            user.CreateDate = DateTime.UtcNow;
            user.IsActive = true;
            context.Users.Add(user);
            context.SaveChanges();

            UserRoleMapping userRoleMapping = new UserRoleMapping();

            userRoleMapping.Id = roleMappingId;
            userRoleMapping.RoleId = userCreated.RoleId;
            userRoleMapping.UserId = userId;
            userRoleMapping.UserName = userCreated.UserName;
            userRoleMapping.RoleCode = context.Roles.First(x => x.Id == userCreated.RoleId).RoleCode.ToString();
            context.UserRoleMapping.Add(userRoleMapping);
            context.SaveChanges();

            return new ServiceResponse<UserAndRoleMappingResponse>
            {
                Message = "Kayıt Başarılı Bir Şekilde Oluşturuldu.",
                Success = true,
                Value = new UserAndRoleMappingResponse
                {
                    Email = userCreated.Email,
                    UserName = userCreated.UserName,
                    UserId = userId,
                    RoleId = userCreated.RoleId,
                    RoleMappingId = roleMappingId
                }
            };
        }

        public List<Users> GetUsers()
        {
            var result = context.Users
                         .Where(i => i.IsActive).ToList();

            return result;
        }

        public ServiceResponse<UserLoginResponseDTO> Login(string UserName, string Password)
        {
            // Veritabanı Kullanıcı Doğrulama İşlemleri Yapıldı.

            //  var encryptedPassword = PasswordEncrypter.Encrypt(Password);
            var result = new ServiceResponse<UserLoginResponseDTO>();

            var dbUser = context.Users.FirstOrDefault(i => i.Email == UserName && i.Password == Password);

            if (dbUser == null)
            {
                result.Success = false;
                result.Message = "Kullanıcı Bulunamadı.";
                return result;
            }



            else if (!dbUser.IsActive)
            {
                result.Success = false;
                result.Message = "Kullanıcı Silinmiş.";
                return result;
            }
            else
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(int.Parse(configuration["JwtExpiryInDays"].ToString()));
                var Role = from rm in context.UserRoleMapping join r in context.Roles on rm.RoleId equals r.Id where rm.UserId == dbUser.Id select r.RoleName;
                string roles = Role.First();

                var claims = new[]
                {
                new Claim(ClaimTypes.Email, UserName),
                new Claim(ClaimTypes.Name, dbUser.UserName),
                new Claim(ClaimTypes.UserData, dbUser.Id.ToString()),
                 new Claim(ClaimTypes.Role, Role.First().ToString())
                };

                var token = new JwtSecurityToken(configuration["JwtIssuer"], configuration["JwtAudience"], claims, null, expiry, creds);

                string a = new JwtSecurityTokenHandler().WriteToken(token);
                var res = new UserLoginResponseDTO()
                {
                    ApiToken = new JwtSecurityTokenHandler().WriteToken(token),
                    UserName = UserName
                };

                result.Value = res;
                //result.Value.ApiToken = new JwtSecurityTokenHandler().WriteToken(token).ToString();
                //result.Value.User = dbUser;

                return result;
            }

        }
        public ServiceResponse<UserLoginResponseDTO> ADLogin(string UserName, string Password)
        {
            var result = new ServiceResponse<UserLoginResponseDTO>();


            try
            {


                string Email = string.Empty;

                using (PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain, "solmaz.com.tr"))
                {
                    bool userCheck = yourDomain.ValidateCredentials(UserName, Password);
                    if (userCheck)
                    {

                        //  user.ThereUSer = true;
                        //PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, "solmaz.com.tr", userLoginRequestDTO.UserName, userLoginRequestDTO.Password);
                        //UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, System.DirectoryServices.AccountManagement.IdentityType.SamAccountName, userLoginRequestDTO.UserName);
                        //List<GroupPrincipal> principalSearchResult = userPrincipal.GetGroups().Cast<GroupPrincipal>().ToList();
                        //if (principalSearchResult.Count > 0)
                        //{
                        //    foreach (var item in principalSearchResult)
                        //    {
                        //        bool result = item.Name.StartsWith("Kep");
                        //        if (result)
                        //        {
                        //            user.UserGroups.Add(item.Name);
                        //        }
                        //    }
                        //}
                        //user.Phone = userPrincipal.VoiceTelephoneNumber;
                        //user.UserFullName = userPrincipal.Name;




                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expiry = DateTime.Now.AddDays(int.Parse(configuration["JwtExpiryInDays"].ToString()));
                        var Role = from rm in context.RoleUserMapping join r in context.Roles on rm.RoleId equals r.Id where rm.UserName == UserName select r.RoleName;
                        string roles = Role.First();

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Email, UserName + "@solmaz.com"),
                            new Claim(ClaimTypes.Name, UserName),
                            new Claim(ClaimTypes.Role, Role.First().ToString())
                        };

                        var token = new JwtSecurityToken(configuration["JwtIssuer"], configuration["JwtAudience"], claims, null, expiry, creds);

                        string a = new JwtSecurityTokenHandler().WriteToken(token);
                        var res = new UserLoginResponseDTO()
                        {
                            ApiToken = new JwtSecurityTokenHandler().WriteToken(token),
                            UserName = UserName
                        };

                        result.Value = res;
                    }


                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }

        }


        public List<Roles> GetRoles()
        {
            var roles = new List<Roles>();

            roles = context.Roles.ToList();

            return roles;
        }

        public UserCreatedRequestDTO GetUserById(Guid Id)
        {
            var db = context.Users.FirstOrDefault(x => x.Id == Id);

            UserCreatedRequestDTO userCreated = new UserCreatedRequestDTO();
            Guid newId = Guid.Parse("4e02b3e4-7785-46cd-84c6-26924f017ef2");
            Guid newuserId = Guid.Parse("4e4b321a-638e-4560-9729-18eeb1860ffd");
            userCreated.Email = db.Email;
            userCreated.Password = db.Password;
            userCreated.UserId = Id;
            userCreated.UserName = db.UserName;
            userCreated.RoleId = context.UserRoleMapping.FirstOrDefault(x => x.UserId == Id).RoleId;
            userCreated.Roles = context.Roles.ToList();

            //    "Admin";// (from c in context.UserRoleMapping join d in context.Roles on c.RoleId equals d.Id  select d.RoleName).Take(1).ToString();

            return userCreated;
        }

        public UserCreatedRequestDTO UpdateUser(UserCreatedRequestDTO User)
        {
            try
            {
                Users edituser = context.Users.FirstOrDefault(x => x.Id == User.UserId);

                edituser.Email = User.Email;
                edituser.Password = User.Password;
                edituser.UserName = User.UserName;

                context.SaveChanges();

                var map = context.UserRoleMapping.Where(x => x.UserId == User.UserId).ToList();
                UserRoleMapping userRoleMapping = new UserRoleMapping();

                if (!String.IsNullOrEmpty(map.FirstOrDefault(x => x.UserId == User.UserId).ToString()))
                {
                    userRoleMapping.RoleId = User.RoleId;
                    userRoleMapping.RoleCode = context.Roles.First(x => x.Id == User.RoleId).RoleCode.ToString();
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }



            return User;



        }
    }
}
