using SnackMachineDDD.logic.Common;

namespace SnackMachineDDD.logic.SnackMachine
{
    public class Snack : AggregateRoot
    {
        //TODO: so far Integration test to make sure that at least this field doesn't exist in the DB.
        public virtual string  Name { get; set; }
        //TODO: Integration test to make sure that the data here are the same as in the DB.
        public static readonly Snack None = new Snack(0,"None");
        public static readonly Snack Chocolate = new Snack(1, "Chocolate");
        public static readonly Snack Soda = new Snack(2, "Soda");
        public static readonly Snack Gum = new Snack(3,"Gum");

        //for Nhibernate to work
        protected Snack(){}

        private Snack(long id ,string name):this ()
        {
            Name = name /*?? throw new ArgumentNullException(nameof(name))*/;
            Id = id;
        }
        
    }
}
