using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Emulators
{ 
	public interface IHaveOutputHistory
	{
		public CurrentHistory OutputHistory { get; }
	}
}