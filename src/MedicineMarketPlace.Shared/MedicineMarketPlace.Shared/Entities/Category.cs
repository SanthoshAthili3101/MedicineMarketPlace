namespace MedicineMarketPlace.Shared.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ThumbnailImage { get; set; }
        public bool IsActive { get; set; }
    }
}
