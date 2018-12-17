using System;
using FluentAssertions;
using FluentAssertions.Common;
using SnackMachineDDD.logic;
using Xunit;

namespace SnackMachineDDD.Tests
{
    public class MoneySpecs
    {
        [Fact]
        public void Sum_of_two_moneys_produces_correct_result()
        {
            /***************************************************             
             *                      Arrange 
             ***************************************************/
            Money money1 = new Money(1, 2, 3, 4, 5, 6);
            Money money2 = new Money(1, 2, 3, 4, 5, 6);

            /***************************************************             
             *                      Act 
             ***************************************************/
            Money sum = money1 + money2;
            
            /***************************************************             
             *                      Assert 
             ***************************************************/
            //Assert.Equal(2, sum.OneCentCount);
            //Fluent assertions mimic english language : More readable!
            sum.OneCentCount.Should().Be(2);
            sum.TenCentCount.Should().Be(4);
            sum.QuarterCount.Should().Be(6);
            sum.OneDollarCount.Should().Be(8);
            sum.FiveDollarCount.Should().Be(10);
            sum.TwentyDollarCount.Should().Be(12);
        }

        [Fact]
        public void Two_money_instances_do_not_equal_if_contain_different_money_amounts()
        {
            Money dollar = new Money(0,0,0,1,0,0);
            Money hundredCents = new Money(100, 0,0,0,0,0);
            dollar.Should().NotBe(hundredCents);

            Money tenTimesTenCents = new Money(0, 10, 0, 0, 0, 0);
            dollar.Should().NotBe(tenTimesTenCents);
        }

        [Theory]
        [InlineData(-1, 0, 0, 0, 0, 0)]
        [InlineData(0, -2, 0, 0, 0, 0)]
        [InlineData(0, 0, -3, 0, 0, 0)]
        [InlineData(0, 0, 0, -4, 0, 0)]
        [InlineData(0, 0, 0, 0, -5, 0)]
        [InlineData(0, 0, 0, 0, 0, -6)]
        public void Cannot_create_money_with_negative_value(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount)
        {
        

            Action action =() => new Money(
                oneCentCount, 
                tenCentCount, 
                quarterCount, 
                oneDollarCount, 
                fiveDollarCount, 
                twentyDollarCount);
            action.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0, 0, 0, 0.01)]
        [InlineData(1, 2, 0, 0, 0, 0, 0.21)]
        [InlineData(1, 2, 3, 0, 0, 0, 0.96)]
        [InlineData(1, 2, 3, 4, 0, 0, 4.96)]
        [InlineData(1, 2, 3, 4,5, 0, 29.96)]
        [InlineData(11, 0, 0, 0, 0, 0, 0.11)]
        [InlineData(110, 0, 0, 0, 100, 0, 501.1)]
        public void Amount_is_calculated_correctly(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount, 
            decimal expectedAmount)
        {
            Money money = new Money(oneCentCount,
                tenCentCount,
                quarterCount,
                oneDollarCount,
                fiveDollarCount,
                twentyDollarCount);
            money.Amount.Should().Be(expectedAmount);
        }

        [Fact]
        public void Substraction_of_two_money_produces_correct_result()
        {
            /***************************************************             
             *                      Arrange 
             ***************************************************/
            Money money1 = new Money(1, 2, 3, 4, 5, 6);
            Money money2 = new Money(1, 2, 3, 4, 5, 6);

            /***************************************************             
             *                      Act 
             ***************************************************/
            Money subMoney= money1 - money2;

            /***************************************************             
             *                      Assert 
             ***************************************************/
            //Fluent assertions mimic english language : More readable!
            subMoney.OneCentCount.Should().Be(0);
            subMoney.TenCentCount.Should().Be(0);
            subMoney.QuarterCount.Should().Be(0);
            subMoney.OneDollarCount.Should().Be(0);
            subMoney.FiveDollarCount.Should().Be(0);
            subMoney.TwentyDollarCount.Should().Be(0);
        }

        [Fact]
        public void Cannot_substract_more_than_exists()
        {
            Money money1 = new Money(0, 1, 0, 0,  0, 0);
            Money money2 = new Money(1, 0, 0, 0, 0, 0);

            Action actionDelegate = () =>
            {
                Money sub = money2 - money1;
            };
            actionDelegate.Should().Throw<InvalidOperationException>();
        }


        [Theory]
        [InlineData(1, 0, 0, 0, 0, 0, "¢1")]
        [InlineData(0, 0, 0, 1, 0, 0, "$1.00")]
        [InlineData(1, 0, 0, 1, 0, 0, "$1.01")]
        [InlineData(0, 0, 2, 1, 0, 0, "$1.50")]
        public void To_string_should_return_an_amount_of_money(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount,
            string expectedString)
        {
            Money money = new Money(oneCentCount,
                tenCentCount,
                quarterCount,
                oneDollarCount,
                fiveDollarCount,
                twentyDollarCount);
            money.ToString().Should().Be(expectedString);
        }
    }
}

