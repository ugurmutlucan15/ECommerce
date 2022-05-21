using LoginService.Models;

namespace LoginService.Services.Interfaces
{
    public interface IUserService
    {
        dynamic CreateAccessToken(UserViewModel model);
    }
}