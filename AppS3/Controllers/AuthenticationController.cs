using AppS3.DTOs;
using AppS3.Services.AuthenticationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(Register register)
        {
            var result = await _authenticationService.Register(register);

            if (result == 0)
            {
                return BadRequest(new { msg = "Password and PasswordConfirm doesn't matched!" });
            }

            if (result == 1)
            {
                return BadRequest(new { msg = "Account already exist!" });
            }

            if (result == 2)
            {
                return BadRequest(new { msg = "Register Failed!" });
            }
            if (result == 3)
            {
                return Ok(new { msg = "Register Successful!" });
            }

            return BadRequest(new { msg = "Execption!" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(Login login)
        {
            var result = await _authenticationService.Login(login);

            if (string.IsNullOrEmpty(result))
            {
                return BadRequest(new { msg = "Login Failed!" });
            }
            
            return Ok(new { token = result, msg = "Login Successful" });
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> Logout(string request)
        {
            await _authenticationService.Logout(request);
            return Ok();
        }
    }
}
