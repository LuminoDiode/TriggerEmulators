using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
	internal class D_Trigger: LogicalDelayer
	{
		public D_Trigger(ICurrentSource D_ChannelInput, ICurrentSource C_ChannelInput) 
			:base(D_ChannelInput, C_ChannelInput)
		{

		}
	}
}
