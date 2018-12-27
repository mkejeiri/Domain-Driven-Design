using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnackMachineDDD.logic.Common
{
    public static class DomainEvents
    {
        /*************************************************************************************************************
         Dynamic Handlers are the handlers added at runtime with the method 'register'. 
         For a particular event type, its delegate is added to the the internal collection of delegates.
         Dynamic Handlers are usually used in unit tests to check that the events are raised correctly. 
         A pre-created dictionary is used; where the key is the type of an event - and the value is the list of handlers added at runtime. 
         *************************************************************************************************************/
        private static Dictionary<Type, List<Delegate>> _dynamicHandlers;

        /*************************************************************************************************************
         Static Handlers are handlers that we define as classes like BalanceChangedEvent handler. 
        *************************************************************************************************************/
        private static List<Type> _staticHandlers;


        //This code scans the current assembly for all such handlers and gathers them into a single list. 
        //It determines whether or not a class is a handler by looking for the IHandler interface we defined previously.
        public static void Init()
        {
            _dynamicHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(IDomainEvent).IsAssignableFrom(x) && !x.IsInterface)
                .ToList()
                .ToDictionary(x => x, x => new List<Delegate>());

            _staticHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandler<>)))
                .ToList();
        }


        //Dynamic Handlers are the handlers added at runtime with the method 'register'. 
        //For a particular event type, its delegate is added to the the internal collection of delegates. 
        public static void Register<T>(Action<T> eventHandler)
            where T : IDomainEvent
        {
            _dynamicHandlers[typeof(T)].Add(eventHandler);
        }

        //Process the actual event : whenever we need to raise an event,
        //we call this static method of the domainEvent class,
        //and it dispatches the event to all dynamic and static handlers registered.
        public static void Raise<T>(T domainEvent)
        where T: IDomainEvent
        {
            //go through the dynamicHandlers with the suited the type of the event passed in.
            foreach (Delegate handler in _dynamicHandlers[domainEvent.GetType()])
            {
                var action = (Action<T>) handler;

                //and invokes each handler in the list.
                action(domainEvent);
            }

            //go through the static Handlers that implement the handler interface.
            foreach (Type handler in _staticHandlers)
            {
                if (typeof(IHandler<T>).IsAssignableFrom(handler))
                {
                    //instantiates a handler object and calls the Handle method on it
                    IHandler<T> instance = (IHandler<T>) Activator.CreateInstance(handler);
                    instance.Handle(domainEvent);
                }
            }
        }
    }
}
