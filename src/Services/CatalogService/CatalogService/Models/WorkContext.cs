using CatalogService.Extensions;
using CatalogService.Models.Interfaces;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace CatalogService.Models
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