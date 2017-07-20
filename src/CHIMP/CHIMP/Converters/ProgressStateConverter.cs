using Chimp.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shell;

namespace Chimp.Converters
{
    sealed class ProgressStateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 1 || !(values[0] is MainViewModel mainViewModel))
                return null;
            if (mainViewModel.IsError)
                return TaskbarItemProgressState.Error;
            if (mainViewModel.IsWarning)
                return TaskbarItemProgressState.Paused;
			if (mainViewModel.IsAborted)
				return TaskbarItemProgressState.Error;
			return TaskbarItemProgressState.Normal;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
