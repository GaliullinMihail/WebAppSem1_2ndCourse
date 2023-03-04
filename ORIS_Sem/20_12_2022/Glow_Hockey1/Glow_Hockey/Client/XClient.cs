using System.Net.Sockets;
using System.Net;
using XProtocol;

namespace Glow_Hockey.Client
{
    public static class XClient
    {
        public static Action<byte[]> OnPacketRecieve { get; set; }

        private static readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();

        private static Socket _socket;
        private static IPEndPoint _serverEndPoint;

        public static void Connect(string ip, int port)
        {
            Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        }

        static XClient()
        {
            XClient.OnPacketRecieve += OnPacketRecieveMethod;
            XClient.Connect("127.0.0.1", 4910);
        }

        private static void OnPacketRecieveMethod(byte[] packet)
        {
            var parsed = XPacket.Parse(packet);

            if (parsed != null)
            {
                ProcessIncomingPacket(parsed);
            }
        }

        public static void Connect(IPEndPoint server)
        {
            _serverEndPoint = server;

            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(_serverEndPoint);

            Task.Run((Action)RecievePackets);
            Task.Run((Action)SendPackets);
        }

        public static void QueuePacketSend(byte[] packet)
        {
            if (packet.Length > 256)
            {
                throw new Exception("Max packet size is 256 bytes.");
            }

            _packetSendingQueue.Enqueue(packet);
        }

        private static void RecievePackets()
        {
            while (true)
            {
                var buff = new byte[256];
                _socket.Receive(buff);

                buff = buff.TakeWhile((b, i) =>
                {
                    if (b != 0xFF) return true;
                    return buff[i + 1] != 0;
                }).Concat(new byte[] { 0xFF, 0 }).ToArray();

                OnPacketRecieve?.Invoke(buff);
            }
        }

        private static void SendPackets()
        {
            while (true)
            {
                if (_packetSendingQueue.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                var packet = _packetSendingQueue.Dequeue();
                _socket.Send(packet);

                Thread.Sleep(100);
            }
        }

        private static void ProcessIncomingPacket(XPacket packet)
        {
            var type = XPacketTypeManager.GetTypeFromPacket(packet);

            switch (type)
            {
                //case XPacketType.Handshake:
                //    ProcessHandshake(packet);
                //    break;
                case XPacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
