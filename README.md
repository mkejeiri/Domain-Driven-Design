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
- Ubiquitous language: Bridges the gap between developers and experts
- Bounded context: Clear boundaries between different parts of the system
- Core domain: Focus on the most important part of the system which it CANNOT be outsourced! (Competitive Advantage)


Interface Abuse:
-----------------
 
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
For instance, In WPF View Model (Application Services) acts as wrapper on top of an entity it, augment it with the functionality required by the view, it doesn't contain any business logic and rather delegated it to the entity, it also wrap other domain classes as well such a repositories... 

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
 
Aggregate (i.e. root entity) is design pattern that help us to simplify the domain model by gathering multiple entities under a single abstraction.
  - It's a conceptual whole, i.e. it represents a cohesive notion of domain model
  - Has a set of invariants which acts as a guard and maintain its state permanently valid during its lifetime... 
  - Every aggregate should have a root entity : classes/entities outside the root could ONLY AND ONLY reference the ROOT ENTITY (and not other entities) of that root. 
  - classes/entities couldn't hold reference to other entities accross aggregates, they must go through the ROOT ENTITY! first, e.g. access to Slot must be done through SnackMachine only, Slot should be hidden from outside world!
  - Restcting access to entities (other than from RootEntities) that are internal to aggregate from outside clients(entities...) helps to better protect the invariants and thus avoiding to corrupt the internal state of the aggregate. 
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
 - Proper boundaries are sometime a trade-off between simplify and performance.
 - There's no limit on how many value objects are in the aggregate.
 - 1-to-many should be 1-to-some, need to extract to different aggregate: e.g. SnackMachine and PurchaseLog, separate the PurchaseLog and use domain event to communicate between SnackMachine and PurchaseLog.  
 - Do NOT expose internal entities outside the aggregate
  

*_AggregateRoot has three major goals:_*
 --------------------------------------------
- More readable code base : which entities are AggregateRoot and which ones are part of an existing Aggregate
- Optimistic locking : if it's introduced we need to version the Aggregate
- Perfect place to hold domain events that happens to an Aggregate during its lifetime ,in our case we will use domain event and optimistic locking so we need otherwise we don't use it.


