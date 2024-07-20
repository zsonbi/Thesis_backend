using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thesis_backend.Data_Structures
{
    public abstract record DbElement
    {
        [JsonIgnore][Key] public long ID { get; set; }

        public virtual object Serialize { get => JsonSerializer.Serialize(this); }
    }
}