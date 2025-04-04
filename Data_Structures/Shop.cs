﻿using System.Text.Json.Serialization;

namespace Thesis_backend.Data_Structures
{
    public record Shop : DbElement
    {
        public string? ProductName { get; set; }
        public int Cost { get; set; }
        public CarType CarType { get; set; }

        public bool Buyable { get; set; } = false;

        [JsonIgnore]
        public override object Serialize => new { ID, ProductName, Cost, CarType, Buyable };
    }
}