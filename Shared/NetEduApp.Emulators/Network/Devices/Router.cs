using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Devices {
    internal class Router : IRouter {
        private INetEmulator emulator;
        private List<INetLgInterface> interfaces;
        private List<INetRoute> routes;

        internal Router(INetEmulator emulator, string name) {
            this.emulator = emulator;
            this.Name = name;
            this.interfaces = new List<INetLgInterface>( );
            this.routes = new List<INetRoute>( );
            this.interfaces.Add(new NetLgInterface(this, name + "1"));
            this.interfaces.Add(new NetLgInterface(this, name + "2"));
            this.interfaces.Add(new NetLgInterface(this, name + "3"));
            this.interfaces.Add(new NetLgInterface(this, name + "4"));
        }

        public INetEmulator Emulator { get { return emulator; } }
        public IReadOnlyList<INetLgInterface> Interfaces { get { return interfaces; } }
        public int PortCount { get { return interfaces.Count; } }
        public IList<INetRoute> Routes { get { return routes; } }
        public string Type { get { return "Computer"; } }

        public INetRoute DefaultRoute { get; set; }

        public string Name { get; set; }

        public void ReceiveData(INetPacket data) {
            foreach (var ipInterface in interfaces) {
                if (ipInterface.Address != null && ipInterface.Address.ToString( ) == data.DestinationAddress.Address.ToString( )) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("{0} recived {1}", this.Name, data);
#endif
                    return;
                }
            }
            if (data.TTL > 0) {
                data.TTL--;
                this.SendData(data);
            }
        }
        public void SendData(INetPacket data) {
            INetAddress target = null;
            foreach (var ipInterface in interfaces) {
                if (ipInterface.Address != null && ipInterface.Address.GetNetwork( ).Contains(data.DestinationAddress) == true) {
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
                    if (ipInterface.Address != null && ipInterface.Address.GetNetwork( ).Contains(target) == true) {
                        ipInterface.SendData(data);
                    }
                }
            }
        }
    }
}
