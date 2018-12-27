using System.Configuration;
using SnackMachineDDD.logic.Common;
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 initialize the SessionFactory: perform all initialization as close as possible to the startup (aka composition root) 
 of the application. We can enable decoupling between the UI and the utility classes 
 by gathering all initialization logic in a special class, and then use it in the composition root
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SnackMachineDDD.logic.Utils
{
    public static class Initer
    {
        //This called at the startup
        public static void Init()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SnackMachineDDDDatabase"].ConnectionString;
            SessionFactory.Init(connectionString);
            //initiate a singleton of headoffice at startup
            //even if the client try to access the instance through the get property HeadOfficeInstance.Instance,
            //it won't be able to create another instance!!!
            //HeadOfficeInstance.Init();
            DomainEvents.Init();
        }
    }
}
