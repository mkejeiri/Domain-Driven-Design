# Domain Driven Design (DDD)
This is merely an introduction to the DDD based on the work of [Vladimir Khorikov](https://enterprisecraftsmanship.com), this includes: 
  - Onion Architecture
  - Rich Domain Model
  - ORM such NHibernate which guarantees a [better degree of Domain Model Isolation than EF](https://enterprisecraftsmanship.com/2018/06/13/ef-core-vs-nhibernate-ddd-perspective/)
  - Domain Driven Design and Events Driven Design
  - WPF
  - Usage of HILO Algorithm when assigning ID's 
  - ...



**_a Few words when implementing DDD_** 
-------------------------------------------
Strategic design:
- Core domain: Focus on the most important part of the system which CANNOT be outsourced! (Competitive Advantage)
- Ubiquitous language: Bridges the gap between developers and experts
- Bounded context : Clear boundaries between different parts of the system, big model brings significant communication and overhead with them as more people get involved. 
    -  Bounded context hepls reduce that using Ubiquitous language, 
	-  Each Bounded context has it's own onion architeture (think microservices)
	-  Explicit relationships between different bounded contexts using the [context Map](https://vimeo.com/125769142)
- Context Map: it render the bounded contexts of the system and the communication between them.
 
Sub-domain belongs to problem space and Bounded context belongs to the solution space, usually the relation is 1 to 1, but in case of legacy system you might add an extra anticorruption layer as bounded context. 
 
How to find boundaries for Bounded Context ?
 - sometimes is the same as how to find boundaries for Sub-domain ? => ask domain experts!
 - sometimes we to look a team size (6-8 dev max) or the code size (should fit in the head)
 - use event storming
 - in some cases bounded context are only logically (and not physically) separeted (same project and same databases with separate schemas), which is not the case in [Microservices](http://bit.ly/1dI7ZJQ) (separate deployments and processes run)!
 
The physical vs logical separation is trade-off between easy to maintain with proper isolation vs bigger maintenance overhead
 


Interface abuse
---------------
 
- No business logic : Implementation could lead to potential duplication 
- Can do : a promise or a contract to do something but doesn't say anything about any eventual relationship or hierarchy of the classes

*_.Net Value type VS Value Object_*

- .Net Value type : Implementation details, Immutable, Structural equality
- Value Object : Design Object, Immutable, Structural equality...



*_Why not use struct (i.e. .Net Value type) instead of value object? :_*

- Struct doesn't support inheritance => equality should be implemented in each struct separately which lead to code duplication.
- Struct doesn't interact very well with ORM's
- Value Objects allow abstraction of multiple fields (e.g SnackPile {snack, quantity, price}, Slot acts as a host for SnackPile), it also has invariants which protect the internal state of the aggregate/entity at all time, there are light-weight and it's also to move business logic to them as much as possible.
- Value Objects are immutable and should remain so (e.g. instead of subtracting quantity we create a new object bearing the same quantity, see SnackPile.SubtractOne())


Unit test should cover at most inner layer (entities, value object, domain events, aggregates) : [read more ...](http://bit.ly/1XF0J6H)
 
 

**_Application Services VS Domain Layer_**
-------------------------------------------

- UI = XAML VIEW = HTLM VIEW
- Application Services = View Model = Controllers

Application Services acts as mediator between the outside world and domain layer (Entities, Value objects, Domain Events, Aggregates and Domain services + repo + Factories), It shouldn't contains any business logic, it only coordinates and delegates inputs from UI and pass it to the domain layer for validation. 
For instance, In WPF View Model (Application Services) acts as wrapper on top of an entity, it increases it with the functionality required by the view, it doesn't contain any business logic and rather delegated it to the entity, it also wraps other domain classes such a repositories... 

Rule of thumb when deciding on where to put a piece of logic:
- If the piece of logic makes sense only in the UI context that it would be better to place it in the Application Service layer
- Otherwise it should be in the domain layer. 

**_Why persist a value object is a bad Idea ?_**
------------------------------------------------
This about Snackmachine (where MoneyId as FK) and Money (with MoneyId as PK)
- It will require an Id, which goes against the definition of value object (structural + reference equality)
- It (Money) will have a lifetime of its own (we could delete Snackmachine object without deleting Money row), it violates the rules that a value object lifetime should fully depend on the entities lifetime.

*_Initialization :_*
 It's good practice to do initialization as close a possible to the startup (factory initialisation, IOC, ...), e.g. in WPF inside the App Class, WebApi Startup class...

**_Aggregates_**
-------------------------
Aggregate (i.e. root entity) is design pattern that help us to simplify the domain model by gathering multiple entities under a single abstraction.
  - It's a conceptual whole, i.e. it represents a cohesive notion of domain model
  - Has a set of invariants which acts as a guard and maintain its state permanently valid during its lifetime... 
  - Every aggregate should have a root entity : classes/entities outside the root could ONLY AND ONLY reference the ROOT ENTITY (and not other entities) of that root. 
  - classes/entities couldn't hold reference to other entities accross aggregates, they must go through the ROOT ENTITY! first, e.g. access to Slot must be done through SnackMachine only, Slot should be hidden from outside world!
  - Restricting access to entities (other than from RootEntities) that are internal to aggregate from outside clients(entities...) helps to better protect the invariants and thus avoiding to corrupt the internal state of the aggregate. 
  - Single operational unit for the application layer: Aggregate acts as single operation unit (unit of work) in the application layer (AL), the AL should retrieve them from the DB, perform some operation on them and store them back as single object. 
  - Consistency boundaries: Aggregate maintain consitency boundaries, the data belonging to an aggregate in the DB should be consistent (i.e. doesn't break the invariants), hence the need to persist an aggregate in transactional way. (e.g. we cannot save snackMachine without the Slots that go with it)  
  - Invariants which span accross multiple aggregates shoudn't be expected to be updated all the time, but they should be consistent all the times
  - Value Object could reside in several aggregate (e.g. integer value! which could be used accross entities of multiple aggregates)
  
  
 *_Find Bounderies for Aggregates_*
 ----------------------------------
 - Entities inside aggregate (i.e. root entity) should be highly cohesive as a group of classes, and entities outside aggregate are loosely coupled
 - To identify an root entity or aggregate, ask the question, Does an entity makes sense by its own? if it does than it's an aggregate (root entity), otherwise it's part of an already existing aggregate or root entity, e.g. a slot cannot exist without a SnackMachine,does an entity within the SnackMachine (aggregate), the Snack class/entity can life by its own so it could be a different root aggregate/entity. 
 - Change boundaries when you discover more information 
 - Avoid creating aggregates that are too large because it hard to maintain consistency if you have to update/store partial part of the aggregate.
 - Proper boundaries are sometime a trade-off between simplicity and performance.
 - There's no limit on how many value objects are in the aggregate.
 - 1-to-many should be 1-to-some, need to extract to different aggregate: e.g. SnackMachine and PurchaseLog, separate the PurchaseLog and use domain event to communicate between SnackMachine and PurchaseLog.  
 - Do NOT expose internal entities outside the aggregate
  

*_AggregateRoot abstract class has three major goals:_*
 --------------------------------------------------------
- More readable code base : which entities are AggregateRoot and which ones are part of an existing Aggregate
- Optimistic locking : if it's introduced we need to version the Aggregate
- Perfect place to hold domain events that happens to an Aggregate during its lifetime ,in our case we will use domain event and optimistic locking so we need otherwise we don't use it.



**_Repository_**
 --------------------------------------------------------
 Design pattern for encapsulating the communication with the database, the client should gather required domain object as if they reside in memory by calling a single method (no additional effort is required).
 
 
 *_How many repositories should we create for domain model_*
 -----------------------------------------------------------
- The general rule it should be a repository per aggregate. e.g. SnackMachineRepository & SnackRepository, they should get eagerly/lazy all entities of the aggregate. All sub-entities should be automatically saved with aggregate (don't forget to SaveUpdate() in FluentNHibernate, for more info check Slot mapping in SnackMachineMap class).

- Repository public method should work only with aggregates/aggregateRoots/Root entities.
- If we have to retrieve a sub-entity we need to get aggregate first and then access to the sub-entity. e.g. SnackMachine snackMachine= repository.GetBySlotId(slotId) here we get SnackMachine and not slot.
- In Web based app we don't work with domain object in de-attach mode, thus the lazy loading makes sense, the reverse is true for desktop apps (domain objects are in de-attach mode).

**_Repository anti-pattern_**
 --------------------------------------------------------
a well-known anti-pattern is to use methods that return not fully initialized entities, i.e. sometime we need a projection of our model that fetches only some required data, in such case we might be lead mistakenly in loading a partially initialized entities from the database which might violates their invariants and hence not adhere to always valid approach, a better approach would be to use DTO's instead.


**_Domain event_**
 --------------------------------------------------------
Domain events that are significant to the domain (as opposed to system events = notion representing the infrastructure), they decouple bounded context and facilitate communication between them. It could also be used within the same bounded context.


Domain Events Guidelines 
 - Naming : use alway the Past tense related the events: e.g. BalanceChangedEvent,
 - Data : Include as little data as possible, don’t use domain classes to represent data in events, use primitive types instead: 
    -  usually domain classes they include more information than needed 
	-  includes additional point of coupling, it's OK to use domain classes and only if the events are consumed by a downstream bounded context (conformist), but in most cases is not the case.
 
*_do we need to include in the event full information or only Id ?_*
--------------------------------------------------------------------
It depends, use :
- Ids : When consuming BC knows about producing BC, so the downstream BC can query the data based on Id's, i.e. we won't introduce any additional coupling... 
- Full information: When consuming BC doesn’t know about producing BC, we must use primitives type in such case otherwise we will introduce coupling.

*_how to deliver physically the events to their subscribers ?_*
---------------------------------------------------------------------
It depends on what isolation type is used for BC :
 - If single process : We use Inner-memory structures
 - Not a single process : it goes through the network through a Service Bus
 
Note : physical delivery is orthogonal to the notion of domain events, we can use whatever techniques, we can even persist events and use them later in an events sourcing architecture.


*_Always Valid vs. Not Always Valid_*
----------------------------------------
Prefer the "Always Valid" approach because :
- Removes temporal coupling, no need to call isValid in one single place at the end by another class
- DRY principle, it may never lead to code duplication, due to human factor (to remember the central place of validation)
- Each class maintain its invariants and remain all time in a valid state
for more about [Fail fast principle](http://bit.ly/1RrHvj8) 

*_Domain Services_*
----------------------------------------
 - Don't have state per their own
 - Contain some domain logic
 - Contains knowledge that doesn't belong neither to entities not to value objects
 
*_Domain Services Vs Application Service_*
----------------------------------------
 - Domain Services : resides inside of the domain layer, Contains domain logic and doesn't communicate with the outside world 
 - Application Service: resides outside of the domain layer, Communicates with the outside world and doesn't contain domain logic, they delegates execution to domain classes such as entities, value objects, repositories and domain services. 
