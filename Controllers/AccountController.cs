using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DutchTreat.Controllers
{
  [ApiController]
  [Route("api/account")]
  public class AccountController : Controller
  {
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<MachineUser> _signInManager;
    private readonly UserManager<MachineUser> _userManager;
    private readonly MachineContext _context;
    private readonly IConfiguration _config;

    public AccountController(ILogger<AccountController> logger, 
      SignInManager<MachineUser> signInManager,
      UserManager<MachineUser> userManager,
      MachineContext context,
      
      IConfiguration config)
    {
      _logger = logger;
      _signInManager = signInManager;
      _userManager = userManager;
      _context = context;
      _config = config;
    }

    [HttpPost("createToken")]
    public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user != null)
        {
          var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

          if (result.Succeeded)
          {
            // Create the token
            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Email),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var caller = Request.HttpContext.Connection.LocalIpAddress;

            var machine = _context.Machines.AsQueryable().FirstOrDefault(p => p.IpAddress == caller.ToString());
            var lastSelection = _context.UserBeverages.AsQueryable().FirstOrDefault(p => p.User == user);
            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["Tokens:Issuer"],
              _config["Tokens:Audience"],
              claims,
              expires: DateTime.Now.AddHours(10),
              signingCredentials: creds);

            var results = new
            {
              token = new JwtSecurityTokenHandler().WriteToken(token),
              expiration = token.ValidTo,
              lastSelection = lastSelection,
              machine = machine
            };

            return Created("", results);
          }
        }
      }

      return BadRequest();
    }

  }
}
