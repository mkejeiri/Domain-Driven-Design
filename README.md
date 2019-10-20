# Domain Driven Design (DDD)
This is merely an introduction to the DDD based on the work of [Vladimir Khorikov](https://enterprisecraftsmanship.com), this includes: 
  - Onion Architecture
  - Rich Domain Model
  - ORM such NHibernate which guarantees a [better degree of Domain Model Isolation than EF](https://enterprisecraftsmanship.com/2018/06/13/ef-core-vs-nhibernate-ddd-perspective/)
  - Domain Driven Design and Events Driven Design
  - WPF
  - Usage of HILO Algorithm when assigning ID's 
  - ...
  
  
### Domain Driven Design Goal
According to [Alberto Brandolini](https://www.eventstorming.com/) eBook (Event storming), When modeling large scale system, we shouldn’t aim for a large “enterprise model”. That’s an attractor for ambiguities and contradictions. Instead we should aim for 2 things:
- multiple, relatively small models with a high degree of semantic consistency,
- a way to make those interdependent models work together.
A small, strongly semantically consistent model is called Bounded Context in Domain-Driven Design and this is what we should be aiming for.

# [Domain Driven Design Distilled](https://www.amazon.com/Domain-Driven-Design-Distilled-Vaughn-Vernon/dp/0134434420)
## Strategic Design
It highlights what is strategically important to business, how to divide up the work by importance, and how to best integrate as needed using Ubiquitous Language, Bounded Contexts, Subdomains within a Bounded Context, Context Mapping...at this stage we need to identify which Bounded Contexts will be the Core Domain.

## Tactical Design
It enables us to tune the fine details of the domain model, important tools are used such aggregating entities and value objects
together into a right-sized cluster, modeling your domain in the most explicit way possible...

**Types of Subdomains** 
- Core Domain: where we make a strategic investment in a single, well-defined domain model
- Supporting Subdomain: modeling a situation that calls for custom development, because an off-the-shelf solution doesn't exist, it is The software model that the Core Domain cannot be successful without it.
- Generic Subdomain: may be available for purchase or be outsourced or even developed in house by less elite developers 

##  Tactical Design : Aggregate
**Aggregate's 4 rules of thumb**

1- Protect business invariants inside Aggregate boundaries.

2- Design small Aggregates with the right-sizing.

3- Reference other Aggregates by identity only.

4- Update other Aggregates using eventual consistency.


**How to find the right-Sizing of aggregates (merging and tuning)**

1- Design small Aggregates: Start by creating every Aggregate with just one Entity , which will serve as the Aggregate Root and Populate each of the Entities with the fields/attributes/properties that are highly cohesive (the ones required to identify and find the Aggregate + additional intrinsic ones that leaves an aggregate in a valid initial state), all intrinsic fields/attributes must be up-to-date when the single-Entity Aggregate is persisted.

2- Use the previous 1st rule thumb (see aggregate design) : ask the Domain Experts if any other Aggregates you have defined must be updated in reaction to changes made to Aggregate, use consistency rules, which will indicate the time frames for all reaction-based updates => Aggregate types would be listed under a single Aggregate Root (AR) if they will be updated in reaction to AR updates.

3- Domain Experts would tell how much time may elapse until each of the reaction-based updates may take place (poke them by presenting an exaggerated time frame) :
- Immediately 
- Within N seconds/minutes/hours/days

4- Immediate time frames, strongly consider composing those two Entities within the same Aggregate boundary.

5- Reacting Aggregates that can be updated following a given elapsed time, we will update these using eventual consistency (4th rule).

## Tactical Design : Domain Events
Domain Events may be caused by commands, while others may happen due to the detection of some other changing state, such as a date or time

1- Use Event storming to find Events and aggregates : Storm out the business process by creating a series of Domain Events on sticky notes
-   Creating Domain Events, Write the name of each Domain Event, Place the sticky notes on your modeling surface in time order
-   Domain Event that happens in parallel with another according to your business process can be located under the Domain Event that happens at the same time
-   Mark new business process that need further investigation with a purple/red sticky note
-   When Domain Event is a Process that needs to run in single step or multiple complex steps. Domain Events that causes a Process to run named on a lilac sticky note,draw a line with an arrowhead from the Domain Event to the named Process (lilac sticky note). Model a fine-grained Domain Event only if it is important to your Core Domain. Take a break if needed and come back next day 

2 - Create Commands that cause each Domain Event
-   Use light blue sticky note for commands and place it to the left of the Domain Event
-   Use  lilac sticky note for command that will cause a single step or multiple complex steps Process to be run. 
-   Commands will cause you to think about Domain Events that you didn’t previously envision, address the discovery by placing the newly discovered Domain Event along with its corresponding Command.
-   It could be that a Command which causes multiple Domain Events, model it  and place it to the left of the multiple Domain Events that caused.

3- Associate the Entity/Aggregate on which the Command is executed and that produces the Domain Event outcome
-   Use Data/Entity (not Aggregate jargon) to communicate with business
-   Place the Aggregate sticky note behind and slightly above the Command and Domain Event pairs.
-   Don’t rearrange your timeline as you find that Aggregates are repeatedly used. Rather, create the same Aggregate noun on multiple sticky notes and place them repeatedly on the timeline
-   As you iterate don't ignore new Domain Events discoveries, rather place them along side of their Commands and Aggregates.

4- Draw boundaries and lines with arrows to show flow on  modeling surface and circule the Bounded Contexts

5- Identify the various views (user interface) that users will need to carry out their actions, and important roles for various users

6- Other Tools: 
-   Introduce high-level executable specifications that follow the given/when/then approach. Need to be timeboxed, requires between 15% to 25% more time and effort to use and maintain executable specifications
-   Try Impact Mapping to make sure the software you are designing is a Core Domain
-   User Story Mapping : understand what software features you should be investing in (Core domain)

> Note : The closer the storming is to the big picture, the farther is from its actual implementation.




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
 - in some cases bounded context are only logically (and not physically) separated (same project and same databases with separate schemas), which is not the case in [Microservices](http://bit.ly/1dI7ZJQ) (separate deployments and processes run)!
 
The physical vs logical separation is trade-off between easy to maintain with proper isolation vs bigger maintenance overhead
 


**Interface abuse in DDD**
- No business logic : Implementation could lead to potential duplication 
- Can do : a promise or a contract to do something but doesn't say anything about any eventual relationship or hierarchy of the classes

[**_.Net Value type VS Value Object_**](https://enterprisecraftsmanship.com/posts/net-value-type-ddd-value-object)

- .Net Value type :.Net Implementation details, Immutable, Structural equality
- Value Object : Design Object, Immutable, Structural equality...



**_Why not use struct (i.e. .Net Value type) instead of value object? :_**

- Struct doesn't support inheritance => equality should be implemented in each struct separately which lead to code duplication.
- Struct doesn't interact very well with ORM's
- Value Objects allow abstraction of multiple fields (e.g SnackPile {snack, quantity, price}, Slot acts as a host for SnackPile), it also has invariants which protect the internal state of the aggregate/entity at all time, there are light-weight => when it makes sense  move as much as possible business logic into them bearing in mind of course the Single Responsibility principle and Separation of Concerns...
- Value Objects are immutable and should remain so (e.g. instead of subtracting quantity we create a new object bearing the same quantity, see SnackPile.SubtractOne())


Unit test should cover at most inner layer (entities, value object, domain events, aggregates) : [read more ...](http://bit.ly/1XF0J6H)
 
 

**_Application Services VS Domain Layer_**
-------------------------------------------

- UI = XAML VIEW = HTLM VIEW
- Application Services = View Model = Controllers

Application Services acts as mediator between the outside world and domain layer (Entities, Value objects, Domain Events, Aggregates and Domain services + repo + Factories), It shouldn't contains any business logic, it only coordinates and delegates inputs from UI and pass it to the domain layer for validation. 
For instance, In WPF View Model (Application Services) acts as wrapper on top of an entity, it increases it with the functionality required by the view, it doesn't contain any business logic and rather delegated it to the entity (eventually domain service), it also wraps other domain classes such as repositories... 

Rule of thumb when deciding on where to put a piece of logic:
- If the piece of logic makes sense only in the UI context that it would be better to place it in the Application Service layer
- Otherwise it should be in the domain layer. 

**_Why persist a value object is a bad Idea ?_**
------------------------------------------------
This is about Snackmachine example (where MoneyId as FK) and Money (with MoneyId as PK)
- It will require an Id, which goes against the definition of value object (structural + reference equality)
- It (Money) will have a lifetime of its own (we could delete Snackmachine object without deleting Money row), it violates the rules that a value object lifetime should fully depend on the entities lifetime.

> *_Initialization :_*
>> It's good practice to do initialization as close a possible to the startup (factory initialisation, IOC, ...), e.g. in WPF inside the App Class, WebApi Startup class...

**_Aggregates(DDD Tactical Design)_**
-------------------------
Aggregate (i.e. root entity) is design pattern that help us to simplify the domain model by gathering multiple entities under a single abstraction.
  - It's a conceptual whole, i.e. it represents a cohesive notion of domain model
  > Cohesion is the drive to have related code all grouped together
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
 - Avoid creating aggregates that are too large (Aggregates clusters) because it hard to maintain consistency if you have to update/store partial part of the aggregate.
 - Proper boundaries are sometime a trade-off between simplicity and performance, in that case newly split aggregates are referenced by their id's in the aggregate root which were part of, they will have their own transaction boundary. If those aggregate lives in the same bounded context and deployed as single physical unit, then use Domain Events to communicate between them and skip the messaging infrastructure such as NServicebus (immediate consistency), otherwise we have to go through the messaging infrastructure (eventual consistency).
 - There's no limit on how many value objects are in the aggregate.
 - 1-to-many should be 1-to-some, need to extract to different aggregate: e.g. SnackMachine and PurchaseLog, separate the PurchaseLog and use domain event to communicate between SnackMachine and PurchaseLog.  
 - Do NOT expose internal entities outside the aggregate
  

*_AggregateRoot abstract class has three major goals:_*
 --------------------------------------------------------
- More readable code base : which entities are AggregateRoot and which ones are part of an existing Aggregate
- Optimistic locking : if it's introduced we need to version the Aggregate
- Perfect place to hold domain events that happens to an Aggregate during its lifetime ,in our case we will use domain event and optimistic locking so we need it, otherwise we don't use it.

**_Repository_**
 --------------------------------------------------------
 Design pattern for encapsulating the communication with the database, the client should gather required domain object as if they reside in memory by calling a single method (no additional effort is required).
 
 
 *_How many repositories should we create for domain model_*
 -----------------------------------------------------------
- The general rule it should be a repository per aggregate. e.g. SnackMachineRepository & SnackRepository, they should get eagerly/lazy all entities of the aggregate. All sub-entities should be automatically saved with aggregate (don't forget to SaveUpdate() in FluentNHibernate, for more info check Slot mapping in SnackMachineMap class).

- Repository public method should work only with aggregates/aggregateRoots/Root entities.
- If we have to retrieve a sub-entity we need to get aggregate first and then access to the sub-entity. e.g. SnackMachine snackMachine= repository.GetBySlotId(slotId) here we get SnackMachine and not slot.
- In Web based app we don't work with domain object in deattached mode, thus the lazy loading makes sense, the reverse is true for desktop apps (domain objects are in attached mode).

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
- Always Valid respects inherently the DRY principle, it may never lead to code duplication, due to human factor (e.g. hard to remember the central place for validation by all team member)
- In Always Valid, each class maintain its invariants and remain all time in a valid state
for more about [Fail fast principle](http://bit.ly/1RrHvj8) 

*_Domain Services_*
----------------------------------------
 - Don't have state per their own
 - Contain some domain logic
 - Contains knowledge that doesn't belong neither to entities not to value objects
 
*_Domain Services Vs Application Service_*
----------------------------------------
 - Domain Services : resides inside of the domain layer, Contains domain logic and doesn't communicate with the outside world 
 - Application Services: resides outside of the domain layer, Communicates with the outside world and doesn't contain domain logic, they delegates execution to domain classes such as entities, value objects, repositories and domain services. 
