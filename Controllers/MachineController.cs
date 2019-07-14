using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MachineController : ControllerBase
    {
        private readonly MachineContext _context;
        private readonly UserManager<MachineUser> _userManager;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public MachineController(MachineContext context, UserManager<MachineUser> userManager, IMapper mapper,
            LinkGenerator linkGenerator)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<UserBevarageModel>> Get()
        {
            var user = Request.HttpContext.User;

            var ub = await _userManager.FindByNameAsync(user.Identity.Name);

            var lastSelection = _context.UserBeverages.AsQueryable()
                .FirstOrDefault(p => p.User == ub);

            return Ok(lastSelection);
        }
        [HttpPost]
        public async Task<ActionResult<UserBevarageModel>> Post(UserBevarageModel model)
        {
            try
            {
                var user = Request.HttpContext.User;

                var ub = await _userManager.FindByNameAsync(user.Identity.Name);

                var lastSelection = _context.UserBeverages.AsQueryable()
                    .FirstOrDefault(p => p.User == ub);


                if (lastSelection == null)
                {
                    _context.UserBeverages.Add(new UserBeverage
                    {
                        Sugar = model.Sugar,
                        BeverageType = model.BeverageType,
                        UseOwnMug = model.UseOwnMug,
                        User = ub
                    });
                }
                else
                {
                    lastSelection.Sugar = model.Sugar;
                    lastSelection.BeverageType = model.BeverageType;
                    lastSelection.UseOwnMug = model.UseOwnMug;
                    _context.UserBeverages.Attach(lastSelection);
                }

                // Create a new Camp
                // var camp = _mapper.Map<Camp>(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    //return Created($"/api/Machine/{camp.Moniker}", _mapper.Map<CampModel>(camp));
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }
    }
}