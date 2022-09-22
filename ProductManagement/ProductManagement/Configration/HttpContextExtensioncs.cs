using ProductManagement.RequestDto;
using System.Security.Claims;

namespace ProductManagement.Configration
{
    public static class HttpContextExtensioncs
    {
        public static int GetUserId(this HttpContext context)
        {
            if (context == null || context.User == null || context.User.Identity == null || (context.User.Identity as ClaimsIdentity).Claims == null)
            {
                return 0;
            }

            try
            {
                var claims = (context.User.Identity as ClaimsIdentity).Claims.ToList();
                var userId = (claims?.Any(x => x.Type.Equals("Id") && !string.IsNullOrWhiteSpace(x.Value))??false) ? Convert.ToInt32(claims.Where(x => x.Type.Equals("Id")).FirstOrDefault()?.Value) : 0;
                return userId;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
