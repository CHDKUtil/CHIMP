using Chimp.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Chimp.Converters
{
    sealed class ProgressValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length != 2 || !(values[0] is StepViewModel stepViewModel) || !(values[1] is int selectedIndex))
                return null;

            var index = Enumerable.Range(0, selectedIndex)
                .Count(i => stepViewModel.Items[i].IsVisible);

            var length = stepViewModel.Items
                .Count(s => s.IsVisible);

            return (double)(index + 1) / length;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
