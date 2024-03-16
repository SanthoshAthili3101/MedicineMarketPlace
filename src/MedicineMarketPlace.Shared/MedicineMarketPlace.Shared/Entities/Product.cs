namespace MedicineMarketPlace.Shared.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int LimitPerUser { get; set; }
        public string Brand { get; set; }
        public long GTIN { get; set; }
        public long Pipcode { get; set; }
        public double RegularPrice { get; set; }
        public double SalePrice { get; set; }
        public int TaxstatusId { get; set; }
        public int TaxclassId { get; set; }
        public long SKU { get; set; }
        public int Quantity { get; set; }
    }
}
