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
            Loser.Visibility = Visibility.Collapsed;
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

            var order = random.Next(2);
            var result = order == 0 
                ? $"{consonant}{vovel}" 
                : $"{vovel}{consonant}";
            
            return result;
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            _okCount++;
            CheckEndGameCondition();
            SetSyllable();
            ProgressBarOk.Value = _okCount;
            Ok.Content = $"Правильно {_okCount}";
        }
        
        private void Wrong_OnClick(object sender, RoutedEventArgs e)
        {
            _wrongCount++;
            CheckEndGameCondition();
            SetSyllable();
            ProgressBarWrong.Value = _wrongCount;
            Wrong.Content = $"Неправильно {_wrongCount}";
        }

        private void CheckEndGameCondition()
        {
            if (_wrongCount == (int)ProgressBarWrong.Maximum)
            {
                Syllable.Visibility = Visibility.Collapsed;
                Loser.Visibility = Visibility.Visible;
            }
            
            if (_okCount == (int)ProgressBarOk.Maximum)
            {
                Syllable.Visibility = Visibility.Collapsed;
                Winner.Visibility = Visibility.Visible;
            }
        }
        


        private void ResetGame_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ResetProgram();
        }
    }
}