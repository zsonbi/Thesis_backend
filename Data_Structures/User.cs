using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Thesis_backend.Data_Structures
{
    public record User : DbElement
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserSettings? UserSettings { get; set; }
        public long GameId { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public DateTime Registered { get; set; }
        public List<Task>? UserTasks { get; set; }

        public long TotalScore { get; set; }
        public long Currency { get; set; }

        [JsonIgnore]
        public override object Serialize => new { ID, Username, PasswordHash, Email, UserSettings?.Serialize, GameId, LastLoggedIn, Registered, UserTasks, TotalScore, Currency };
    }
}