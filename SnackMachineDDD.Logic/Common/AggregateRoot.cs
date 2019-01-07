using System.Collections.Generic;
/****************************************************************************************************************
 The collection of domain events are defined in the AggregateRoot base class for the same reason as 
 AggregateRoot's being responsible for maintaining invariants and consistency boundaries, 
 it's also responsible for all domain events occurred in the aggregate.
 *****************************************************************************************************************/
namespace SnackMachineDDD.logic.Common
{
    public abstract class AggregateRoot : Entity
    {
        public virtual int Version { get; set; }
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        //protected : beacause the responsibility to create an event should belong to the entity itself not commissioned to external code.
        //process domain events only when the aggregate was successfully persisted into the database 
        protected virtual void AddDomainEvents(IDomainEvent newEvent) => _domainEvents.Add(newEvent);
        public virtual void ClearEvents() => _domainEvents.Clear();

    }
}
