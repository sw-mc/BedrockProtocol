using BedrockProtocol.Serializer;
using SkyWing.Binary;

namespace BedrockProtocol.Packet;

public abstract class Packet{

	public abstract int Pid();

	public abstract string GetName();
	
	public abstract bool CanBeSentBeforeLogin();


	public abstract void Decode(PacketSerializer incoming);
	
	public abstract void Encode(PacketSerializer outgoing);
	
	public abstract bool Handler (PacketHandlerInterface handler);
}

public abstract class DataPacket : Packet{
	
	public static int NETWORK_ID = 0;
	public static int PID_MASK = 0x3ff;
	
	private static int SUBCLIENT_ID_MASK = 0x03; //2 bits
	private static int SENDER_SUBCLIENT_ID_SHIFT = 10;
	private static int RECIPIENT_SUBCLIENT_ID_SHIFT = 12;

	public long senderSubId = 0;
	public long recipientSubId = 0;

	public override int Pid(){
		return NETWORK_ID;
	}

	public override string GetName() {
		return GetShortName();
	}

	public abstract string GetShortName();
	
	public override bool CanBeSentBeforeLogin() {
		return false;
	}

	public sealed override void Decode(PacketSerializer incoming) {
		DecodeHeader(incoming);
		DecodePayload(incoming);
	}
	
	private void DecodeHeader(PacketSerializer incoming) {
		var header = incoming.ReadVarInt();
		var pid = header & PID_MASK;
		if (pid != NETWORK_ID) {
			throw new PacketDecodeException("Expected " + NETWORK_ID + " for packet ID, got " + pid);
		}
		senderSubId = (header >> SENDER_SUBCLIENT_ID_SHIFT) & SUBCLIENT_ID_MASK;
		recipientSubId = (header >> RECIPIENT_SUBCLIENT_ID_SHIFT) & SUBCLIENT_ID_MASK;
	}
	
	public abstract void DecodePayload(PacketSerializer incoming);
	
	public sealed override void Encode(PacketSerializer outgoing) {
		EncodeHeader(outgoing);
		EncodePayload(outgoing);
	}
	
	private void EncodeHeader(PacketSerializer outgoing) {
		outgoing.WriteVarInt(Convert.ToUInt32((uint) NETWORK_ID | senderSubId << SENDER_SUBCLIENT_ID_SHIFT | recipientSubId << RECIPIENT_SUBCLIENT_ID_SHIFT));
	}
	
	public abstract void EncodePayload(PacketSerializer outgoing);

	public override bool Handler(PacketHandlerInterface handler) {
		throw new NotImplementedException();
	}
}

public class PacketDecodeException : System.Exception {
	public PacketDecodeException(string message) : base(message) {
	}
}

public abstract class ClientboundPacket : Packet {
	
}

public abstract class ServerboundPacket : Packet {
	
}