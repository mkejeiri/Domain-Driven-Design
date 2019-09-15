using SnackMachineDDD.logic.Common;

namespace SnackMachineDDD.logic.Atms
{
    //note that BalanceChangedEventHandler reside with ATM BC, that because the responsibility
    //to send event to service bus lies on BC producing it
    public class BalanceChangedEventHandler: IHandler<BalanceChangedEvent>
    {
        public void Handle(BalanceChangedEvent domainEvent)
        {
            //this is a service bus
           // EsbGateWay.Instance.SendBalanceChangedMessage(domainEvent.Delta);
        }
    }
}
 