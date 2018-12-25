
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace SnackMachineDDD.logic
{
    public class SnackMachineMap : ClassMap<SnackMachine>
    {
        public SnackMachineMap()
        {
            //ID field is mapped using the Id method which specifies the Identifier property for the entity
            Id(x => x.Id);
            // The Component allows us to inline the value objects fields into the parent entities table
            Component(x => x.MoneyInside, y =>
            {
                y.Map(x => x.OneCentCount);
                y.Map(x => x.TenCentCount);
                y.Map(x => x.QuarterCount);
                y.Map(x => x.OneDollarCount);
                y.Map(x => x.FiveDollarCount);
                y.Map(x => x.TwentyDollarCount);
            });
            HasMany<Slot>(Reveal.Member<SnackMachine>("Slots")).Cascade.SaveUpdate().Not.LazyLoad();
        }
    }
}
