using System;
using System.Linq;
using FluentAssertions;
using FluentNHibernate.Visitors;
using SnackMachineDDD.logic;
using SnackMachineDDD.logic.SharedKernel;
using SnackMachineDDD.logic.SnackMachine;
using Xunit;
using static SnackMachineDDD.logic.SharedKernel.Money;
using static SnackMachineDDD.logic.SnackMachine.Snack;

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
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 10, 1m));
            snackMachine.InsertMoney(OneDollar);
            snackMachine.BuySnack(position:1);

            //snackMachine.MoneyInTransaction.Should().Be(None);
            snackMachine.MoneyInTransaction.Should().Be(0);
            snackMachine.MoneyInside.Should().Be(OneDollar);
            //snackMachine.Slots.Single(x => x.Position == 1).Quantity.Should().Be(9);
            //snackMachine.GetQuantityOfSnacksInSlots(1).Quantity.Should().Be(9);
            snackMachine.GetSnackPile(1).Quantity.Should().Be(9);
        }

        [Fact]
        public void Cannot_make_purchase_where_there_is_no_snack()
        {
            var snackMachine = new SnackMachine();
            Action actionDelegate = () => snackMachine.BuySnack(1);
            actionDelegate.Should().Throw<InvalidOperationException>();
        }
        [Fact]
        public void Cannot_make_purchase_if_not_enough_money_inserted()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1, 2m));
            Action actionDelegate = () => snackMachine.BuySnack(1);
            actionDelegate.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Return_money_with_highest_denomination_first()
        {
            SnackMachine  snackMachine = new SnackMachine();
            //one dollar in the machine
            snackMachine.LoadMoney(OneDollar);

            //customer inserted a quarter 4 times and get one dollar
            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);

            //return the money to the customer
            snackMachine.ReturnMoney();
            snackMachine.MoneyInside.QuarterCount.Should().Be(4);
            snackMachine.MoneyInside.OneDollarCount.Should().Be(0);
        }

        //TODO: Allocation method test coverage
        [Fact]
        public void After_purchase_change_is_returned()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1,  0.5m));

            //snackMachine.LoadMoney(new Money(0,10,0,0,0,0));
            snackMachine.LoadMoney(TenCent * 10);

            snackMachine.InsertMoney(OneDollar);
            snackMachine.BuySnack(1);
            snackMachine.MoneyInside.Amount.Should().Be(1.5m);
            snackMachine.MoneyInTransaction.Should().Be(0);
        }
        [Fact]
        public void Cannot_buy_snack_if_not_enough_change()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1, 0.5m));
            //the MoneyInside = 0
            snackMachine.InsertMoney(OneDollar);
            Action actionDelegate = () => snackMachine.BuySnack(1);
            actionDelegate.Should().Throw<InvalidOperationException>();
        }
    }
}
