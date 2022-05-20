using SkyWing.Binary;

namespace BedrockProtocol.Serializer;

public class PacketSerializer : BinaryStream{

	public PacketSerializer(int size) : base(size) {
	}
}