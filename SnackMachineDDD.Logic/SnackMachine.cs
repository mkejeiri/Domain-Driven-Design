using System;
using System.Linq;
using static SnackMachineDDD.logic.Money;
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
  We should isolate our domain model from the persistence logic as much as possible, 
  it's not always feasible when using an ORM, we still need to adapt the domain layer and some persistence logic leaks 
  into the domain model. Luckily, the degree of the leak is not too big (unsealed class + virtual + Getype pb with lazy loading), 
  and the made tradeoff is worth it. we still preserve a lot of isolation for our domain model. For example, 
  we didn't have to change any of the existing tests that validate our domain classes. 
  So it is possible to maintain the same high degree of isolation even in larger projects.
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SnackMachineDDD.logic
{
    //Nhibernate require all non-private members to be marked as virtual!
    public class SnackMachine : Entity
    {
        //amount of money inside the machine
        public virtual Money MoneyInside { get; protected set; } = None;
        //amount of money in transaction
        public virtual Money MoneyInTransaction { get; protected set; } = None;
        public virtual decimal Amount { get; set; }

        //public virtual void InsertMoney(Money money) => MoneyInTransaction += money;
        public virtual void ReturnMoney() => MoneyInTransaction = None;

        public virtual void BuySnack()
        {
            MoneyInside += MoneyInTransaction;
            MoneyInTransaction = None;
        }

       
        public virtual void InsertMoney(Money money)
        {
            Money[] coinsAndNotes = {
                Cent,
                TenCent,
                Quarter,
                FiveDollar,
                OneDollar,
                TwentyDollar
            };

            if (!coinsAndNotes.Contains( money)) throw  new InvalidOperationException();
            MoneyInTransaction += money;
        }

        public virtual void BuySnack(int position)
        {
            throw new NotImplementedException();
        }


        public virtual string CanBuySnack(int position)
        {
            throw new NotImplementedException();
        }

        public virtual void GetAllSnackPiles()
        {
            throw new NotImplementedException();
        }
    }

}
