using NHibernate;
namespace SnackMachineDDD.logic
{
   public abstract class Repository<T> 
               where T: AggregateRoot
    {
        public T GetById(long Id)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Get<T>(Id);
            }
        }

        public void Save(T aggregateRoot)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction()) 
                {
                    session.SaveOrUpdate(aggregateRoot);
                    transaction.Commit();
                }
            }
        }
    }
}
