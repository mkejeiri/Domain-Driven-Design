using FluentAssertions;
using SnackMachineDDD.logic.Atms;
using SnackMachineDDD.logic.Common;
using SnackMachineDDD.logic.Utils;
using static SnackMachineDDD.logic.SharedKernel.Money;
using Xunit;

namespace SnackMachineDDD.Tests
{
    public class AtmSpecs
    {
        /********************************************************************
         * Verify the money decrease along with the commission
         *********************************************************************/
        [Fact]
        public void Take_money_exchanges_with_commission()
        {
            var atm = new Atm();
            atm.LoadMoney(OneDollar);
            atm.TakeMoney(1m);
            atm.MoneyInside.Amount.Should().Be(0m);
            atm.MoneyCharged.Should().Be(1.01m);
        }

        [Fact]
        public void Commission_should_be_at_least_one_cent()
        {
            var atm = new  Atm();
            atm.LoadMoney(Cent);

            atm.TakeMoney(0.01m);
            atm.MoneyInside.Amount.Should().Be(0m);
            atm.MoneyCharged.Should().Be(0.02m);
        }

        [Fact]
        public void Commission_is_rounded_up_to_the_next_cent()
        {
            var atm = new Atm();
            atm.LoadMoney(OneDollar+ TenCent);
            atm.TakeMoney(1.1m);
            atm.MoneyInside.Amount.Should().Be(0m);
            atm.MoneyCharged.Should().Be(1.12m);
        }

        [Fact]
        public void Take_money_raise_an_event()
        {
            SessionFactory.Init(@"Server=.\SQLEXPRESS;Database=SnackMachineDDD;Trusted_Connection=True;"); ;
            Atm atm = new Atm();
            atm.LoadMoney(OneDollar);
            atm.TakeMoney(1m);

            BalanceChangedEvent balanceChangedEvent = null;
            DomainEvents.Register<BalanceChangedEvent>(ev => balanceChangedEvent = ev);

            //Check event
            balanceChangedEvent.Should().NotBeNull();
            balanceChangedEvent.Delta.Should().Be(1.01m);
        }
    }
}
