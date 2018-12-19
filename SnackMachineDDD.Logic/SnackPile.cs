
using System;

namespace SnackMachineDDD.logic
{ 
    /***********************************************************************************************************************************************
     - All responsibilities of the slot entity had been transfered to the SnackPile value object (light weight). 
       The Slot acts just as a host for that value object, nothing more. 
     - To adhere to the immutability rule of value objects. Instead of making the quantity property in the SnackPile ValueObject mutable, 
       a separate method that creates a new instance of that value object had been introduce.     
     ************************************************************************************************************************************************/

    public sealed class SnackPile:ValueObject<SnackPile>
    {
        private SnackPile(){}
        public SnackPile(Snack snack, int quantity, decimal price):this()
        {
            //TODO:Unit test for the invanriants
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
