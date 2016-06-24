using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Simulator.Abstract {
	public interface INetLgInterface : INetHwInterface {
		NetAddress? Address { get; set; }
	}
}
