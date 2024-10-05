using System.Text.Json.Serialization;

namespace Thesis_backend.Data_Structures
{
    public record TaskHistory : DbElement
    {
        public required User Owner { get; set; }

        public required PlayerTask CompletedTask { get; set; }

        public long OwnerId { get; set; }

        public long TaskId { get; set; }

        public DateTime Completed { get; set; }
        [JsonIgnore]
        public override object Serialize => new { ID, OwnerId, Completed, CompletedTask.Serialize };
    }
}