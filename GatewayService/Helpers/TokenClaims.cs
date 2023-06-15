using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GatewayService.Helpers
{
    public static class TokenClaims
    {

        public static string GetLoggedInUserId(HttpContext httpContext)
        {
            //get the logged in customer ID
          string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return userId;
        }

        public static ClaimsPrincipal GetLoggedInUser(HttpContext httpContext)
        {
            //get the logged in customer ID
            var user = httpContext.User;
            return user;
        }



    }
}
