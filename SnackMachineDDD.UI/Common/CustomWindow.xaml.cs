using System.Windows;

namespace SnackMachineDDD.UI.Common
{
    public partial class CustomWindow
    {
        public CustomWindow(ViewModel viewModel)
        {
            InitializeComponent();

            //Owner = Application.Current.MainWindow  ;
            DataContext = viewModel;
        }
    }
}
