
using System;

namespace SnackMachineDDD.logic
{
    public sealed class SnackPile:ValueObject<SnackPile>
    {
        private SnackPile(){}
        public SnackPile(Snack snack, int quantity, decimal price):this()
        {
            //TODO:Unit test for invanriants
            //Invariants
            if (Quantity<0 || Price< 0) throw  new InvalidOperationException();
            if (price%0.01m>0) throw new InvalidOperationException();

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
