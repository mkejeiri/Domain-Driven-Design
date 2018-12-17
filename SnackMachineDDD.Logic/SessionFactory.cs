using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;

namespace SnackMachineDDD.logic
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
                        ConventionBuilder.Property.When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set), x => x.Not.Nullable()))
                    .Conventions.Add<TablenNameConvention>()
                    .Conventions.Add<HiloConvention>()
                );
            return configuration.BuildSessionFactory();
        }
        public class TablenNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table($"[dbo].[{instance.EntityType.Name}]");
            }
        }

        /* High is the number of the batch, whereas low is the number of identifier in that batch*/
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
