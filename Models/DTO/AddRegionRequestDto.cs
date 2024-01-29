using System.ComponentModel.DataAnnotations;

namespace NZWalks2.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code Has to be minimum of 3 character")]
        [MaxLength(3, ErrorMessage = "Code Has to be maximum of 3 character")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be maximum of 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
