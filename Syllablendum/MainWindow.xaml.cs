using Syllablendum.ViewModels;

namespace Syllablendum
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVm();
        }
    }
}