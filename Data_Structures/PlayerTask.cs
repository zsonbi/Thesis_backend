using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Thesis_backend.Data_Structures
{
    public record PlayerTask : DbElement
    {
        public required User TaskOwner { get; set; }
        public required string TaskName { get; set; }
        public string? Description { get; set; }
        public bool TaskType { get; set; }
        public required int PeriodRate { get; set; }
        public DateTime Updated { get; set; }
        public DateTime LastCompleted { get; set; }
        public bool Completed { get; set; }
        public bool Deleted { get; set; } = false;
        [JsonIgnore]
        public override object Serialize => new { ID, TaskName, Description, TaskType, PeriodRate, Updated, LastCompleted, Completed, Deleted };
    }
}