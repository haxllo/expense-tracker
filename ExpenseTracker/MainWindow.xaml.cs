using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.IsOpen = true;
            }
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            if (app != null)
            {
                foreach (var window in app.Windows.Cast<Window>().Where(w => w != this).ToList())
                {
                    window.Close();
                }

                app.Shutdown();
            }
            else
            {
                Close();
            }
        }
    }
}
