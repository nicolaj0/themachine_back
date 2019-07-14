using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreCodeCamp.Data
{
  public class MachineContext :  IdentityDbContext<MachineUser>
  {
    private readonly IConfiguration _config;

    public MachineContext(DbContextOptions options, IConfiguration config) : base(options)
    {
      _config = config;
    }

    protected override void OnModelCreating(ModelBuilder bldr)
    {

      base.OnModelCreating(bldr);
    }

    public DbSet<Machine> Machines { get; set; }
    public DbSet<UserBeverage> UserBeverages { get; set; }

  
   

  }
}
