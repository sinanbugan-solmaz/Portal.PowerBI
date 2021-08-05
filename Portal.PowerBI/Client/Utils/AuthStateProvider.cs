using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Portal.PowerBI.Server.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portal.PowerBI.Client
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient client;
        private readonly AuthenticationState anonim;
      //  private readonly PortalContext context;
        public AuthStateProvider(ILocalStorageService _localstorageservice, HttpClient Client/*,PortalContext _context*/)
        {
            localStorageService = _localstorageservice;
            anonim = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            client = Client;
           // context = _context;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string apitoken = await localStorageService.GetItemAsStringAsync("token");
            if (string.IsNullOrEmpty(apitoken))
            {
                return anonim;
            }

            string username = await localStorageService.GetItemAsStringAsync("username");
            //string UserId = await localStorageService.GetItemAsStringAsync("UserId");
            //Guid id =Guid.Parse(UserId);
          
            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username),
               
               
            }, "jwtAuthType"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apitoken);

            return await Task.FromResult( new AuthenticationState(cp));
        }

        public  void NotifyUserLogin(String username)
        {

            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[] { 
                new Claim(ClaimTypes.Name, username)
            }, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(cp));

            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(anonim);
            NotifyAuthenticationStateChanged(authState);
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            ExtractRolesFromJWT(claims, keyValuePairs);

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }
        private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');

                if (parsedRoles.Length > 1)
                {
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }
        }

        public void NotifyUserAuthentication(string token)
        {
           // var a = ParseClaimsFromJwt(token);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private static byte[] ParseBase64WithoutPadding(string payload)
        {
            payload = payload.Replace('-', '+').Replace('_', '/');
            var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            return Convert.FromBase64String(base64);
        }
    }
}
