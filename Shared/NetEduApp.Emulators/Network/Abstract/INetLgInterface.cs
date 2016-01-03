using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetLgInterface : INetHwInterface {
		INetAddress Address { get; set; }
	}
}
