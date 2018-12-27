using SnackMachineDDD.logic.Common;
/*************************************************************************************
 The single responsibility principle implies that repositories should fetch only objects 
 from the database and save it back. They shouldn't hold instances of anything or implement 
 any functionality (e.g. They shouldn't hold instances of anything such as HeadOffice Instance).
 **************************************************************************************/
namespace SnackMachineDDD.logic.Management
{
    public class HeadOfficeRepository : Repository<HeadOffice> 
    {

    }
}
