﻿namespace MedicineMarketPlace.BuildingBlocks.Identity.Models
{
    public class JwtConfiguration
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessTokenExpiry { get; set; }

        public int RefreshTokenExpiry { get; set; }
    }
}
