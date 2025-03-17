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
            PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Handle special keys first
            switch (e.Key)
            {
                case Key.Enter:
                    if (_viewModel.EqualsCommand.CanExecute(null))
                    {
                        _viewModel.EqualsCommand.Execute(null);
                        e.Handled = true;
                    }
                    break;
                case Key.Escape:
                    if (_viewModel.ClearCommand.CanExecute(null))
                    {
                        _viewModel.ClearCommand.Execute(null);
                        e.Handled = true;
                    }
                    break;
            }

            // If the key wasn't handled by special cases, forward it to HandleKeyInput
            if (!e.Handled)
            {
                _viewModel.HandleKeyInput(e.Key);
                e.Handled = true;
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new Views.AboutWindow
            {
                Owner = this
            };
            aboutWindow.ShowDialog();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
