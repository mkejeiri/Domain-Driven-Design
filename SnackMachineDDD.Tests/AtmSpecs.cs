using System.Linq;
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
            SessionFactory.Init(@"Server=.;Database=SnackMachineDDD;Trusted_Connection=True;");
            DomainEvents.Init();
            Atm atm = new Atm();
            atm.LoadMoney(OneDollar);
            //BalanceChangedEvent balanceChangedEvent = null;
            //DomainEvents.Register<BalanceChangedEvent>(ev => balanceChangedEvent = ev);

            atm.TakeMoney(1m);
            
            //Check event
            var balanceChangedEvent = atm.DomainEvents[0] as BalanceChangedEvent; 
            //balanceChangedEvent.Should().NotBeNull();
            //balanceChangedEvent.Delta.Should().Be(1.01m);
            atm.ShouldContainsBalanceChangedEvent(1.01m);
        }
    }

    //extension method to increase the readability  
    internal static class AtmExtensions
    {
        public static void ShouldContainsBalanceChangedEvent(this Atm atm, decimal delta)
        {
            BalanceChangedEvent domainEvent = (BalanceChangedEvent) atm.DomainEvents
                .SingleOrDefault(x => x.GetType() == typeof(BalanceChangedEvent));
            domainEvent.Should().NotBeNull();
            domainEvent.Delta.Should().Be(1.01m);
        }
    }

}
