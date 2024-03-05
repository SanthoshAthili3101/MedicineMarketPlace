namespace MedicineMarketPlace.BuildingBlocks.Extensions
{
    public static class RandomExtensions
    {
        public static int NextRandomEven(this Random random)
        {
            int randomValue = random.Next(1, 1000);
            return randomValue % 2 == 0 ? randomValue : randomValue + 1;
        }

        public static int NextRandomOdd(this Random random)
        {
            int randomValue = random.Next(1, 1000);
            return randomValue % 2 != 0 ? randomValue : randomValue + 1;
        }
    }
}
