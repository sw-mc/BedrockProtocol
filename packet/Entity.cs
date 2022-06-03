using BedrockProtocol.Serializer;
using SkyWing.Binary;

namespace BedrockProtocol.Packet;

public class AddEntityPacket : DataPacket, ClientboundPacket {

    public int EntityId { get; set; }
    
    public override int Pid() {
        return PacketIds.ADD_ENTITY_PACKET;
    }

    public override string GetName() {
        return "AddEntity";
    }

    public static AddEntityPacket Create(int netId) {
        return new AddEntityPacket {
            EntityId = netId
        };
    }

    public override void EncodePayload(PacketSerializer outgoing) {
        outgoing.WriteUnsignedVarInt(BinaryStream.UnSignInt(EntityId));
    }

    public override void DecodePayload(PacketSerializer incoming) {
        EntityId = (int) incoming.ReadUnsignedVarInt();
    }
}

public class AnimateEntityPacket : DataPacket, ClientboundPacket {

    public string   Animation;
    public string   NextState;
    public string   StopExpression;
    public int      StopExpressionVersion;
    public string   Controller;
    public float    BlendOutTime;
    public long[] ActorRuntimeIds;

    public override int Pid() {
        return PacketIds.ANIMATE_ENTITY_PACKET;
    }

    public override string GetName() {
        return "AnimateEntity";
    }

    public static AnimateEntityPacket Create(string animation, string nextState, string stopExpression,
        int stopExpressionVersion, string controller, float blendOutTime, long[] actorRuntimeIds) {
        
        return new AnimateEntityPacket {
            Animation             = animation,
            NextState             = nextState,
            StopExpression        = stopExpression,
            StopExpressionVersion = stopExpressionVersion,
            Controller            = controller,
            BlendOutTime          = blendOutTime,
            ActorRuntimeIds       = actorRuntimeIds
        };
    }


    public override void EncodePayload(PacketSerializer outgoing) {
        outgoing.WriteString(Animation);
        outgoing.WriteString(NextState);
        outgoing.WriteString(StopExpression);
        outgoing.WriteLInt(StopExpressionVersion);
        outgoing.WriteString(Controller);
        outgoing.WriteLFloat(BlendOutTime);
        outgoing.WriteUnsignedVarInt((uint) ActorRuntimeIds.Length);
        foreach (var actorRuntimeId in ActorRuntimeIds) {
            outgoing.WriteUnsignedVarLong((ulong) actorRuntimeId);
        }
    }
    
    public override void DecodePayload(PacketSerializer incoming) {
        Animation = incoming.ReadString();
        NextState = incoming.ReadString();
        StopExpression = incoming.ReadString();
        StopExpressionVersion = incoming.ReadLInt();
        Controller = incoming.ReadString();
        BlendOutTime = incoming.ReadLFloat();
        var length = incoming.ReadUnsignedVarInt();
        ActorRuntimeIds = new long[length];
        for (var i = 0; i < length; i++) {
            ActorRuntimeIds[i] = (long) incoming.ReadUnsignedVarLong();
        }
    }
}