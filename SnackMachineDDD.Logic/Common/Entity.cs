using System;
using NHibernate.Proxy;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
  We should isolate our domain model from the persistence logic as much as possible, 
  it's not always feasible when using an ORM, we still need to adapt the domain layer and some persistence logic leaks 
  into the domain model. Luckily, the degree of the leak is not too big (unsealed class + virtual + Getype pb with lazy loading), 
  and the made tradeoff is worth it. we still preserve a lot of isolation for our domain model. For example, 
  we didn't have to change any of the existing tests that validate our domain classes. 
  So it is possible to maintain the same high degree of isolation even in larger projects.
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SnackMachineDDD.logic.Common
{

    //This uses for struct and valueObject, use a strong type as param, here YAGNI
    //public abstract class Entity : IEquatable<Entity> {
    //public bool Equals(Entity other)
    //{
    //    throw new NotImplementedException();
    //}
    //}

    //Nhibernate require all non-private members to be marked as virtual!
    public abstract class Entity
    {
        public virtual long Id { get; protected set; }


        public override bool Equals(object obj)
        {
            var other = obj as Entity;
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            //NHibernate creates proxy classes that inherit from the classes that are being wrapped.
            //In case of lazy loading, the GetType method will return the type of the proxy ,
            //not the type of the underlying entity. GetRealType would retrieve the real type of
            //the entity regardless of whether there is a proxy on top of it. 
            //if (GetType() != other.GetType())
            if (GetRealType() != other.GetRealType())
            {
                return false;
            }
            if (Id == 0 || other.Id == 0)
            {
                return false;
            }
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            //return (GetType().ToString() + Id).GetHashCode();
            return (GetRealType().ToString() + Id).GetHashCode();
        }
        
        //to avoid having proxy entity type in case of lazy loading instead of the real entity type
        public virtual Type GetRealType()
        {
            return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }


    }
}
