﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TCPServer.Hockey;

namespace TCPServer
{
    public class XServer
    {
        private readonly Socket _socket;
        private readonly List<ConnectedClient> _clients;
        private Game _game;

        private bool _listening;
        private bool _stopListening;

        public XServer()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];


            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<ConnectedClient>();
            _game = new Game();
        }

        public void Start()
        {
            if (_listening)
            {
                throw new Exception("Server is already listening incoming requests.");
            }

            _socket.Bind(new IPEndPoint(IPAddress.Any, 4910));
            _socket.Listen(10);

            _listening = true;
        }

        public void Stop()
        {
            if (!_listening)
            {
                throw new Exception("Server is already not listening incoming requests.");
            }

            _stopListening = true;
            _socket.Shutdown(SocketShutdown.Both);
            _listening = false;
        }

        public void AcceptClients()
        {
            while (true)
            {
                if (_stopListening)
                {
                    return;
                }

                Socket client;

                try
                {
                    client = _socket.Accept();
                }
                catch { return; }

                Console.WriteLine($"[!] Accepted client from {(IPEndPoint)client.RemoteEndPoint}");

                var c = new ConnectedClient(client, _game);
                _clients.Add(c);
            }
        }
    }
}