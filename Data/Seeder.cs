using System;
using System.Threading.Tasks;
using CoreCodeCamp;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace CoreCodeCamp.Data
{
    public class Seeder
    {
        private readonly MachineContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<MachineUser> _userManager;

        public Seeder(MachineContext ctx, IHostingEnvironment hosting, UserManager<MachineUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            // Seed the Main User
            MachineUser user = await _userManager.FindByEmailAsync("shawn@dutchtreat.com");
            if (user == null)
            {
                user = new MachineUser()
                {
                    LastName = "ju",
                    FirstName = "ju",
                    Email = "ju@ju.com",
                    UserName = "ju@ju.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create user in Seeding");
                }
            }
            var machine = new  Machine
            {
                IpAddress = "127.0.0.1",
                Name = "TheMachine_1"
            };

            _ctx.Machines.Add(machine);


            var userBeverage = new UserBeverage
            {
                Sugar = 25,
                User = user,
                BeverageType = 2,
                UseOwnMug = false
            };

            _ctx.UserBeverages.Add(userBeverage);

            _ctx.SaveChanges();
        }
    }
}