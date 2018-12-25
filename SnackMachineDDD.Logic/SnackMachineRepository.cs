using System;
using System.Collections.Generic;

namespace SnackMachineDDD.logic
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
