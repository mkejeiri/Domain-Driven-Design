namespace SnackMachineDDD.logic.Common
{
    public interface IHandler<T>
    where T: IDomainEvent
    {
        void Handle(T IDomainEvent);
    }
}
