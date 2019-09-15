using System;
using FluentAssertions;
using SnackMachineDDD.logic.SnackMachine;
using Xunit;

namespace SnackMachineDDD.Tests
{
    public class SnackPileSpecs
    {
        // Unit test for Snack pile invariants
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(-1, -1)]
        [InlineData(0, -1)]
        [InlineData(0, 0.01)]
        [InlineData(-1, 0.01)]
        [InlineData(0, 100.21)]
        [InlineData(10, 12.51)]
        public void Cannot_have_quantity_or_price_negative_or_price_with_cent(int quantity, decimal price)
        {
            Action action = () => new SnackPile(Snack.None, quantity, price);
            action.Should().Throw<InvalidOperationException>();
        }

       






    }
}
