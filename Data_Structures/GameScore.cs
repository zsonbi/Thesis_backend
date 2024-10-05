using System.Text.Json.Serialization;

namespace Thesis_backend.Data_Structures
{
    public record GameScore : DbElement
    {
        public required User Owner { get; set; }
        public long OwnerId { get; set; }

        public int Score { get; set; }

        public DateTime AchievedTime { get; set; }
        [JsonIgnore]
        public override object Serialize => new { ID, Owner = OwnerId, AchievedTime, Score };
    }
}