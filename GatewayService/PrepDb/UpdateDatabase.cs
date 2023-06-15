
using GatewayService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GatewayService.PrepDb
{
    public static class UpdateDatabase{
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<GatewayContext>();
                if (context != null && context.Database != null)
                {
                    context.Database.Migrate();
                }
            }
        }


    }
}