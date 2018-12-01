using Pencils.ViewModels;
using Pencils.Views;
using System.Windows;

namespace Pencils
{
    public partial class App : Application
    {
        private void StartApp(object sender, StartupEventArgs e)
        {
            var writer = new WriterView { DataContext = new WriterViewModel() };
            writer.Show();
        }
    }
}
