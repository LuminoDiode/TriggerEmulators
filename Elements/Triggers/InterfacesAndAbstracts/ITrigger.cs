using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
    public interface ITrigger
    {
		public bool IsInClockedMode { get; set; }

		public TRIGGER_STATES CurrentState { get; set; }
	}
}
