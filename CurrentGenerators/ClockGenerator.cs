using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Emulators
{
	/// <summary>
	/// An abstract clock generator. Generates square impulses and writes it's history.
	///	Default parameters is 1 Volt, 1 Hz, 0.1 sec high voltage phase duration.
	/// </summary>
	public class ClockGenerator : ICurrentGenerator
	{
<<<<<<< Updated upstream:CurrentGenerators/ClockGenerator.cs
=======
		public static bool AutoRun { get; set; }
		private bool _IsRunning;
		private CancellationTokenSource _MyCancellationTokenSource;
		public bool IsRunning
		{
			get => _IsRunning;
			set
			{
				if (value == _IsRunning) return;
				else if (value == false)
				{
					_MyCancellationTokenSource.Cancel();
					_IsRunning = false;
					CurrentLevel_Volt = float.NaN;
				}
				else if (value == true)
				{
					CurrentLevel_Volt = 0f;
					_MyCancellationTokenSource = new CancellationTokenSource();
					this.ExecuteAsync(_MyCancellationTokenSource.Token);
					_IsRunning = false;
				}
			}
		}


>>>>>>> Stashed changes:Elements/CurrentGenerators/ClockGenerator.cs
		public event EventHandler OutputChanged;
		/// <summary>
		/// Voltage of low level phase (see <see cref="CLOCK_GENERATOR_PHASE"/>).
		/// </summary>
		public float LowLevel_Volt { get; set; } = 0f;
		/// <summary>
		/// Voltage of high level phase (see <see cref="CLOCK_GENERATOR_PHASE"/>).
		/// </summary>
		public float HighLevel_Volt { get; set; } = 1f;

		private float _LowLevelDuration_sec = 0.9f;
		/// <summary>
		/// Low level voltage phase duration (seconds).
		/// Changing this invokes <see cref="FrequencyChanged"/>.
		/// </summary>
		public float LowLevelDuration_sec
		{
			get => _LowLevelDuration_sec;
			set
			{
				_LowLevelDuration_sec = value;
			}
		}

		private float _HighLevelDuration_sec = 0.1f;
		/// <summary>
		/// High level voltage phase duration (seconds).
		/// Changing this invokes <see cref="FrequencyChanged"/>.
		/// </summary>
		public float HighLevelDuration_sec
		{
			get => _HighLevelDuration_sec;
			set
			{
				_HighLevelDuration_sec = value;
			}
		}
		/// <summary>
		/// Generator frequency. On set, it does not change <see cref="LowLevelDuration_sec"/> to <see cref="HighLevelDuration_sec"/> ratio.
		/// For example, 1 Hz 0.9sec/0.1sec => 2 Hz 1.8/0.2sec. This property is calculated from <see cref="LowLevelDuration_sec"/> and <see cref="HighLevelDuration_sec"/>.
		/// Changing this invokes <see cref="FrequencyChanged"/>.
		/// </summary>
		public float Clock_Hz
		{
			get => 1 / (this.LowLevelDuration_sec + this.HighLevelDuration_sec);
			set
			{
				var PerSec = 1 / value;
				var HightToAll = this.HighLevelDuration_sec / (this.LowLevelDuration_sec + this.HighLevelDuration_sec);
				var LowToAll = 1 - HightToAll;
				this.LowLevelDuration_sec = (1 / value) * LowToAll;
				this.HighLevelDuration_sec = (1 / value) * HightToAll;
			}
		}
		/// <summary>
		/// Possible generator phases.
		/// </summary>
		public enum CLOCK_GENERATOR_PHASE
		{
			LowVoltagePhase,
			HightVoltagePhase
		}
		/// <summary>
		/// Current generator phase, see <see cref="CLOCK_GENERATOR_PHASE"/>.
		/// </summary>
		public CLOCK_GENERATOR_PHASE CurrentPhase { get; private set; }
		private float _CurrentLevel_Volt;

		/// <summary>
		/// Current generator voltage.
		/// Changing this invokes <see cref="LevelChanged"/>.
		/// </summary>
		public float CurrentLevel_Volt
		{
			get => this._CurrentLevel_Volt;
			private set
			{
				this._CurrentLevel_Volt = value;
				this.OutputChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Hostory of generator's current.
		/// Writes new record when generator's voltage changes.
		/// </summary>
		public AHistory History { get; private set; }

		public ClockGenerator()
		{
<<<<<<< Updated upstream:CurrentGenerators/ClockGenerator.cs
			this.History = new CurrentHistory(this);
=======
			this.OutputHistory = new CurrentHistory(this);
			if (AutoRun) { IsRunning = true; }
>>>>>>> Stashed changes:Elements/CurrentGenerators/ClockGenerator.cs
		}


		public bool IsExecuting { get; private set; } = false;
		/// <summary>
		/// Runs generator.
		/// </summary>
<<<<<<< Updated upstream:CurrentGenerators/ClockGenerator.cs
		public async Task ExecuteAsync(CancellationToken ct)
=======
		private async void ExecuteAsync(CancellationToken ct)
>>>>>>> Stashed changes:Elements/CurrentGenerators/ClockGenerator.cs
		{
			IsExecuting = true;
			while (!ct.IsCancellationRequested)
			{
				Console.Write("Im in execute async cycle");
				this.CurrentPhase = CLOCK_GENERATOR_PHASE.HightVoltagePhase;
				this.CurrentLevel_Volt = this.HighLevel_Volt;
				await Task.Delay((int)(this.HighLevelDuration_sec * 1000));

				this.CurrentPhase = CLOCK_GENERATOR_PHASE.LowVoltagePhase;
				this.CurrentLevel_Volt = this.LowLevel_Volt;
				await Task.Delay((int)(this.LowLevelDuration_sec * 1000));
			}
		}
	}
}
