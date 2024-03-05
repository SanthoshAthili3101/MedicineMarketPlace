namespace MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities
{
    public interface IEntity
    {
        string CreatedBy { get; set; }

        DateTime CreatedDate { get; set; }

        string ModifiedBy { get; set; }

        DateTime? ModifiedDate { get; set; }

        bool ShouldHandleConcurrency();

        List<IEntity> GetAllCompositeEntities();
    }

    public interface IEntity<TPrimaryKey> : IEntity
    {
        TPrimaryKey Id { get; set; }

        object GetIdentityValue();

        new List<IEntity<TPrimaryKey>> GetAllCompositeEntities();
    }
}
