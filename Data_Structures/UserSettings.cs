using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Thesis_backend.Data_Structures
{
    public record UserSettings : DbElement
    {
        public required User User { get; set; }
        public long UserId { get; set; }

        public string? ProfilePic { get; set; }
        public long privacy { get; set; }

        public override object Serialize => new { ID, UserId, ProfilePic, privacy };
    }
}