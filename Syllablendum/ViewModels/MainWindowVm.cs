using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Syllablendum.ViewModels
{
	public class MainWindowVm : ViewModelBase
	{
		private int _okCount;
		private int _wrongCount;
		private GameMode _gameMode;
		private string _syllable;

		public MainWindowVm()
		{
			OkCommand = new RelayCommand(OnOk);
			WrongCommand = new RelayCommand(OnWrong);
			ResetCommand = new RelayCommand(ResetGame);
			char[] consonants = { 'Ц', 'К', 'Н', 'Г', 'Ш', 'Щ', 'З', 'Х', 
				'Ф', 'В', 'П', 'Р', 'Л', 'Д', 'Ж', 
				'Ч', 'С', 'М', 'Т', 'Б' };
			foreach (var consonant in consonants)
			{
				Consonants.Add(new LetterVm
				{
					Value = consonant.ToString(), 
					IsEnabled = true
				});
			}

			char[] vowels = { 'У', 'Е', 'Ы', 'А', 'О', 'Э', 'Я', 'И', 'Ю' };
			foreach (var vowel in vowels)
			{
				Vowels.Add(new LetterVm
				{
					Value = vowel.ToString(),
					IsEnabled = true
				});
			}

			ResetGame();
		}

		public List<LetterVm> Consonants { get; set; } = new List<LetterVm>();
		public List<LetterVm> Vowels { get; set; } = new List<LetterVm>();
		public bool OnlyOneOrder { get; set; }

		public string Syllable
		{
			get => _syllable;
			set => Set(ref _syllable, value);
		}

		public RelayCommand OkCommand { get; set; }
		public RelayCommand WrongCommand { get; set; }
		public RelayCommand ResetCommand { get; set; }

		public int OkCount
		{
			get => _okCount;
			set => Set(ref _okCount, value);
		}

		public int OkMaximum { get; set; } = 10;

		public int WrongCount
		{
			get => _wrongCount;
			set => Set(ref _wrongCount, value);
		}

		public int WrongMaximum { get; set; } = 10;

		public GameMode GameMode
		{
			get => _gameMode;
			set => Set(ref _gameMode, value);
		}


		public void ResetGame()
		{
			OkCount = 0;
			WrongCount = 0;
			GameMode = GameMode.Running;

			SetSyllable();
		}

		public void OnOk()
		{
			OkCount++;
			CheckEndGameCondition();
			SetSyllable();
		}

		public void OnWrong()
		{
			WrongCount++;
			CheckEndGameCondition();
			SetSyllable();
		}

		public void SetSyllable()
		{
			Syllable = GetRandomSyllable();
		}

		public void CheckEndGameCondition()
		{
			if (WrongCount == WrongMaximum)
			{
				GameMode = GameMode.Lose;
			}

			if (OkCount == OkMaximum)
			{
				GameMode = GameMode.Win;
			}
		}

		private string GetRandomSyllable()
		{
			var vowels = Vowels.Where(v => v.IsEnabled).ToArray();
			var consonants = Consonants.Where(v => v.IsEnabled).ToArray();

			var random = new Random();
			var vovel = vowels[random.Next(vowels.Length)];
			var consonant = consonants[random.Next(consonants.Length)];

			string result;
			if (OnlyOneOrder)
			{
				result = $"{consonant.Value}{vovel.Value}";
			}
			else
			{
				var order = random.Next(2);
				result = order == 0
					? $"{consonant.Value}{vovel.Value}"
					: $"{vovel.Value}{consonant.Value}";
			}

			return result;
		}
	}
}
