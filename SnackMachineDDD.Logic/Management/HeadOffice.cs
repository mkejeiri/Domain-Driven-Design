using SnackMachineDDD.logic.Common;
using SnackMachineDDD.logic.SharedKernel;

namespace SnackMachineDDD.logic.Management
{
    public class HeadOffice : AggregateRoot
    {
        //keep track of all payment made by bank card
        public virtual decimal Balance { get; set; }

        //contain cash transfered from snack machine
        public virtual Money Cash { get; set; } = Money.None;

        public virtual void ChangeBalance(decimal delta)
        {
            Balance += delta;
        }
    }
}
