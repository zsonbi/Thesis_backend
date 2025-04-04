﻿using System.Text.Json.Serialization;
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
        public Game? Game { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public DateTime Registered { get; set; }
        public List<PlayerTask>? UserTasks { get; set; }

        public long TotalScore { get; set; } = 0;
        public long CurrentTaskScore { get; set; } = 0;
        public int CompletedGoodTasks { get; set; } = 0;
        public int CompletedBadTasks { get; set; } = 0;

        [JsonIgnore]
        public override object Serialize => new { ID, Username, PasswordHash, Email, userSettings = UserSettings?.Serialize, game = Game?.Serialize, LastLoggedIn, Registered, UserTasks, TotalScore, CurrentTaskScore, CompletedGoodTasks, CompletedBadTasks };
    }
}