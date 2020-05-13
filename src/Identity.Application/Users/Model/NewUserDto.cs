using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Users.Model
{
    public class NewUserDto
    {
        private const int _defaultSystem = 1;

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
        public int System { get; set; } = _defaultSystem;
    } 

}