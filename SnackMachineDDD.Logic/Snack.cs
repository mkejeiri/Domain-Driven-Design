using System;

namespace SnackMachineDDD.logic
{
    public class Snack : AggregateRoot
    {
        public virtual string  Name { get; set; }
        //for Nhibernate to work
        protected Snack(){}

        public Snack(string name):this ()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }


    }
}
