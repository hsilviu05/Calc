using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calc.ViewModels;
using Calc.Views;

namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CalculatorViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new CalculatorViewModel();
            DataContext = _viewModel;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.HandleKeyInput(e.Key);

            // If ESC key is pressed, clear the result (equivalent to "C")
            if (e.Key == Key.Escape)
            {
                _viewModel.ClearCommand.Execute(null);
            }

            // If Enter key is pressed, execute equals operation
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                _viewModel.EqualsCommand.Execute(null);
            }
        }
    }
}
