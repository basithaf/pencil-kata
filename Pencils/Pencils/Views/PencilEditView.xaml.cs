using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pencils.Views
{
    public partial class PencilEditView : UserControl
    {
        public PencilEditView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Validate that input is a number 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
