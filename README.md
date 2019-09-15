# Domain Driven Design (DDD)
This is merely an introduction to the DDD based on the work of [Vladimir Khorikov](https://enterprisecraftsmanship.com), this includes: 
  - Onion Architecture
  - Rich Domain Model
  - ORM such NHibernate which guarantees a [better degree of Domain Model Isolation than EF](https://enterprisecraftsmanship.com/2018/06/13/ef-core-vs-nhibernate-ddd-perspective/)
  - Domain Driven Design and Events Driven Design
  - WPF
  - Usage of HILO Algorithm when assigning ID's 
  - ...



### a Few words when implementing DDD 
Strategic design:
- Ubiquitous language: Bridges the gap between developers and experts
- Bounded context: Clear boundaries between different parts of the system
- Core domain: Focus on the most important part of the system


Interface: 
- No business logic : could lead to potential duplication
- Can do : a promise or a contract to do something but doesn't say anything about any eventual relationship or hierarchy of the classes


.Net Value type : Implementation details, Immutable, Structural equality
Value Object : Design Object, Immutable, Structural equality
Why not use struct (i.e. .Net Value type) instead of value object? : 
- Struct doesn't support inheritance => equality should be implemented in each struct separately which lead to code duplication.
- Struct doesn't interact very well with ROM's

Unit test should cover at most inner layer (entities, value object, domain events, aggregates) : [read more ...](http://bit.ly/1XF0J6H)
 
 

- UI = XAML VIEW = HTLM VIEW
- Application Services = View Model = Controllers

Application Services acts as mediator between the outside world and domain layer (Entities, Value objects, Domain Events, Aggregates and Domain services + repo + Factories), It shouldn't contains any business logic, it only coordinates and delegates inputs from UI and pass it to the domain layer for validation. 
For instance, In WPF View Model (Application Services) acts as wrapper on top of an entity it, augment it with the functionality required by the view, it doesn't contain any business logic and rather delegated it to the entity, it also wrap other domain classes as well such a repositories... 

Rule of thumb when deciding on where to put a piece of logic:
- If the piece of logic makes sense only in the UI context that it would be better to place it in the Application Service layer
- Otherwise it should be in the domain layer. 

**_Why persist a value object is a bad Idea ?_**

This about Snackmachine (where MoneyId as FK) and Money (with MoneyId as PK)
- It will require an Id, which goes against the definition of value object (structural + reference equality)
- It (Money) will have a lifetime of its own (we could delete Snackmachine object without deleting Money row), it violates the rules that a value object lifetime should fully depend on the entities lifetime.



