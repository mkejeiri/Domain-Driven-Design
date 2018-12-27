using NHibernate;
using SnackMachineDDD.logic;
using SnackMachineDDD.logic.Atms;
using SnackMachineDDD.logic.SnackMachine;
using SnackMachineDDD.UI.Atm;
using SnackMachineDDD.UI.SnackMachine;

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

            //logic.SnackMachine.SnackMachine snackMachine = new SnackMachineRepository().GetById(1);
            //var viewModel = new SnackMachineViewModel(snackMachine);

            var atm = new AtmRepository().GetById(1);
            var viewModel = new AtmViewModel(atm);
            _dialogService.ShowDialog(viewModel);


        }
    }
}
