using FluentNHibernate.Mapping;

namespace SnackMachineDDD.logic
{
    public class SlotMap:ClassMap<Slot>
    {
        public SlotMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.Position);

            Component(x => x.SnackPile, y =>
            {
                y.Map(x => x.Quantity);
                y.Map(x => x.Price);
                /***************************************************************************************************
                     lazy loading is disable for the Snack entity (referenced by value object SnackPile). 
                     The domain objects work in the detach mode, here all required elements are fetched at once. 
                     So no lazy loading when an object is detached from its session - Desktop app!!!. 
                     However, In the web-based apps,probably we should keep the lazy loading behavior,
                     usually we don't have domain objects detached and session is scoped (request lifecycle).
                 ****************************************************************************************************/
                y.References(x => x.Snack).Not.LazyLoad();//we disable lazy loading
            });

            References(x => x.SnackMachine);
        }
    }
}
