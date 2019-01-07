using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;
using SnackMachineDDD.logic.Common;
/*********************************************************************************************************
 To preseve the Unit of work, EventListener implements four interfaces DispatchEvents method that performs 
 the actual dispatching we are listening  here to PostUpdate, PostDelete, and PostInsert, which are used 
 by NHibernate after an entity is updated, deleted, or inserted... process domain events only when the 
 aggregate was successfully persisted into the database 
 ********************************************************************************************************/
namespace SnackMachineDDD.logic.Utils
{
    internal class EventListerner :
        IPostInsertEventListener,
        IPostDeleteEventListener,
        IPostUpdateEventListener,
        IPostCollectionUpdateEventListener
    {

        /*****************************************************************************************************************
          PostUpdateCollection triggered after a collection update, e.g. the number of slots changed in snack machine, 
          PostUpdate wouldn't work because snack machine entity itself wouldn't change, and so NHibernate wouldn't 
          trigger the PostUpdate event for it, it would fire a PostUpdateCollection event instead.
         *****************************************************************************************************************/
        public void OnPostUpdateCollection(PostCollectionUpdateEvent ev) => DispatchEvents(ev.AffectedOwnerIdOrNull as AggregateRoot);

        public void OnPostInsert(PostInsertEvent ev) => DispatchEvents(ev.Entity as AggregateRoot);
        public void OnPostDelete(PostDeleteEvent ev) => DispatchEvents(ev.Entity as AggregateRoot);
        public void OnPostUpdate(PostUpdateEvent ev) => DispatchEvents(ev.Entity as AggregateRoot);

        //allows us to dispatch domain events only after the persistence operation is completed, and thus preserve the unit of work 
        private void DispatchEvents(AggregateRoot aggregateRoot)
        {
            if (aggregateRoot == null)
            {
                return;
            }      
            foreach (IDomainEvent domainEvent in aggregateRoot.DomainEvents)
            {
                DomainEvents.Dispatch(domainEvent);
            }

            aggregateRoot.ClearEvents();
        }
        //TODO: no implementation is required because they aren't registered in Factory class : SOLID??? => Inteface segregation principle!!! 
        public Task OnPostInsertAsync(PostInsertEvent ev, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task OnPostDeleteAsync(PostDeleteEvent ev, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task OnPostUpdateAsync(PostUpdateEvent ev, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent ev, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
