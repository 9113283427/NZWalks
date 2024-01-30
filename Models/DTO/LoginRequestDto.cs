using System.ComponentModel.DataAnnotations;

namespace NZWalks2.API.Models.DTO
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string JwtToken { get; internal set; }
    }
}
