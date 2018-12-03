using Pencils.ViewModels;
using Pencils.Views;
using System.Windows;

namespace Pencils
{
    public partial class App : Application
    {
        /// <summary>
        /// This App is just the WriterView, so initialize that and show it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartApp(object sender, StartupEventArgs e)
        {
            var writer = new WriterView { DataContext = new WriterViewModel() };
            writer.Show();
        }
    }
}
