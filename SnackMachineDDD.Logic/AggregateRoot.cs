namespace SnackMachineDDD.logic
{
    public abstract class AggregateRoot : Entity
    {
        public virtual int Version { get; set; }
    }
}
