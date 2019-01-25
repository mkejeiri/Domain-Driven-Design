
using System;
using SnackMachineDDD.logic.Common;

namespace SnackMachineDDD.logic.SnackMachine
{ 
    /***********************************************************************************************************************************************
     - All responsibilities of the slot entity had been transfered to the SnackPile value object (light weight). 
       The Slot acts just as a host for that value object, nothing more. 
     - To adhere to the immutability rule of value objects. Instead of making the quantity property in the SnackPile ValueObject mutable, 
       a separate method that creates a new instance of that value object had been introduce.     
     ************************************************************************************************************************************************/

    public sealed class SnackPile:ValueObject<SnackPile>
    {
        public static readonly SnackPile EmptySnackPile = new SnackPile(Snack.None, 0,0m);
        private SnackPile(){}
        public SnackPile(Snack snack, int quantity, decimal price):this()
        {
            //TODO:Unit test for the Invariants
            //Invariants
            if (quantity < 0 ) throw new InvalidOperationException();
            if (price < 0) throw new InvalidOperationException();
            //don't allow prices that include less than one cent : machine cannot handle such physical money
            if (price % 0.01m > 0) throw new InvalidOperationException();

            Snack = snack;
            Quantity = quantity;
            Price = price;
        }

        public  Snack Snack { get;  }
        public  int Quantity { get;  }
        public  decimal Price { get; }

        protected override bool EqualsCore(SnackPile other)
        {
            return other.Snack == Snack && other.Quantity == Quantity && other.Price == Price;
        }

        protected override int GetHashCodeCore()
        {
        /******************************************************************************************************************************
        The value 397 was chosen because it is an odd prime. If it were even and the multiplication overflowed, 
        information would be lost, as multiplication by 2 is equivalent to shifting. The advantage of using a prime looks like less clear, 
        and become traditional/obvious through Resharper. To further elaborate on that, SHIFTING left just introduces a zero on the right 
        and loses a bit on the left of the number's binary representation, so it's a clear information loss. 
        Repeating this process gradually loses all information that was accumulated from earlier computation. 
        That means that the more fields enter your hashcode calculation, the less effect on the final result the early fields have. 
        The objective of hashCode is basically randomizing it across all unequal instances(Overflows are overlooked here). 
        Multiplication is there to avoid collisions between two fields having commuted values: e.g.
        if you have int x, y, you don't want the same hashCode when you swap x and y. Multiplication by an odd prime avoid just that.        
        *******************************************************************************************************************************/
        
            unchecked
            {
                int hashCode = Snack.GetHashCode();
                hashCode = (hashCode * 397) ^ Quantity;
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                return hashCode;
            }
        }

        public SnackPile SubtractOne()
        { 
            return  new SnackPile(Snack, Quantity-1, Price);
        }
    }
}
