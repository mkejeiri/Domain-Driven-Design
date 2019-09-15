using System.Collections.Generic;
/****************************************************************************************************************
 AGGREGATE is design pattern that help us to simplify the domain model by gathering multiple entities under a single abstraction
  - It represents a cohesive notion of domain model
  - Has a set of invariants which acts as a guard and maintain its state permanently valid during its lifetime... 
  - Every aggregate should have a root entity : class/entity outside the root could ONLY AND ONLY reference the ROOT ENTITY (and not other entity) of that root. 
  - class/entity couldn't hold reference to other entities inside a different aggregate, they must go through the ROOT ENTITY!,    
    e.g. access to Slot must be done through SnackMachine only, Slot should be hidden from outside world!
    --------------------------------------------------------------------------------------------------------------------------------------------------------
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

        //protected : because the responsibility to create an event should belong to the entity itself not commissioned to external code.
        //process domain events only when the aggregate was successfully persisted into the database 
        protected virtual void AddDomainEvents(IDomainEvent newEvent) => _domainEvents.Add(newEvent);
        public virtual void ClearEvents() => _domainEvents.Clear();

    }
}
