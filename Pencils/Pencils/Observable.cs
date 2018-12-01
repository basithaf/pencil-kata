using System.ComponentModel;

namespace Pencils
{
    /// <summary>
    /// This class wraps an implementation of INotifyPropertyChanged
    /// as a generic type for use in Models and ViewModels whose
    /// properties are variable and shown in a view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observable<T> : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            // This syntax means "call method/handler if not null"
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private T _value;
        public T Value
        {
            get => _value;
            set => SetValue(value);
        }

        private void SetValue(T newValue)
        {
            if (!newValue.Equals(_value))
            {
                SetValueSilently(newValue);
                NotifyPropertyChanged("Value");
            }
        }

        private void SetValueSilently(T newValue)
        {
            _value = newValue;
        }

        public Observable(T initialValue = default(T))
        {
            SetValueSilently(initialValue);
        }
    }
}
