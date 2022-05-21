using CartService.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendMail(string email, IEnumerable<Cart> orders);
    }
}