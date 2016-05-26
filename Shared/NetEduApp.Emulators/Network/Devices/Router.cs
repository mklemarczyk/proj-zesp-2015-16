﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
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
                if (ipInterface.Address != null && ipInterface.Address.Address == data.DestinationAddress?.Address) {
                    return;
                }
            }
            if (data.TTL > 0) {
                data.TTL--;
                this.SendData(data);
            }
        }
        public void SendData(INetPacket data) {
            if (data.DestinationAddress != null) {
                NetAddress? target = null;
                foreach (var ipInterface in interfaces) {
                    if (ipInterface.Address != null && ipInterface.Address.GetNetwork( ).Contains(data.DestinationAddress.Value) == true) {
                        EmulatorLogger.Log(LogLevel.Info, EventType.RouteFoundConnected, string.Empty);
                        ipInterface.SendData(data);
                    }
                }
                if (target == null) {
                    foreach (var route in this.Routes) {
                        if (route.IsMatch(data.DestinationAddress.Value)) {
                            EmulatorLogger.Log(LogLevel.Info, EventType.RouteFound, string.Empty);
                            target = route.Target;
                        }
                    }
                }
                if (target == null && this.DefaultRoute != null) {
                    EmulatorLogger.Log(LogLevel.Info, EventType.RouteDefaultUsed, string.Empty);
                    target = this.DefaultRoute.Target;
                }
                if (target != null) {
                    foreach (var ipInterface in interfaces) {
                        if (ipInterface.Address != null && ipInterface.Address.GetNetwork( ).Contains(target.Value) == true) {
                            EmulatorLogger.Log(LogLevel.Info, EventType.PacketRouted, string.Empty);
                            ipInterface.SendData(data);
                        }
                    }
                } else {
                    EmulatorLogger.Log(LogLevel.Info, EventType.RouteNotFound, string.Empty);
                }
            }
        }
    }
}
