﻿using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Thesis_backend.Data_Structures
{
    public record Game : DbElement
    {
        public int Lvl { get; set; }
        public long UserId { get; set; }

        public User User { get; set; }
        public long CurrentXP { get; set; }
        public int NextLVLXP { get; set; }
        public int Currency { get; set; }
        public List<OwnedCar>? OwnedCars { get; set; }

        public override object Serialize => new { ID, Lvl, CurrentXP, NextLVLXP, UserId, Currency, OwnedCars = OwnedCars?.Select(car => car.Serialize).ToList() };
    }
}