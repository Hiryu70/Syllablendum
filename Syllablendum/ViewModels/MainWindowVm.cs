using GalaSoft.MvvmLight;

namespace Syllablendum.ViewModels
{
	public class MainWindowVm : ViewModelBase
	{
		public TrueFalseGameVm TrueFalseGame { get; set; } = new TrueFalseGameVm();
	}
}
