using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Incidentapi_mounaa.Models

{
    public class Incident
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "champ obligatoire")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(30)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;

        public string Severity { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}