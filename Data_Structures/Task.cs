using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Thesis_backend.Data_Structures
{
    public record Task : DbElement
    {
        public required User TaskOwner { get; set; }
        public required string TaskName { get; set; }
        public bool TaskType { get; set; }
        public int PeriodRate { get; set; }
        public DateTime Added { get; set; }
        public DateTime LastCompleted { get; set; }
        public bool Completed { get; set; }

        public override object Serialize => new { ID, TaskType, PeriodRate, Added, LastCompleted, Completed };
    }
}