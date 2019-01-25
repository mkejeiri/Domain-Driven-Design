﻿using System;
using SnackMachineDDD.logic.Common;

namespace SnackMachineDDD.logic.SharedKernel 
{
    public class Money : ValueObject<Money>
    {
        public static Money None = new Money(0, 0, 0, 0, 0, 0);
        //amout of money inside
        /***********************************************************************************
         * To implement mutability we could hide the properties from the outside world
                e.g : public int OneCentCount { get; private set}
         * BUT c# 6 introduce readonly properties like this :  public int OneCentCount {get;}
         ************************************************************************************/
        public int OneCentCount { get; }
        public int TenCentCount { get; }
        public int QuarterCount { get;}
        public int OneDollarCount { get; }
        public int FiveDollarCount { get; }
        public int TwentyDollarCount { get; }

        public decimal Amount => (OneCentCount * 0.01m +
                        TenCentCount * 0.1m +
                        QuarterCount * 0.25m +
                        OneDollarCount * 1m +
                        FiveDollarCount * 5m +
                        TwentyDollarCount * 20m);
        
        public static Money Cent = new Money(1, 0, 0, 0, 0, 0);
        public static Money TenCent = new Money(0, 1, 0, 0, 0, 0);
        public static Money Quarter = new Money(0, 0, 1, 0, 0, 0);
        public static Money OneDollar = new Money(0, 0, 0, 1, 0, 0);
        public static Money FiveDollar = new Money(0, 0, 0,  0,1, 0);
        public static Money TwentyDollar = new Money(0, 0, 0, 0, 0, 1);

        // I M P O R T A N T!
        //The parameter-less constructor is required
        //because NHibernate creates entities via reflection.
        private Money() {}
        //ctr
        public Money(
            int oneCentCount, 
            int tenCentCount, 
            int quarterCount, 
            int oneDollarCount, 
            int fiveDollarCount,
            int twentyDollarCount) : this()
        {
            if (oneCentCount < 0) throw new InvalidOperationException();
            if (tenCentCount < 0) throw new InvalidOperationException();
            if (quarterCount < 0) throw new InvalidOperationException();
            if (oneDollarCount < 0) throw new InvalidOperationException();
            if (fiveDollarCount < 0) throw new InvalidOperationException();
            if (twentyDollarCount < 0) throw new InvalidOperationException();

            OneCentCount = oneCentCount;
            TenCentCount = tenCentCount;
            QuarterCount = quarterCount;
            OneDollarCount = oneDollarCount;
            FiveDollarCount = fiveDollarCount;
            TwentyDollarCount = twentyDollarCount;
        }

      

        public static Money operator +(Money money, Money moneyOther)
        {
            var sum = new Money(money.OneCentCount + moneyOther.OneCentCount,
                 money.TenCentCount + moneyOther.TenCentCount,
                money.QuarterCount + moneyOther.QuarterCount,
                money.OneDollarCount + moneyOther.OneDollarCount,
                money.FiveDollarCount + moneyOther.FiveDollarCount,
                money.TwentyDollarCount + moneyOther.TwentyDollarCount);
            return sum;
        }

        public static Money operator -(Money money, Money moneyOther)
        {
            var minus = new Money(money.OneCentCount - moneyOther.OneCentCount,
                money.TenCentCount - moneyOther.TenCentCount,
                money.QuarterCount - moneyOther.QuarterCount,
                money.OneDollarCount - moneyOther.OneDollarCount,
                money.FiveDollarCount - moneyOther.FiveDollarCount,
                money.TwentyDollarCount - moneyOther.TwentyDollarCount);
            return minus;
        }

        public static Money operator *(Money money, int multiplier)
        {
            return new Money(money.OneCentCount * multiplier,
                money.TenCentCount * multiplier,
                money.QuarterCount * multiplier,
                money.OneDollarCount * multiplier,
                money.FiveDollarCount * multiplier,
                money.TwentyDollarCount * multiplier
                );
        }

        /********************************************************************************************
         Command-query separation principle (a part from some cases : e.g. removing a file case!)
            - Allocate (Commands are void) : doesn't return value : just side effects
            - CanAllocate (Queries are non-void ): return a boolean value with no side effects
        *********************************************************************************************/

        public bool CanAllocate(decimal amount)
        {
            Money money = AllocateCore(amount);
            //Amount should differ in not enough change
           return (amount == money.Amount);
        }

        public Money Allocate(decimal amount)
        {
            if (!CanAllocate(amount)) throw new InvalidOperationException();
            return AllocateCore(amount);
        }

        protected override bool EqualsCore(Money other)
        {
            return (OneCentCount == other.OneCentCount) &&
                   (TenCentCount == other.TenCentCount) &&
                   (QuarterCount == other.QuarterCount) &&
                   (OneDollarCount == other.OneDollarCount) &&
                   (FiveDollarCount == other.FiveDollarCount) &&
                   (TwentyDollarCount == other.TwentyDollarCount);
        }

        protected override int GetHashCodeCore()
        {
        /*
        The value 397 was chosen because it is an odd prime. If it were even and the multiplication overflowed, 
        information would be lost, as multiplication by 2 is equivalent to shifting. The advantage of using a prime looks like less clear, 
        and become traditional/obvious through Resharper. To further elaborate on that, SHIFTING left just introduces a zero on the right 
        and loses a bit on the left of the number's binary representation, so it's a clear information loss. 
        Repeating this process gradually loses all information that was accumulated from earlier computation. 
        That means that the more fields enter your hashcode calculation, the less effect on the final result the early fields have. 
        The objective of hashCode is basically randomizing it across all unequal instances(Overflows are overlooked here). 
        Multiplication is there to avoid collisions between two fields having commuted values: e.g.
        if you have int x, y, you don't want the same hashCode when you swap x and y. Multiplication by an odd prime avoid just that.        
        */
            unchecked
            {
                int hashCode = OneCentCount;
                hashCode = (hashCode * 397) ^ TenCentCount;
                hashCode = (hashCode * 397) ^ QuarterCount;
                hashCode = (hashCode * 397) ^ OneDollarCount;
                hashCode = (hashCode * 397) ^ FiveDollarCount;
                hashCode = (hashCode * 397) ^ TwentyDollarCount;
                return hashCode;
            }
        }

        public override string ToString() 
            => (Amount < 1) ?  "¢" + (Amount * 100).ToString("0") :  "$" + Amount.ToString("0.00");

        //transform an amount into Money object from the highest bill to  the lower cent!
        private Money AllocateCore(decimal amount)
        {
            int twentyDollarCount = Math.Min((int)(amount / 20), TwentyDollarCount);
            amount -= twentyDollarCount * 20;

            int fiveDollarCount = Math.Min((int)(amount / 5), FiveDollarCount);
            amount -= fiveDollarCount * 5;

            int oneDollarCount = Math.Min((int)(amount), OneDollarCount);
            amount -= oneDollarCount;


            int quarterCount = Math.Min((int)(amount / .25m), QuarterCount);
            amount -= quarterCount * .25m;

            int tenCentCount = Math.Min((int)(amount / .1m), TenCentCount);
            amount -= tenCentCount * .1m;

            int oneCentCount = Math.Min((int)(amount / .01m), OneCentCount);
            amount -= oneCentCount * .01m;
            return new Money(oneCentCount, tenCentCount, quarterCount, oneDollarCount, fiveDollarCount, twentyDollarCount);
        }
    }
}
