using System.Text;
using SkyWing.Binary;

namespace BedrockProtocol.Serializer;

public class PacketSerializer : BinaryStream {
	public PacketSerializer(int size) : base(size) {}

    public void WriteString(string value) {
        WriteUnsignedVarInt(UnSignInt(value.Length));
        WriteBytes(Encoding.UTF8.GetBytes(value));
    }
}