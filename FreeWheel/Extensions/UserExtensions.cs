using System;
using System.Security.Claims;

namespace FreeWheel.Extensions
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user) {
            return Convert.ToInt32(((ClaimsIdentity)user.Identity).FindFirst("uid").Value);
        }
    }
}
