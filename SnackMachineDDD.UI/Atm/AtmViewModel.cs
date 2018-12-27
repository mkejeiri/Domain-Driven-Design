using System;
using SnackMachineDDD.logic.Atms;
using SnackMachineDDD.logic.Management;
using SnackMachineDDD.logic.SharedKernel;
using SnackMachineDDD.UI.Common;

namespace SnackMachineDDD.UI.Atm
{
    public class AtmViewModel : ViewModel
    {
        private readonly PaymentGateway _paymentGateway;
        private readonly AtmRepository _repository;
        private readonly logic.Atms.Atm _atm;

        public override string Caption => "ATM";
        public Money MoneyInside => _atm.MoneyInside;
        public string MoneyCharged => _atm.MoneyCharged.ToString("C2");

        public Command<decimal> TakeMoneyCommand { get; private set;}
        private string _message;

        public string Message
        {
            get { return _message;}
            set
            {
                _message = value; 
                Notify();
            }
        }

        public AtmViewModel(logic.Atms.Atm atm)
        {
            _atm = atm ;
            _repository = new AtmRepository();
            _paymentGateway = new PaymentGateway();
            //this will be executed only if amount is > 0
            TakeMoneyCommand = new Command<decimal>(x=>x>0, TakeMoney);
        }

        
        private void TakeMoney(decimal amount)
        {
            string error = _atm.CanTakeMoney(amount);
            if (error!= string.Empty)
            {
                NotifyClient(error);
                return;
            }

            decimal amountWithCommission = _atm.CalculateAmountWithCommission(amount);
            _paymentGateway.ChargedPayment(amountWithCommission);
            _atm.TakeMoney(amount);
            _repository.Save(_atm);

            /*Option 1 :
                         HeadOffice headOffice = GetHeadOfficeInstance();
                        headOffice.Balance += amountWithCommission;
                        _headOfficeRepository.Save(headOffice);
          
            Drawbacks: 
                a - coupling HeadOffice to viewModel entity, i.e. additional dependency between ATM and mgnt bounded context
                    not only mgt knows about ATM but also ATM becomes aware of mngt => bidirectional coupling!!!

                b - if we add another viewAtmModel we should also repeat our selves transfering the balance to the head office. 
                    => Error prone due to code duplication and possible human factor!
             */

            //OR

            /*Option 2 :  TakeMoney adding  headOffice as param :  TakeMoney(decimal amount, HeadOffice headOffice)                      
                       headOffice.Balance += amountWithCommission;

               Pros: it allows us to eliminate code duplicate but...
               Drawbacks: 
                   a - still bidirectional coupling between ATM and mngt bounded contexts => should be avoided
                       not only mgt knows about ATM but also ATM becomes aware of mngt => bidirectional coupling!!!

                   b - ATM entity gets responsability which not related to ATM itself, it's not up to ATM to increase 
                   the balance of the headoffice  
            */
            /********************************************************************************
             * Option 3 : decoupling of the bounded context through the use of Domain events
                - ATM raise event when cash withdrawal
                - Mngt should subscribe to such events
             ********************************************************************************/

            NotifyClient("You've taken " + amount.ToString("C2"));
        }

        private void NotifyClient(string message)
        {
            Message = message;
            Notify(nameof(MoneyInside));
            Notify(nameof(MoneyCharged));
        }
    }
}
 