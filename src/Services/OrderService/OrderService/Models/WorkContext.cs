using Microsoft.AspNetCore.Http;

using OrderService.Extensions;
using OrderService.Models.Interfaces;

using System.Security.Claims;

namespace OrderService.Models
{
    public class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal Session => _httpContextAccessor?.HttpContext?.User;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId() => Session.FindFirstValue(ClaimTypes.Sid).ToInt();

        public string GetUserName() => Session.FindFirstValue(ClaimTypes.Name);

        public string GetEmail() => Session.FindFirstValue(ClaimTypes.Email);
    }
}