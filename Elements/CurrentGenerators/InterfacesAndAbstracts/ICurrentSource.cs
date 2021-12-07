using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Emulators
{
	public interface ICurrentSource: IHaveOutputHistory
	{
		public event EventHandler OutputChanged;
		public float CurrentLevel_Volt { get; }
	}
}
