using System;
using System.Linq;
using FluentAssertions;
using FluentNHibernate.Visitors;
using SnackMachineDDD.logic;
using Xunit;
using static SnackMachineDDD.logic.Money;

namespace SnackMachineDDD.Tests
{
    public class SnackMachineSpecs
    {
        [Fact]
        public void Cannot_insert_more_than_one_coin_or_note_at_a_time()
        {
            var snackMachine = new SnackMachine();
            Money twoCents = Cent + Cent;
            Action action = () => snackMachine.InsertMoney(twoCents);
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void BuySnack_trades_inserted_money_for_a_snack()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(new Snack("Some snack"), 10, 1m));
            snackMachine.InsertMoney(OneDollar);
            snackMachine.BuySnack(position:1);

            snackMachine.MoneyInTransaction.Should().Be(None);
            snackMachine.MoneyInside.Should().Be(OneDollar);
            //snackMachine.Slots.Single(x => x.Position == 1).Quantity.Should().Be(9);
            //snackMachine.GetQuantityOfSnacksInSlots(1).Quantity.Should().Be(9);
            snackMachine.GetSnackPile(1).Quantity.Should().Be(9);
        }



    }
}
