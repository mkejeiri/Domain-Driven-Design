using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/******************************************************************************************************
     HeadOfficeInstance resides in the onion architecture in the same level as repository 
     since it plays the same role as a repository (second innermost layer on the diagram). 
     It means that classes from Domain model cannot access it. 
     Only classes from the same layer or upper can work with this singleton.
  *******************************************************************************************************/
namespace SnackMachineDDD.logic.Management
{
    public static class HeadOfficeInstance
    {
        //we use only ONE headoffice that has an Id of 1 and a singleton instance 
        //that lives throughout the lifetime of the application
        private const long HeadOfficeId = 1;
        private static readonly object syncLock = new object();
        private static HeadOffice _headOffice;
        public static HeadOffice Instance
        {
            get
            {
                lock (syncLock)
                {
                    if (_headOffice == null)
                    {
                        var repository = new HeadOfficeRepository();
                        _headOffice = repository.GetById(HeadOfficeId);
                    }
                    return _headOffice;
                }
            }
        }

        public static void Init()
        {
            lock (syncLock)
            {
                if (_headOffice == null)
                {
                    var repository = new HeadOfficeRepository();
                    _headOffice = repository.GetById(HeadOfficeId);
                }
            }
        }

        //public sealed class Singleton
        //{
        //    private static readonly Lazy<Singleton> lazy =
        //        new Lazy<Singleton>(() => new Singleton());

        //    public static Singleton Instance { get { return lazy.Value; } }

        //    private Singleton()
        //    {
        //    }
        //}

    }
}
