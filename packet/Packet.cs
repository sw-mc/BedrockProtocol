using BedrockProtocol.Serializer;

namespace BedrockProtocol.Packet;

public abstract class Packet {

	public abstract int Pid();

	public abstract string GetName();
	
	public abstract bool CanBeSentBeforeLogin();


	public abstract void Decode(PacketSerializer incoming);
	
	public abstract void Encode(PacketSerializer outgoing);
	
	public abstract bool Handler (PacketHandlerInterface handler);
}

public abstract class DataPacket : Packet {
	
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

public interface ClientboundPacket {
	
}

public interface ServerboundPacket {
	
}


public class PacketIds {

	public const int LOGIN_PACKET = 0x01;
	public const int PLAY_STATUS_PACKET = 0x02;
	public const int SERVER_TO_CLIENT_HANDSHAKE_PACKET = 0x03;
	public const int CLIENT_TO_SERVER_HANDSHAKE_PACKET = 0x04;
	public const int DISCONNECT_PACKET = 0x05;
	public const int RESOURCE_PACKS_INFO_PACKET = 0x06;
	public const int RESOURCE_PACK_STACK_PACKET = 0x07;
	public const int RESOURCE_PACK_CLIENT_RESPONSE_PACKET = 0x08;
	public const int TEXT_PACKET = 0x09;
	public const int SET_TIME_PACKET = 0x0a;
	public const int START_GAME_PACKET = 0x0b;
	public const int ADD_PLAYER_PACKET = 0x0c;
	public const int ADD_ACTOR_PACKET = 0x0d;
	public const int REMOVE_ACTOR_PACKET = 0x0e;
	public const int ADD_ITEM_ACTOR_PACKET = 0x0f;

	public const int TAKE_ITEM_ACTOR_PACKET = 0x11;
	public const int MOVE_ACTOR_ABSOLUTE_PACKET = 0x12;
	public const int MOVE_PLAYER_PACKET = 0x13;
	public const int PASSENGER_JUMP_PACKET = 0x14;
	public const int UPDATE_BLOCK_PACKET = 0x15;
	public const int ADD_PAINTING_PACKET = 0x16;
	public const int TICK_SYNC_PACKET = 0x17;
	public const int LEVEL_SOUND_EVENT_PACKET_V1 = 0x18;
	public const int LEVEL_EVENT_PACKET = 0x19;
	public const int BLOCK_EVENT_PACKET = 0x1a;
	public const int ACTOR_EVENT_PACKET = 0x1b;
	public const int MOB_EFFECT_PACKET = 0x1c;
	public const int UPDATE_ATTRIBUTES_PACKET = 0x1d;
	public const int INVENTORY_TRANSACTION_PACKET = 0x1e;
	public const int MOB_EQUIPMENT_PACKET = 0x1f;
	public const int MOB_ARMOR_EQUIPMENT_PACKET = 0x20;
	public const int INTERACT_PACKET = 0x21;
	public const int BLOCK_PICK_REQUEST_PACKET = 0x22;
	public const int ACTOR_PICK_REQUEST_PACKET = 0x23;
	public const int PLAYER_ACTION_PACKET = 0x24;

	public const int HURT_ARMOR_PACKET = 0x26;
	public const int SET_ACTOR_DATA_PACKET = 0x27;
	public const int SET_ACTOR_MOTION_PACKET = 0x28;
	public const int SET_ACTOR_LINK_PACKET = 0x29;
	public const int SET_HEALTH_PACKET = 0x2a;
	public const int SET_SPAWN_POSITION_PACKET = 0x2b;
	public const int ANIMATE_PACKET = 0x2c;
	public const int RESPAWN_PACKET = 0x2d;
	public const int CONTAINER_OPEN_PACKET = 0x2e;
	public const int CONTAINER_CLOSE_PACKET = 0x2f;
	public const int PLAYER_HOTBAR_PACKET = 0x30;
	public const int INVENTORY_CONTENT_PACKET = 0x31;
	public const int INVENTORY_SLOT_PACKET = 0x32;
	public const int CONTAINER_SET_DATA_PACKET = 0x33;
	public const int CRAFTING_DATA_PACKET = 0x34;
	public const int CRAFTING_EVENT_PACKET = 0x35;
	public const int GUI_DATA_PICK_ITEM_PACKET = 0x36;
	public const int ADVENTURE_SETTINGS_PACKET = 0x37;
	public const int BLOCK_ACTOR_DATA_PACKET = 0x38;
	public const int PLAYER_INPUT_PACKET = 0x39;
	public const int LEVEL_CHUNK_PACKET = 0x3a;
	public const int SET_COMMANDS_ENABLED_PACKET = 0x3b;
	public const int SET_DIFFICULTY_PACKET = 0x3c;
	public const int CHANGE_DIMENSION_PACKET = 0x3d;
	public const int SET_PLAYER_GAME_TYPE_PACKET = 0x3e;
	public const int PLAYER_LIST_PACKET = 0x3f;
	public const int SIMPLE_EVENT_PACKET = 0x40;
	public const int EVENT_PACKET = 0x41;
	public const int SPAWN_EXPERIENCE_ORB_PACKET = 0x42;
	public const int CLIENTBOUND_MAP_ITEM_DATA_PACKET = 0x43;
	public const int MAP_INFO_REQUEST_PACKET = 0x44;
	public const int REQUEST_CHUNK_RADIUS_PACKET = 0x45;
	public const int CHUNK_RADIUS_UPDATED_PACKET = 0x46;
	public const int ITEM_FRAME_DROP_ITEM_PACKET = 0x47;
	public const int GAME_RULES_CHANGED_PACKET = 0x48;
	public const int CAMERA_PACKET = 0x49;
	public const int BOSS_EVENT_PACKET = 0x4a;
	public const int SHOW_CREDITS_PACKET = 0x4b;
	public const int AVAILABLE_COMMANDS_PACKET = 0x4c;
	public const int COMMAND_REQUEST_PACKET = 0x4d;
	public const int COMMAND_BLOCK_UPDATE_PACKET = 0x4e;
	public const int COMMAND_OUTPUT_PACKET = 0x4f;
	public const int UPDATE_TRADE_PACKET = 0x50;
	public const int UPDATE_EQUIP_PACKET = 0x51;
	public const int RESOURCE_PACK_DATA_INFO_PACKET = 0x52;
	public const int RESOURCE_PACK_CHUNK_DATA_PACKET = 0x53;
	public const int RESOURCE_PACK_CHUNK_REQUEST_PACKET = 0x54;
	public const int TRANSFER_PACKET = 0x55;
	public const int PLAY_SOUND_PACKET = 0x56;
	public const int STOP_SOUND_PACKET = 0x57;
	public const int SET_TITLE_PACKET = 0x58;
	public const int ADD_BEHAVIOR_TREE_PACKET = 0x59;
	public const int STRUCTURE_BLOCK_UPDATE_PACKET = 0x5a;
	public const int SHOW_STORE_OFFER_PACKET = 0x5b;
	public const int PURCHASE_RECEIPT_PACKET = 0x5c;
	public const int PLAYER_SKIN_PACKET = 0x5d;
	public const int SUB_CLIENT_LOGIN_PACKET = 0x5e;
	public const int AUTOMATION_CLIENT_CONNECT_PACKET = 0x5f;
	public const int SET_LAST_HURT_BY_PACKET = 0x60;
	public const int BOOK_EDIT_PACKET = 0x61;
	public const int NPC_REQUEST_PACKET = 0x62;
	public const int PHOTO_TRANSFER_PACKET = 0x63;
	public const int MODAL_FORM_REQUEST_PACKET = 0x64;
	public const int MODAL_FORM_RESPONSE_PACKET = 0x65;
	public const int SERVER_SETTINGS_REQUEST_PACKET = 0x66;
	public const int SERVER_SETTINGS_RESPONSE_PACKET = 0x67;
	public const int SHOW_PROFILE_PACKET = 0x68;
	public const int SET_DEFAULT_GAME_TYPE_PACKET = 0x69;
	public const int REMOVE_OBJECTIVE_PACKET = 0x6a;
	public const int SET_DISPLAY_OBJECTIVE_PACKET = 0x6b;
	public const int SET_SCORE_PACKET = 0x6c;
	public const int LAB_TABLE_PACKET = 0x6d;
	public const int UPDATE_BLOCK_SYNCED_PACKET = 0x6e;
	public const int MOVE_ACTOR_DELTA_PACKET = 0x6f;
	public const int SET_SCOREBOARD_IDENTITY_PACKET = 0x70;
	public const int SET_LOCAL_PLAYER_AS_INITIALIZED_PACKET = 0x71;
	public const int UPDATE_SOFT_ENUM_PACKET = 0x72;
	public const int NETWORK_STACK_LATENCY_PACKET = 0x73;

	public const int SCRIPT_CUSTOM_EVENT_PACKET = 0x75;
	public const int SPAWN_PARTICLE_EFFECT_PACKET = 0x76;
	public const int AVAILABLE_ACTOR_IDENTIFIERS_PACKET = 0x77;
	public const int LEVEL_SOUND_EVENT_PACKET_V2 = 0x78;
	public const int NETWORK_CHUNK_PUBLISHER_UPDATE_PACKET = 0x79;
	public const int BIOME_DEFINITION_LIST_PACKET = 0x7a;
	public const int LEVEL_SOUND_EVENT_PACKET = 0x7b;
	public const int LEVEL_EVENT_GENERIC_PACKET = 0x7c;
	public const int LECTERN_UPDATE_PACKET = 0x7d;

	public const int ADD_ENTITY_PACKET = 0x7f;
	public const int REMOVE_ENTITY_PACKET = 0x80;
	public const int CLIENT_CACHE_STATUS_PACKET = 0x81;
	public const int ON_SCREEN_TEXTURE_ANIMATION_PACKET = 0x82;
	public const int MAP_CREATE_LOCKED_COPY_PACKET = 0x83;
	public const int STRUCTURE_TEMPLATE_DATA_REQUEST_PACKET = 0x84;
	public const int STRUCTURE_TEMPLATE_DATA_RESPONSE_PACKET = 0x85;

	public const int CLIENT_CACHE_BLOB_STATUS_PACKET = 0x87;
	public const int CLIENT_CACHE_MISS_RESPONSE_PACKET = 0x88;
	public const int EDUCATION_SETTINGS_PACKET = 0x89;
	public const int EMOTE_PACKET = 0x8a;
	public const int MULTIPLAYER_SETTINGS_PACKET = 0x8b;
	public const int SETTINGS_COMMAND_PACKET = 0x8c;
	public const int ANVIL_DAMAGE_PACKET = 0x8d;
	public const int COMPLETED_USING_ITEM_PACKET = 0x8e;
	public const int NETWORK_SETTINGS_PACKET = 0x8f;
	public const int PLAYER_AUTH_INPUT_PACKET = 0x90;
	public const int CREATIVE_CONTENT_PACKET = 0x91;
	public const int PLAYER_ENCHANT_OPTIONS_PACKET = 0x92;
	public const int ITEM_STACK_REQUEST_PACKET = 0x93;
	public const int ITEM_STACK_RESPONSE_PACKET = 0x94;
	public const int PLAYER_ARMOR_DAMAGE_PACKET = 0x95;
	public const int CODE_BUILDER_PACKET = 0x96;
	public const int UPDATE_PLAYER_GAME_TYPE_PACKET = 0x97;
	public const int EMOTE_LIST_PACKET = 0x98;
	public const int POSITION_TRACKING_D_B_SERVER_BROADCAST_PACKET = 0x99;
	public const int POSITION_TRACKING_D_B_CLIENT_REQUEST_PACKET = 0x9a;
	public const int DEBUG_INFO_PACKET = 0x9b;
	public const int PACKET_VIOLATION_WARNING_PACKET = 0x9c;
	public const int MOTION_PREDICTION_HINTS_PACKET = 0x9d;
	public const int ANIMATE_ENTITY_PACKET = 0x9e;
	public const int CAMERA_SHAKE_PACKET = 0x9f;
	public const int PLAYER_FOG_PACKET = 0xa0;
	public const int CORRECT_PLAYER_MOVE_PREDICTION_PACKET = 0xa1;
	public const int ITEM_COMPONENT_PACKET = 0xa2;
	public const int FILTER_TEXT_PACKET = 0xa3;
	public const int CLIENTBOUND_DEBUG_RENDERER_PACKET = 0xa4;
	public const int SYNC_ACTOR_PROPERTY_PACKET = 0xa5;
	public const int ADD_VOLUME_ENTITY_PACKET = 0xa6;
	public const int REMOVE_VOLUME_ENTITY_PACKET = 0xa7;
	public const int SIMULATION_TYPE_PACKET = 0xa8;
	public const int NPC_DIALOGUE_PACKET = 0xa9;
	public const int EDU_URI_RESOURCE_PACKET = 0xaa;
	public const int CREATE_PHOTO_PACKET = 0xab;
	public const int UPDATE_SUB_CHUNK_BLOCKS_PACKET = 0xac;
	public const int PHOTO_INFO_REQUEST_PACKET = 0xad;
	public const int SUB_CHUNK_PACKET = 0xae;
	public const int SUB_CHUNK_REQUEST_PACKET = 0xaf;
	public const int PLAYER_START_ITEM_COOLDOWN_PACKET = 0xb0;
	public const int SCRIPT_MESSAGE_PACKET = 0xb1;
	public const int CODE_BUILDER_SOURCE_PACKET = 0xb2;
	public const int TICKING_AREAS_LOAD_STATUS_PACKET = 0xb3;
	public const int DIMENSION_DATA_PACKET = 0xb4;
	public const int AGENT_ACTION_EVENT_PACKET = 0xb5;
	public const int CHANGE_MOB_PROPERTY_PACKET = 0xb6;
	
}