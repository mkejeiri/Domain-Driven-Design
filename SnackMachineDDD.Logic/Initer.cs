using System.Configuration;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 initialize the SessionFactory: perform all initialization as close as possible to the startup (aka composition root) 
 of the application. We can enable decoupling between the UI and the utility classes 
 by gathering all initialization logic in a special class, and then use it in the composition root
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SnackMachineDDD.logic
{
    public static class Initer
    {
        public static void Init()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SnackMachineDDDDatabase"].ConnectionString;
            SessionFactory.Init(connectionString);
        }
    }
}
