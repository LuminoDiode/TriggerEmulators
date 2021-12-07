using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
	public interface ICurrentGenerator
	{
		public event EventHandler OutputChanged;
		public float CurrentLevel_Volt { get; }
		public CurrentHistory History { get; }
	}
}
