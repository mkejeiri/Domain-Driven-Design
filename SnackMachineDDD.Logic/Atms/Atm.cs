using System;
using SnackMachineDDD.logic.Common;
using SnackMachineDDD.logic.SharedKernel;

namespace SnackMachineDDD.logic.Atms
{
    public class Atm : AggregateRoot
    {
        private const decimal CommissionRate = 0.01m;
        public virtual Money MoneyInside { get; protected set; } = Money.None;
        public virtual decimal MoneyCharged { get; protected set; }

        public virtual string CanTakeMoney(decimal amount)
        {
            if (amount <= 0m)
            {
                return "Invalid amount";
            }

            if (MoneyInside.Amount < amount)
            {
                return "Not enough money";
            }

            if (!MoneyInside.CanAllocate(amount))
            {
                return "Not enough change";
            }
            return string.Empty;
        }

        public virtual void TakeMoney(decimal amount)
        {
            //TODO: Unit test for that case
            if (CanTakeMoney(amount) != string.Empty) throw new InvalidOperationException();

            Money output = MoneyInside.Allocate(amount);
            MoneyInside -= output;
            decimal amountWithCommission = CalculateAmountWithCommission(amount);
            MoneyCharged += amountWithCommission;
            //DomainEvents.Raise(new BalanceChangedEvent(amountWithCommission));


            //domain entity is no longer responsible for raising the event, it just saves it to the internal list.
            // creating an event is the responsability of domain entities and dispatching it is the responsability of infrastracture. 
            AddDomainEvents(new BalanceChangedEvent(amountWithCommission));
            //if (amount < 1m)
            //{
            //    //decimal amountWithCommission = amount + CommissionRate;
            //    decimal amountWithCommission = CalculateAmountWithCommission(amount);
            //    MoneyCharged += amountWithCommission;
            //}
            //else
            //{
            //    //decimal amountWithCommission = amount + amount * CommissionRate;
            //    decimal amountWithCommission = CalculateAmountWithCommission(amount);
            //    MoneyCharged += amountWithCommission;
            //}
        }

        public virtual decimal CalculateAmountWithCommission(decimal amount)
        {
            decimal commission = amount * CommissionRate;
            decimal lessThanCentReminder = commission % 0.01m;
            if (lessThanCentReminder > 0)
            {
                commission += 0.01m - lessThanCentReminder;
                return amount + commission;
            }

            return amount + commission;
        }

        public virtual void LoadMoney(Money oneDollar)
        {
            MoneyInside += oneDollar;
        }

    }
}
