using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using SnackMachineDDD.logic;
using SnackMachineDDD.UI.Common;

/*
    1- ViewModel (e.g SnackMachineViewModel) acts as a wrapper on top of the entity. It augments it 
       with the functionality required for the view. It doesn't contain any business logic, but rather delegated it to the entity.

    2- This an application layer (ViewModel or Controllers in MVC):
        - Acts a mediator between the outside world (Domain layer and UI)
        - Shouldn't contain any business logic!
        - Only coordinates the communications between different elements of the Domain and validates the input from the UI. 
    3-  application layer also works with other domain classes such as repositories. 
    
    I M P O R T A N T : rule of thumb to decide where to put the business logic
    - Put business logic in app layer when only it makes sense for the UI!
    - otherwise all business logic should go to the domain layer (Entities, ValueObjects, ...).
 */

namespace SnackMachineDDD.UI
{
    public class SnackMachineViewModel : ViewModel
    {
        private readonly SnackMachine _snackMachine;
        private readonly SnackMachineRepository _snackMachineRepository;
        public override string Caption => "Snack Machine";
        public string MoneyInTransaction => _snackMachine.MoneyInTransaction.ToString();
        //No need to sum both, we already loaded the money into SnackMachine
        //public Money MoneyInside => _snackMachine.MoneyInside + _snackMachine.MoneyInTransaction;
        public Money MoneyInside => _snackMachine.MoneyInside;

        public IReadOnlyList<SnackPileViewModel> Piles
        {
            get
            {
                return _snackMachine.GetAllSnackPiles()
                    .Select(x => new SnackPileViewModel(x))
                    .ToList();
            }
        }


        public Command InsertCentCommand { get; private set; }
        public Command InsertTenCentCommand { get; private set; }
        public Command InsertQuarterCommand { get; private set; }
        public Command InsertDollarCommand { get; private set; }
        public Command InsertFiveDollarCommand { get; private set; }
        public Command InsertTwentyDollarCommand { get; private set; }
        public Command ReturnMoneyCommand { get; private set; }
        public Command<string> BuySnackCommand { get; private set; }
        private string _message ="";
        public string Message
        {
            get => _message;
            set {
                _message = value;
                //name of prop not specified, automatically notify method will pickup 
                //the default name.
                Notify();
            }
        }



        public SnackMachineViewModel(SnackMachine snackMachine)
        {
            _snackMachine = snackMachine ?? throw new ArgumentNullException(nameof(snackMachine));
            _snackMachineRepository = new SnackMachineRepository();

            InsertCentCommand = new Command(() => InsertMoney(Money.Cent));
            InsertTenCentCommand = new Command(() => InsertMoney(Money.TenCent));
            InsertQuarterCommand = new Command(() => InsertMoney(Money.Quarter));
            InsertDollarCommand = new Command(() => InsertMoney(Money.OneDollar));
            InsertFiveDollarCommand = new Command(() => InsertMoney(Money.FiveDollar));
            InsertTwentyDollarCommand = new Command(() => InsertMoney(Money.TwentyDollar));
            ReturnMoneyCommand = new Command(()=> ReturnMoney());
            BuySnackCommand = new Command<string>( BuySnack);
        }

        private void BuySnack(string positionString)
        {
            int position = int.Parse(positionString);
            string error = _snackMachine.CanBuySnack(position);
            if (error !=string.Empty)
            {
                NotifyClient(error, Money.None);
                return;
            }
           _snackMachine.BuySnack(position);
            //using (ISession session = SessionFactory.OpenSession())
            //using(ITransaction transaction = session.BeginTransaction())
            //{
            //    session.SaveOrUpdate(_snackMachine);
            //    transaction.Commit();
            //}
            _snackMachineRepository.Save(_snackMachine);
            NotifyClient("You've bought a snack", Money.None);
        }

        private void ReturnMoney()
        {
            var moneyToBeReturned = MoneyInside;
            _snackMachine.ReturnMoney();
            NotifyClient("Money was returned: ", moneyToBeReturned);
        }

        private void InsertMoney(Money coinOrNote)
        {
            _snackMachine.InsertMoney(coinOrNote);
            NotifyClient("You've just inserted: ", coinOrNote);
        }

        private void NotifyClient(string message, Money coinOrNote)
        {
            //in case of we decide to rename the propery the complier will notify us to apply also changes here.
            //Internally, the compiler just replace the nameof invocation with string, so This is no performance drawback in this solution.
            Notify(nameof(MoneyInTransaction));
            Notify(nameof(MoneyInside));
            Notify(nameof(Piles));
            Message = $"{message}  {coinOrNote}";
        }
    }
}
//private readonly SnackMachine _snackMachine;
//private readonly SnackMachineRepository _repository;

//public override string Caption => "Snack Machine";
//public string MoneyInTransaction => _snackMachine.MoneyInTransaction.ToString();
//public Money MoneyInside => _snackMachine.MoneyInside;

//public IReadOnlyList<SnackPileViewModel> Piles
//{
//    get
//    {
//        return _snackMachine.GetAllSnackPiles()
//            .Select(x => new SnackPileViewModel(x))
//            .ToList();
//    }
//}

//private string _message = "";
//public string Message
//{
//    get { return _message; }
//    private set
//    {
//        _message = value;
//        Notify();
//    }
//}

//public Command InsertCentCommand { get; private set; }
//public Command InsertTenCentCommand { get; private set; }
//public Command InsertQuarterCommand { get; private set; }
//public Command InsertDollarCommand { get; private set; }
//public Command InsertFiveDollarCommand { get; private set; }
//public Command InsertTwentyDollarCommand { get; private set; }
//public Command ReturnMoneyCommand { get; private set; }
//public Command<string> BuySnackCommand { get; private set; }

//public SnackMachineViewModel(SnackMachine snackMachine)
//{
//    _snackMachine = snackMachine;
//    _repository = new SnackMachineRepository();

//    InsertCentCommand = new Command(() => InsertMoney(Money.Cent));
//    InsertTenCentCommand = new Command(() => InsertMoney(Money.TenCent));
//    InsertQuarterCommand = new Command(() => InsertMoney(Money.Quarter));
//    InsertDollarCommand = new Command(() => InsertMoney(Money.Dollar));
//    InsertFiveDollarCommand = new Command(() => InsertMoney(Money.FiveDollar));
//    InsertTwentyDollarCommand = new Command(() => InsertMoney(Money.TwentyDollar));
//    ReturnMoneyCommand = new Command(() => ReturnMoney());
//    BuySnackCommand = new Command<string>(BuySnack);
//}

//private void BuySnack(string positionString)
//{
//    int position = int.Parse(positionString);

//    string error = _snackMachine.CanBuySnack(position);
//    if (error != string.Empty)
//    {
//        NotifyClient(error);
//        return;
//    }

//    _snackMachine.BuySnack(position);
//    _repository.Save(_snackMachine);
//    NotifyClient("You have bought a snack");
//}

//private void ReturnMoney()
//{
//    _snackMachine.ReturnMoney();
//    NotifyClient("Money was returned");
//}

//private void InsertMoney(Money coinOrNote)
//{
//    _snackMachine.InsertMoney(coinOrNote);
//    NotifyClient("You have inserted: " + coinOrNote);
//}

//private void NotifyClient(string message)
//{
//    Message = message;
//    Notify(nameof(MoneyInTransaction));
//    Notify(nameof(MoneyInside));
//    Notify(nameof(Piles));
//}