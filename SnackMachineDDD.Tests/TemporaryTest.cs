using NHibernate;
using System.Configuration;
using SnackMachineDDD.logic;
using Xunit;
using SnackMachineDDD.logic;

namespace SnackMachineDDD.Tests
{

    public class TemporaryTest
    {
        [Fact]
        public void Test()
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["SnackMachineDDDDatabase"].ConnectionString;

            SessionFactory.Init(@"Server=.\SQLEXPRESS;Database=SnackMachineDDD;Trusted_Connection=True;");

            using (ISession session = SessionFactory.OpenSession())
            {
                long id = 1;
                var snackMachine = session.Get<SnackMachine>(id);
            }
        }
    }
}
