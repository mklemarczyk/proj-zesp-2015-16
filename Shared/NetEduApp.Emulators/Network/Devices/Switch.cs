using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Devices {
	internal class Switch : ISwitch {
		public INetEmulator Emulator
		{
			get
			{
				throw new NotImplementedException( );
			}
		}

		public IReadOnlyCollection<INetHwInterface> Interfaces
		{
			get
			{
				throw new NotImplementedException( );
			}
		}

		public string Name
		{
			get
			{
				throw new NotImplementedException( );
			}

			set
			{
				throw new NotImplementedException( );
			}
		}

		public int PortCount
		{
			get
			{
				throw new NotImplementedException( );
			}
		}

		public string Type
		{
			get
			{
				throw new NotImplementedException( );
			}
		}

		public void ReciveData(INetPacket data) {
			throw new NotImplementedException( );
		}

		public void SendData(INetPacket data) {
			throw new NotImplementedException( );
		}
	}
}
