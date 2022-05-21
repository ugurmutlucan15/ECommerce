using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class UserSaveModel
    {
        public int? Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }
    }
}