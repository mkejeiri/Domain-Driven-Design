using SnackMachineDDD.UI.Common;

namespace SnackMachineDDD.UI.Utils
{
    public class DialogService
    {
        public bool? ShowDialog(ViewModel viewModel)
        {
            CustomWindow window = new CustomWindow(viewModel);
            return window.ShowDialog();
        }
    }
}
