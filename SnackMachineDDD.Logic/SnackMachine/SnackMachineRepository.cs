using System;
using System.Collections.Generic;
using SnackMachineDDD.logic.Common;
using SnackMachineDDD.logic.SharedKernel;

namespace SnackMachineDDD.logic.SnackMachine
{
    public class SnackMachineRepository: Repository<SnackMachine>
    {
        public IReadOnlyList<SnackMachine> GetAllWithSnack(Snack snack)
        {
            throw  new NotImplementedException();
        }

        public IReadOnlyList<SnackMachine> GetAllWithMoneyInside(Money money)
        {
            throw new NotImplementedException();
        }
    }
}
