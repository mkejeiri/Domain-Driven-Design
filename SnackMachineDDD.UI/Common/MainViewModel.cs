using NHibernate;
using SnackMachineDDD.logic;
using SnackMachineDDD.logic.SnackMachine;

namespace SnackMachineDDD.UI.Common
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            

            //using (ISession session = SessionFactory.OpenSession())
            //{
            //    SnackMachine snackMachine = session.Get<SnackMachine>(1L);
            //    var viewModel = new SnackMachineViewModel(snackMachine);
            //    _dialogService.ShowDialog(viewModel);
            //}

            SnackMachine snackMachine = new SnackMachineRepository().GetById(1);
            var viewModel = new SnackMachineViewModel(snackMachine);
            _dialogService.ShowDialog(viewModel);


        }
    }
}
