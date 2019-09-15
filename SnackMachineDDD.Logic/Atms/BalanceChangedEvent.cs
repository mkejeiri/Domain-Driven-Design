using SnackMachineDDD.logic.Common;

namespace SnackMachineDDD.logic.Atms
{
    //BalanceChangedEventHandler consumes the BalanceChangedEvent, thus it resides in the consumer BC 
    //while BalanceChangedEvent resides in the producer BC where it raised from.
    public class BalanceChangedEvent : IDomainEvent
    {
        public decimal Delta { set; get; }
        public BalanceChangedEvent(decimal delta)
        {
            Delta = delta;
        }
    }
}
