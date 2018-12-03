using System.Windows;
using System.Windows.Controls;

namespace Pencils
{
    /// <summary>
    /// A textbox that allows for binding to the CaretIndex
    /// </summary>
    public class BindableCaretTextBox : TextBox
    {
        public int BindableCaretIndex {
            get { return (int)GetValue(BindableCaretIndexProperty); }
            set { SetValue(BindableCaretIndexProperty, value); }
        }

        public static readonly DependencyProperty BindableCaretIndexProperty =
            DependencyProperty.Register("BindableCaretIndex", 
                typeof(int), typeof(BindableCaretTextBox),
                new PropertyMetadata(OnBindableCaretIndexChanged));

        /// <summary>
        /// Updated via binding, so update CaretIndex
        /// </summary>
        private static void OnBindableCaretIndexChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is BindableCaretTextBox tb)
            {
                tb.CaretIndex = tb.BindableCaretIndex;
            }
        }

        public BindableCaretTextBox() : base()
        {
            SelectionChanged += UpdateBindableCaretIndex;
        }

        /// <summary>
        /// CaretIndex was updated, so BindableCaretIndex should reflect that
        /// </summary>
        private void UpdateBindableCaretIndex(object sender, RoutedEventArgs e)
        {
            BindableCaretIndex = CaretIndex;
        }
    }
}
