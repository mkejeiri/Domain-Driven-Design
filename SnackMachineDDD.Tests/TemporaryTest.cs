using SnackMachineDDD.logic.SnackMachine;
using SnackMachineDDD.logic.Utils;
using Xunit;
using static SnackMachineDDD.logic.SharedKernel.Money;

namespace SnackMachineDDD.Tests
{

    public class TemporaryTest
    {
        [Fact]
        public void Test()
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["SnackMachineDDDDatabase"].ConnectionString;

            SessionFactory.Init(@"Server=.\SQLEXPRESS;Database=SnackMachineDDD;Trusted_Connection=True;");

            //using (ISession session = SessionFactory.OpenSession())
            //{
            //    long id = 1;
            //    //var snackMachine = session.Get<SnackMachine>(id);
            //    var repository = new SnackMachineRepository();
            //    var snackMachine = repository.GetById(1);
            //    snackMachine.InsertMoney(OneDollar);
            //    snackMachine.InsertMoney(OneDollar);
            //    snackMachine.InsertMoney(OneDollar);
            //    snackMachine.BuySnack(1);
            //    repository.Save(snackMachine);
            //}
           
            //var snackMachine = session.Get<SnackMachine>(id);
            var repository = new SnackMachineRepository();
            var snackMachine = repository.GetById(1);
            snackMachine.InsertMoney(OneDollar);
            snackMachine.InsertMoney(OneDollar);
            snackMachine.InsertMoney(OneDollar);
            snackMachine.BuySnack(1);
            repository.Save(snackMachine);
        }
    }
}
