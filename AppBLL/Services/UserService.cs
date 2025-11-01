
using AppBO.Models;
using AppBO.Utility;
using AppDAL.Db;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AppBO.DbSet.AccessControl;

namespace AppBLL.Services
{
    public class UserService
    {
        public PifErpDbContext pifErpDbContext;
        public IConfiguration configuration;

        public UserService(
            PifErpDbContext pifErpDbContext,
            IConfiguration configuration
            )
        {
            this.pifErpDbContext = pifErpDbContext;
            this.configuration = configuration;
        }

        public ApplicationUserModel GeyByApplicationUserId(long ApplicationUserId)
        {
            ApplicationUserModel model = new ApplicationUserModel();

            model = pifErpDbContext.ApplicationUsers.Where(m => m.ApplicationUserId == ApplicationUserId).MapTo<ApplicationUser, ApplicationUserModel>().FirstOrDefault();

            return model;
        }

        public string Token(string UserName, int expires)
        {

            string SigningKey = configuration["SigningKey"];
            var key = Encoding.UTF8.GetBytes(SigningKey);

            ApplicationUser user = pifErpDbContext.ApplicationUsers.Where(m => m.UserName == UserName).FirstOrDefault();

            var claims = new[]
            {
                new Claim(nameof(user.ApplicationUserId), user.ApplicationUserId.ToString()),
                new Claim(nameof(user.FullName), user.FullName),
                new Claim(nameof(user.OrganizationId), user.OrganizationId.ToString()),
                // Add any other claims as needed
            };

            var signingKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                issuer: "melabari.com",
                audience: "melabari.com",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expires),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string AuthenticationUser(string UserName, string Password)
        {
            string status = ProccessStatus.Success;

            ApplicationUser user = pifErpDbContext.ApplicationUsers
                .Where(m => m.UserName == UserName).FirstOrDefault();

            if (user != null)
            {

                if (user.Password == Password)
                {
                    status = ProccessStatus.Success;
                }
                else
                {
                    status = "Incorrect information.";
                }

            }
            else
            {
                status = "No account available.";
            }

            return status;
        }
    }
}
