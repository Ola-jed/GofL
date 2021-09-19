using System.Windows;
using System.Windows.Threading;

namespace GofL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message,"Something weird happened",MessageBoxButton.OK,MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}