using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Emulators
{
	public abstract class ACurrentSource : ICurrentSource
	{
		public event EventHandler OutputChanged;

		public CurrentHistory OutputHistory { get; protected set; }

		private float _CurrentLevel_Volt;
		public float CurrentLevel_Volt
		{
			get => this._CurrentLevel_Volt;
			set
			{
				this._CurrentLevel_Volt = value;
				OutputChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
