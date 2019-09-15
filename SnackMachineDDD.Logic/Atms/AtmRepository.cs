using System.Collections.Generic;
using System.Linq;
using NHibernate;
using SnackMachineDDD.logic.Common;
using SnackMachineDDD.logic.Utils;

namespace SnackMachineDDD.logic.Atms
{
    //All required functionality resides in the base class
    public class AtmRepository : Repository<Atm>
    {
        public IReadOnlyList<AtmDto> GetAtmList()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Query<Atm>()
                    .ToList() // Fetch data into memory
                    .Select(x => new AtmDto(x.Id, x.MoneyInside.Amount))
                    .ToList();
            }
        }
    }
}
