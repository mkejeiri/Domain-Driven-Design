namespace SnackMachineDDD.logic.Common
{
    public abstract class AggregateRoot : Entity
    {
        public virtual int Version { get; set; }
    }
}
