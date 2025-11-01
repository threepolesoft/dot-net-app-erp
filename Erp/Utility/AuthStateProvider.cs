using AppBO.Models;
using Erp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Erp.Utility
{
    public class AuthStateProvider : AuthenticationStateProvider
    {

        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        private readonly LocalStorageService localStorageService;

        public AuthStateProvider(
            LocalStorageService localStorageService
            )

        {
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Retrieve session data from server-side session storage
                var session = await localStorageService.GetItemAsync<UserSessionModel>("UserSession");

                var userSession = session;

                if (session == null || IsTokenExpired(userSession.Token))
                    return new AuthenticationState(anonymous);

                // Create claims based on the user session
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(nameof(userSession.Token), userSession.Token),
                };

                foreach (var role in userSession.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth"));
                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                return new AuthenticationState(anonymous);
            }
        }

        public async Task UpdateAuthenticationState(UserSessionModel userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (!string.IsNullOrEmpty(userSession.UserName))
            {
                // Serialize the UserSession and store it in server-side session storage
                await localStorageService.SetItemAsync("UserSession", userSession);

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(nameof(userSession.Token), userSession.Token),
                };

                foreach (var role in userSession.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth"));
            }
            else
            {
                // If no user session, clear the session and set to anonymous
                await localStorageService.RemoveItem("UserSession");
                claimsPrincipal = anonymous;
            }

            // Notify that the authentication state has changed
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        // Method to check if the JWT token is expired
        private bool IsTokenExpired(string token)
        {
            if (string.IsNullOrEmpty(token))
                return true;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return true;

            var expirationDate = jwtToken?.ValidTo;

            // Check if token is expired
            return expirationDate < DateTime.UtcNow;
        }
    }
}
