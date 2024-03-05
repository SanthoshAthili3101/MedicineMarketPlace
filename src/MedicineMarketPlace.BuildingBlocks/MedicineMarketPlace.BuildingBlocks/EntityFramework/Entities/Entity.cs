namespace MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities
{
    public class Entity : IEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual bool ShouldHandleConcurrency()
        {
            return true;
        }

        public virtual List<IEntity> GetAllCompositeEntities()
        {
            return new List<IEntity>();
        }
    }

    public class Entity<TPrimaryKey> : Entity, IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }

        public object GetIdentityValue()
        {
            return Id;
        }

        public new virtual List<IEntity<TPrimaryKey>> GetAllCompositeEntities()
        {
            return new List<IEntity<TPrimaryKey>>();
        }
    }
}
