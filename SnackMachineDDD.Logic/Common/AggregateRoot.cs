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
    /*
     * AggregateRoot has three major goals:
     * - More readable code base : which entities are AggregateRoot and which ones are part of an existing Aggregate
     * - Optimistic locking : if it's introduced we need to version the Aggregate
     * - Perfect place to hold domain events that happens to an Aggregate during its lifetime
     in our case we will use domain event and optimistic locking so we need otherwise we don't use it.
     */
    public abstract class AggregateRoot : Entity
    {
        //Optimistic locking : if it's introduced we need to version the AggregateRoot
        public virtual int Version { get; set; } 
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        //protected : because the responsibility to create an event should belong to the entity itself not commissioned to external code.
        //process domain events only when the aggregate was successfully persisted into the database 
        protected virtual void AddDomainEvents(IDomainEvent newEvent) => _domainEvents.Add(newEvent);
        public virtual void ClearEvents() => _domainEvents.Clear();

    }
}
