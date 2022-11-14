using System;
using System.Windows;

namespace BitmapCompressor.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindowView();
            MainWindow.DataContext = new MainWindowViewModel(new BlockCompressor());
            MainWindow.Show();
        }
    }
}
