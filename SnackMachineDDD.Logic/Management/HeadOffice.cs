using SnackMachineDDD.logic.Atms;
using SnackMachineDDD.logic.Common;
using SnackMachineDDD.logic.SharedKernel;

namespace SnackMachineDDD.logic.Management
{
    public class HeadOffice : AggregateRoot
    {
        //keep track of all payment made by bank card
        public virtual decimal Balance { get; set; }

        //contain cash transferred from the snack machine
        public virtual Money Cash { get; set; } = Money.None;

        public virtual void ChangeBalance(decimal delta)
        {
            Balance += delta;
        }

        public virtual void UnloadCashFromSnackMachine(SnackMachine.SnackMachine snackMachine)
        {
            Money money = snackMachine.UnloadMoney();
            Cash += money;
        }

        public virtual void LoadCashToAtm(Atm atm)
        {
            atm.LoadMoney(Cash);
            Cash = Money.None;
        }
    }
}
