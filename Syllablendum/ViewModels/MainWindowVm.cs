using GalaSoft.MvvmLight;

namespace Syllablendum.ViewModels
{
	public class MainWindowVm : ViewModelBase
	{
		public SyllableGameVm SyllableGame { get; set; } = new SyllableGameVm();
		public SyllableTimeGameVm SyllableTimeGame { get; set; } = new SyllableTimeGameVm();
	}
}
