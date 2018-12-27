using SnackMachineDDD.logic.Common;

namespace SnackMachineDDD.logic.Atms
{
    public class BalanceChangedEvent : IDomainEvent
    {
        public decimal Delta { set; get; }
        public BalanceChangedEvent(decimal delta)
        {
            Delta = delta;
        }
    }
}
