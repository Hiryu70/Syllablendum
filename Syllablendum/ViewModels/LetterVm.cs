using GalaSoft.MvvmLight;

namespace Syllablendum.ViewModels
{
	public class LetterVm : ViewModelBase
	{
		private string _value;
		private bool _isEnabled;

		public string Value
		{
			get => _value;
			set => Set(ref _value, value);
		}

		public bool IsEnabled
		{
			get => _isEnabled;
			set => Set(ref _isEnabled, value);
		}
	}
}