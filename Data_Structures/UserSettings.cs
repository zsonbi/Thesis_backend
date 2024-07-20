using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Thesis_backend.Data_Structures
{
    public record UserSettings : DbElement
    {
        public required User User { get; set; }
        public long UserId { get; set; }

        public string? ProfilePic { get; set; }
        public long privacy { get; set; }
    }
}