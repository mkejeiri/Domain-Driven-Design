using SnackMachineDDD.logic.Atms;
using SnackMachineDDD.logic.Common;

namespace SnackMachineDDD.logic.Management
{
    /******************************************************************
     BalanceChangedEventHandler consumes the BalanceChangedEvent, thus it resides in the consumer BC 
     while BalanceChangedEvent resides in the producer BC where it raised from.
     Handlers delegate the actual work to other domain classes. 
     Event handlers are just like domain services      
     *******************************************************************/
    public class BalanceChangedEventHandler : IHandler<BalanceChangedEvent>
    {
        public void Handle(BalanceChangedEvent domainEvent)
        {
            HeadOffice headOffice = HeadOfficeInstance.Instance;
            headOffice.ChangeBalance(domainEvent.Delta);
            var  repository = new HeadOfficeRepository();
            repository.Save(headOffice);
        }
    }
}
