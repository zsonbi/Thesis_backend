using System.Text.Json.Serialization;

namespace Thesis_backend.Data_Structures
{
    public record Friend : DbElement
    {
        public long SenderId { get; set; }

        public User? Sender { get; set; }

        public long ReceiverId { get; set; }
        public User? Receiver { get; set; }

        public DateTime SentTime { get; set; } = DateTime.UtcNow;

        public bool Pending { get; set; } = true;
        [JsonIgnore]
        public override object Serialize => new { ID, Sender = Sender?.Serialize, Receiver = Receiver?.Serialize, SentTime, Pending };
    }
}