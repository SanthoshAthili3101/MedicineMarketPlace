namespace MedicineMarketPlace.BuildingBlocks.Extensions
{
    public static class OtpGenerateExtension
    {
        public static int GenerateVerificationCode()
        {
            var rnd = new Random();
            return rnd.Next(100000, 999999);
        }
    }
}
