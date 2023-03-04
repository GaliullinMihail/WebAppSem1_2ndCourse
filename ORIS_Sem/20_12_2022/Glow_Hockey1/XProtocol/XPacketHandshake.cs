using XProtocol.Serializator;

namespace XProtocol
{
    public class XPacketHandshake
    {
        [XField(1)]
        public int MagicHandshakeNumber;
    }

    public class XPacketRegister
    {
    }

    public class XPacketAddToGame
    {
    }
}