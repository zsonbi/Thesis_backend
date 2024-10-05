namespace Thesis_backend.Data_Structures
{
    public record GameScore : DbElement
    {
        public required User Owner { get; set; }
        public long OwnerId { get; set; }

        public int Score { get; set; }

        public DateTime AchievedTime { get; set; }

        public override object Serialize => new { ID, Owner = OwnerId, AchievedTime, Score };
    }
}