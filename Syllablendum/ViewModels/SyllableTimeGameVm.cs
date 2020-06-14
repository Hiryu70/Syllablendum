using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Syllablendum.ViewModels
{
	public class SyllableTimeGameVm : ViewModelBase
	{
		private int _okCount;
		private GameMode _gameMode;
		private string _syllable;
		private string _lastSyllable;
		private bool _allowChangeOrder;
		private DispatcherTimer _timer;
		private Stopwatch _stopwatch;
		private long _timerValue;

		public SyllableTimeGameVm()
		{
			OkCommand = new RelayCommand(Ok);
			ResetCommand = new RelayCommand(ResetGame);
			StartCommand = new RelayCommand(StartGame);
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

		public long TimerValue
		{
			get => _timerValue;
			set
			{
				Set(ref _timerValue, value);
				if (_timerValue < 0)
				{
					GameOver(GameMode.Lose);
				}
			} 
		}

		public long TimerMaximum { get; set; } = 7000;

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

		public RelayCommand ResetCommand { get; set; }

		public RelayCommand StartCommand { get; set; }

		public RelayCommand SwitchAllConsonantsCommand { get; set; }

		public RelayCommand SwitchAllVowelsCommand { get; set; }


		private void StartGame()
		{
			GameMode = GameMode.Running;
			StartTimer();
		}

		private void ResetGame()
		{
			OkCount = 0;
			GameMode = GameMode.Start;
			SetSyllable();
			StopTimer();
		}

		private void Ok()
		{
			OkCount++;
			CheckEndGameCondition();
			SetSyllable();
			_stopwatch?.Restart();
		}

		private void GameOver(GameMode gameMode)
		{
			StopTimer();
			GameMode = gameMode;
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
			if (OkCount == OkMaximum)
			{
				GameOver(GameMode.Win);
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

		private void StartTimer()
		{
			_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(5)
			};
			_timer.Tick += TimerTick;
			_timer.Start();

			_stopwatch = new Stopwatch();
			_stopwatch.Start();
		}

		private void StopTimer()
		{
			if (_timer != null)
			{
				
				_timer.Stop();
				_timer = null;
			}

			if (_stopwatch != null)
			{
				_stopwatch.Stop();
				_stopwatch = null;
			}
		}

		private void TimerTick(object send, EventArgs e)
		{
			TimerValue = TimerMaximum - _stopwatch.ElapsedMilliseconds;
		}
	}
}
