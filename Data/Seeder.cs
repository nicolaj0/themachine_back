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
        private readonly CampContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public Seeder(CampContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            // Seed the Main User
            StoreUser user = await _userManager.FindByEmailAsync("shawn@dutchtreat.com");
            if (user == null)
            {
                user = new StoreUser()
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

            _ctx.SaveChanges();
        }
    }
}