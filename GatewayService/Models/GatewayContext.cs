using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace GatewayService.Models
{
	public class GatewayContext: IdentityDbContext<User>
	{
	
		public GatewayContext(DbContextOptions<GatewayContext> options) : base(options)
        {
		}

		public override DbSet<User>? Users { get; set; }

	
    }
}

