using System;
using System.Windows;
using System.Windows.Input;

namespace Syllablendum
{
    public partial class MainWindow
    {
        private int _okCount;
        private int _wrongCount;
        private readonly string _lastSylalble = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            ResetProgram();
        }

        private void ResetProgram()
        {
            _okCount = 0;
            _wrongCount = 0;
            ProgressBarOk.Value = 0;
            ProgressBarWrong.Value = 0;
            Syllable.Visibility = Visibility.Visible;
            Winner.Visibility = Visibility.Collapsed;
            Ok.Content = "Правильно";
            Wrong.Content = "Неправильно";
            
            SetSyllable();
        }
        
        private void SetSyllable()
        {
            string newSyllable;
            do
            {
                newSyllable = GetRandomSyllable();
            } while (newSyllable == _lastSylalble);
            
            Syllable.Text = newSyllable;
        }

        private string GetRandomSyllable()
        {
            char[] vowels = {'У', 'Е', 'Ы', 'А', 'О', 'Э', 'Я', 'И', 'Ю'};
            char[] consonants = {'Ц','К','Н','Г','Ш','Щ','З','Х','Ф','В','П','Р','Л','Д','Ж','Ч','С','М','Т','Б'};
            
            var random = new Random();
            var vovel = vowels[random.Next(vowels.Length)];
            var consonant = consonants[random.Next(consonants.Length)];
            var result = $"{consonant}{vovel}";
            return result;
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            SetSyllable();
            _okCount++;
            SetProgressBar();
            Ok.Content = $"Правильно {_okCount}";
        }

        private void SetProgressBar()
        {
            var trueBalance = _okCount - _wrongCount;
            if (trueBalance >= 0)
            {
                ProgressBarOk.Value = trueBalance;
                ProgressBarWrong.Value = 0;
            }
            else
            {
                ProgressBarWrong.Value = trueBalance * -1;
                ProgressBarOk.Value = 0;
            }

            if (trueBalance == (int)ProgressBarOk.Maximum)
            {
                Syllable.Visibility = Visibility.Collapsed;
                Winner.Visibility = Visibility.Visible;
            }
        }
        
        private void Wrong_OnClick(object sender, RoutedEventArgs e)
        {
            SetSyllable();
            _wrongCount++;
            SetProgressBar();
            Wrong.Content = $"Неправильно {_wrongCount}";
        }

        private void Winner_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ResetProgram();
        }
    }
}