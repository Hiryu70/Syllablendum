using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Syllablendum.ViewModels
{
	public class SyllableGameVm : ViewModelBase
	{
		private int _okCount;
		private int _wrongCount;
		private GameMode _gameMode;
		private string _syllable;
		private string _lastSyllable;
		private bool _allowChangeOrder;


		public SyllableGameVm()
		{
			OkCommand = new RelayCommand(Ok);
			WrongCommand = new RelayCommand(Wrong);
			ResetCommand = new RelayCommand(ResetGame);
			SwitchAllConsonantsCommand = new RelayCommand(SwitchAllConsonants);
			SwitchAllVowelsCommand = new RelayCommand(SwitchAllVowels);
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

		public string Syllable
		{
			get => _syllable;
			set => Set(ref _syllable, value);
		}

		public bool AllowChangeOrder
		{
			get => _allowChangeOrder;
			set => Set(ref _allowChangeOrder, value);
		}

		public GameMode GameMode
		{
			get => _gameMode;
			set => Set(ref _gameMode, value);
		}

		public RelayCommand OkCommand { get; set; }

		public RelayCommand WrongCommand { get; set; }

		public RelayCommand ResetCommand { get; set; }

		public RelayCommand SwitchAllConsonantsCommand { get; set; }

		public RelayCommand SwitchAllVowelsCommand { get; set; }


		private void ResetGame()
		{
			OkCount = 0;
			WrongCount = 0;
			GameMode = GameMode.Running;

			SetSyllable();
		}

		private void Ok()
		{
			OkCount++;
			CheckEndGameCondition();
			SetSyllable();
		}

		private void Wrong()
		{
			WrongCount++;
			CheckEndGameCondition();
			SetSyllable();
		}

		private void SwitchAllConsonants()
		{
			var anyEnabled = Consonants.Any(c => c.IsEnabled);
			foreach (LetterVm letter in Consonants)
			{
				letter.IsEnabled = !anyEnabled;
			}
		}

		private void SwitchAllVowels()
		{
			var anyEnabled = Vowels.Any(c => c.IsEnabled);
			foreach (LetterVm letter in Vowels)
			{
				letter.IsEnabled = !anyEnabled;
			}
		}

		private void SetSyllable()
		{
			int attempt = 5;
			do
			{
				Syllable = GetRandomSyllable();
				attempt--;
			} while (Syllable == _lastSyllable && attempt > 0);
			_lastSyllable = Syllable;
		}

		private void CheckEndGameCondition()
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
			LetterVm[] vowels = Vowels.Where(v => v.IsEnabled).ToArray();
			LetterVm[] consonants = Consonants.Where(v => v.IsEnabled).ToArray();

			if (vowels.Length == 0 || consonants.Length == 0)
			{
				return "ПА";
			}

			var random = new Random();
			LetterVm vovel = vowels[random.Next(vowels.Length)];
			LetterVm consonant = consonants[random.Next(consonants.Length)];

			string result;
			if (AllowChangeOrder)
			{
				var order = random.Next(2);
				result = order == 0
					? $"{consonant.Value}{vovel.Value}"
					: $"{vovel.Value}{consonant.Value}";
			}
			else
			{
				result = $"{consonant.Value}{vovel.Value}";
			}

			return result;
		}
	}
}
