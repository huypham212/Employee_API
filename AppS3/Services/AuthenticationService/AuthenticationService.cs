using AppS3.DTOs;
using AppS3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AppS3.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDistributedCache _cache;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(IDistributedCache cache, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _cache = cache;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<string> Login(Login login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user == null)
            {
                return null;
            }

            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            userRoles.ToList().ForEach(e =>
            {
                authClaims.Add(new Claim(ClaimTypes.Role, e));
            });

            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecretKey"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task Logout(string request)
        {
            await _cache.SetStringAsync($"tokens:{request}:deactivated", " ", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            await _signInManager.SignOutAsync();   
        }

        public async Task<int> Register(Register register)
        {
            if(register.Password != register.ConfirmPassword)
            {
                return 0;
            }

            using(var transaction = new CommittableTransaction(new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    var userExist = await _userManager.FindByNameAsync(register.UserName);

                    if(userExist != null)
                    {
                        return 1;
                    }

                    var user = new User()
                    {
                        UserName = register.UserName,
                        Email = register.Email,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var result = await _userManager.CreateAsync(user, register.Password);

                    if (!await _roleManager.RoleExistsAsync(Role.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
                    }

                    if (!await _roleManager.RoleExistsAsync(Role.Employee))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Role.Employee));
                    }
                    
                    await _userManager.AddToRoleAsync(user, register.Roles);
                    

                    transaction.Commit();

                    if (!result.Succeeded)
                    {
                        return 2;
                    }

                    return 3;
                }catch(Exception e)
                {
                    return 4;
                }

            }
        }
    }
}
