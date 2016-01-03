using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Devices {
    internal class Computer : IComputer {
        private INetEmulator emulator;
        private List<INetLgInterface> interfaces;
        private List<INetRoute> routes;

        internal Computer(INetEmulator emulator, string name) {
            this.emulator = emulator;
            this.Name = name;
            this.interfaces = new List<INetLgInterface>();
            this.routes = new List<INetRoute>();
            this.interfaces.Add(new NetLgInterface(emulator, this, name));
        }

        public INetEmulator Emulator { get { return emulator; } }
        public IReadOnlyList<INetLgInterface> Interfaces { get { return interfaces; } }
        public int PortCount { get { return interfaces.Count; } }
        public IList<INetRoute> Routes { get { return routes; } }
        public string Type { get { return "Computer"; } }

        public INetRoute DefaultRoute { get; set; }

        public string Name { get; set; }

        public void ReciveData(INetPacket data) {
            System.Diagnostics.Debug.WriteLine("{0} recived {1}", this.Name, data);
        }
        public void SendData(INetPacket data) {
            INetAddress target = null;
            foreach (var ipInterface in interfaces) {
                if (ipInterface.Address != null && ipInterface.Address.GetNetwork().Contains(data.DestinationAddress) == true) {
                    ipInterface.SendData(data);
                }
            }
            if (target == null) {
                foreach (var route in this.Routes) {
                    if (route.IsMatch(data.DestinationAddress)) {
                        target = route.Target;
                    }
                }
            }
            if (target == null && this.DefaultRoute != null) {
                target = this.DefaultRoute.Target;
            }
            if (target != null) {
                foreach (var ipInterface in interfaces) {
                    if (ipInterface.Address != null && ipInterface.Address.GetNetwork().Contains(target) == true) {
                        ipInterface.SendData(data);
                    }
                }
            }
        }

        public void SendPing(NetIpAddress ipAddress) {
            SendData(new NetPacket(null, null, null, new NetAddress(ipAddress)));
        }
    }
}
