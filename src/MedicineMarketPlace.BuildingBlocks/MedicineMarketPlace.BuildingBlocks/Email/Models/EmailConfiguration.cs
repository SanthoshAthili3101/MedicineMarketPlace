namespace MedicineMarketPlace.BuildingBlocks.Email.Models
{
    public class EmailConfiguration
    {
        public string Name { get; set; }

        public string From { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }
    }
}
