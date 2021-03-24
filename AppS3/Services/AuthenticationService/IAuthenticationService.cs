using AppS3.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<int> Register(Register register);

        Task<string> Login(Login login);

        Task Logout(string request);
    }
}
