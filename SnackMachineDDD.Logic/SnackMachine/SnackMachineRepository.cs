using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using SnackMachineDDD.logic.Common;
using SnackMachineDDD.logic.SharedKernel;
using SnackMachineDDD.logic.Utils;

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

        public IReadOnlyList<SnackMachineDto> GetSnackMachineList()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Query<SnackMachine>()
                    .ToList() // Fetch data into memory
                    .Select(x => new SnackMachineDto(x.Id, x.MoneyInside.Amount))
                    .ToList();
            }
        }
    }
}
