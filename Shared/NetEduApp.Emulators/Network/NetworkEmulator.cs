using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
	internal class NetworkEmulator : INetEmulator {
		internal NetworkEmulator( ) {
			links = new Dictionary<INetHwInterface, INetHwInterface>( );
		}

		internal void MakeLink(INetHwInterface a, INetHwInterface b) {
			Disconnect(a);
			Disconnect(b);
			Connect(a, b);
		}

		internal void UnLink(INetHwInterface a) {
			Disconnect(a);
		}

		public void TransferData(INetPacket netPacket) {
			if (netPacket.DestinationInterface != null)
				TransferTo(netPacket.DestinationInterface, netPacket);
			else if (netPacket.SourceInterface != null)
				TransferFrom(netPacket.SourceInterface, netPacket);
		}

		public INetHwInterface GetDestination(INetHwInterface netHwInterface) {
			if (links.ContainsKey(netHwInterface))
				return links[netHwInterface];
			return null;
		}

		#region Private methods
		private Dictionary<INetHwInterface, INetHwInterface> links;

		private void Connect(INetHwInterface a, INetHwInterface b) {
			links.Add(a, b);
			links.Add(b, a);
		}

		private void Disconnect(INetHwInterface a) {
			if (links.ContainsKey(a)) {
				var b = links[a];
				links.Remove(a);
				Disconnect(b);
			}
		}

		private void TransferFrom(INetHwInterface source, INetPacket data) {
			if (links.ContainsKey(source)) {
				links[source].ReciveData(data);
			}
		}

		private void TransferTo(INetHwInterface destination, INetPacket data) {
			if (links.ContainsKey(destination) && links[destination] == data.SourceInterface) {
				destination.ReciveData(data);
			}
		}
		#endregion
	}
}
