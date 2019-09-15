using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Event;
using System.Reflection;

namespace SnackMachineDDD.logic.Utils
{
    public static class SessionFactory
    {
        //SessionFactory isa heavyweight class, it should be one instance of it per each DB
        private static ISessionFactory _factory;


        /* Session = context in EF or LINQ to SQL
         It keeps track of all objects loaded from the database into memory, and automatically updates
         the corresponding roles in the database according to the changes made to those objects. 
         Session implements the unit of work design pattern, i.e. that it pushes all accumulated changes at once, 
         usually at the end of its lifetime. Common guidline => different session per user interaction between db and UI!*/
        public static ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        public static void Init(string connectionString)
        {
            _factory = BuildSessionFactory(connectionString);
        }

        private static ISessionFactory BuildSessionFactory(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add(
                        ForeignKey.EndsWith("ID"),
                        //All column in the tables should be treated as nullable in the table
                        ConventionBuilder.Property.When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set),
                            x => x.Not.Nullable()))
                    .Conventions.Add<TableNameConvention>()
                    .Conventions.Add<HiloConvention>()
                )
                /*
                 NHibernate provides several extension points that allow us to inject our code into its internal pipeline 
                 so that it would be executed only when some persistent event occurs. We process domain events only when 
                 the aggregate was successfully persisted into the database.                  
                 */
                //To be able to dispatch event after the unit of work is committed
                //This make sure that NHibernate aware of the DomainEventListener class, we need to specify it in the configuration
                .ExposeConfiguration(x =>
                {
                    x.EventListeners.PostCommitUpdateEventListeners = new IPostUpdateEventListener[]{new EventListerner()};
                    x.EventListeners.PostCommitDeleteEventListeners = new IPostDeleteEventListener[]{new EventListerner()};
                    x.EventListeners.PostCommitInsertEventListeners = new IPostInsertEventListener[]{new EventListerner()};
                    x.EventListeners.PostCollectionUpdateEventListeners = new IPostCollectionUpdateEventListener[]{new EventListerner()};
                });
            return configuration.BuildSessionFactory();
        }
        public class TableNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table($"[dbo].[{instance.EntityType.Name}]");
            }
        }

        /* High is the number of the batch, whereas low is the number of identifier in that batch
            this will avoid unnecessary round-trip to the DB to get the next ID VS the identity columns the SQL Server
            which required not only round-trip which doesn't play well with the concept of unit of work        
        */

        public class HiloConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {

                instance.Column($"{instance.EntityType.Name}ID");
                //void HiLo(string table, string column, string maxLo, string where);
                instance.GeneratedBy.HiLo("[dbo].[Ids]",  //table where all batch numbers are stored
                    "NextHigh", //the column name for the batch number itself
                    "9" , //This is the size of the batches, we could take it from a config file
                    $"EntityName='{instance.EntityType.Name}'" //filter statement, used to determine which
                                                               //of the rows NHibernate should look into
                                                               //to get the next ID for a given entity
                    );
            }
        }
    }
}
