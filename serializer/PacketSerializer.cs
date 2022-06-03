using System.Text;
using SkyWing.Binary;

namespace BedrockProtocol.Serializer;

public class PacketSerializer : BinaryStream {
	public PacketSerializer(int size) : base(size) {}

    public string ReadString() {
        return Encoding.UTF8.GetString(ReadBytes((int) ReadUnsignedVarInt()));
    }
    
    public void WriteString(string value) {
        WriteUnsignedVarInt(UnSignInt(value.Length));
        WriteBytes(Encoding.UTF8.GetBytes(value));
    }
}