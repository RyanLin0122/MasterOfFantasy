using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Concurrent;
namespace PEProtocal
{
    [ProtoContract]
    public class ProtoMsg
    {
        ///0:HeartBeat 心跳
        ///1:LoginReq  登入請求
        ///2:LoginRsp  登入回應
        ///3:ServerChoose 選服
        ///4:ServerResponse 選服回應
        ///5:CreateCharacterReq 創角請求
        ///6:CreateCharacterRsp 創角回應
        ///7:DeleteReq 刪角請求
        ///8:DeleteRsp 刪角回應
        ///9:LogoutReq 登出請求
        ///10:MapInformation 按幀數回傳人物怪物位置
        ///11:EnterGameReq 登入請求
        ///12:EnterGameRsp 登入回應
        ///13:AddMapPlayer增加一名玩家回應
        ///14:MoveMapPlayer移動同步請求
        ///15:ToOtherMapReq去別張地圖請求
        ///16:ToOtherMapRsp去別張地圖回應
        ///17:RemoveMapPlayerRsp移除一名角色回應
        ///18:AddPropertyPoint點屬性點請求
        ///19:AddPropertyPoint點屬性點回應
        ///20:EnterMiniGameReq進入小遊戲請求
        ///21:EnterMiniGameRsp進入小遊戲回應
        ///22:MiniGameScoreReq小遊戲分數請求
        ///23:MiniGameScoreRsp小遊戲分數回應
        ///24:ChatRequest聊天請求
        ///25:ChatResponse聊天回應
        ///26:CalculatorAssign計算機指定
        ///27:CalculatorReady計算機準備
        ///28:CalculatorReport計算機回傳
        ///29:MonsterGenerate生怪
        ///30:MonsterGetHurt怪物受傷
        ///31:MonsterDeath怪物死亡
        ///32:ExpPacket經驗值封包
        ///33:LevelUp升級封包
        ///34:WeatherPacket天氣封包
        ///35:ReduceHpMp扣血扣魔封包
        ///36:PlayerGetHurt玩家受傷
        ///37:PlayerDeath玩家死亡
        ///38:PlayerReliveReq玩家復活請求
        ///39:PlayerReliveRsp玩家復活回應
        ///40:PlayerAction玩家動作
        ///41:KnapsackOperation背包操作
        ///42:EquipmentOperation裝備操作
        ///43:RecycleItems回收道具
        ///44:MiniGameSetting小遊戲設定
        ///45:Rewards獎品
        ///46:CashShopRequest商城請求
        ///47:CashShopResponse商城回應
        ///48:TransactionRequest交易請求
        ///49:TransactionResponse交易回應
        ///50:LockerOperation 倉庫操作
        ///51:MailBoxOperation 信箱操作
        ///52:StrengthenRequest強化請求
        ///53:StrengthenResponse強化回應
        ///54:SkillRequest 放技能請求
        ///55:SkillResponce 放技能回應
        ///56:LearnSkill 學技能
        ///57:HotKey 快捷鍵相關
        ///58:ManufactureReq製作請求
        ///59:ManufactureRsp製作回應
        ///60:SkillHitResponse 技能命中回應
        ///61:RunOperation 跑步
        ///62:DropItemInfo 掉落物品通知
        ///63:PickUpReq撿取請求
        ///64:ConsumableOperation使用消耗類
        ///65:接取任務請求
        ///66:接取任務回應
        ///67:完成任務請求
        ///68:完成任務回應
        ///69:放棄任務請求
        ///70:放棄任務回應
        ///71:伺服器公告
        ///72:換頻道
        ///73:賣東西請求
        ///74:賣東西回應
        ///75:整理東西
        ///76:他人情報
        ///77:坐船
        ///78:搭飛龍
        [ProtoMember(1, IsRequired = false)]
        public int MessageType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string SessionID { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string Account { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int ServerNum { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int ChannelNum { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int CharacterNum { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int GameStatus { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public LoginRequest loginRequest { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public LoginResponse loginResponse { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public AccountInfo accountInfo { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public Player[] players { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public CreateInfo CreateRequest { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public Player CreateResponse { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public ServerChooseRequest serverChooseRequest { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public ServerChooseResponse serverChooseResponse { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public DeleteRequest deleteRequest { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public DeleteRsp deleteRsp { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public LogoutReq logoutReq { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public MapInformation mapInformation { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public EnterGameReq enterGameReq { get; set; }
        [ProtoMember(22, IsRequired = false)]
        public EnterGameRsp enterGameRsp { get; set; }
        [ProtoMember(23, IsRequired = false)]
        public AddMapPlayer addMapPlayer { get; set; }
        [ProtoMember(24, IsRequired = false)]
        public EntitySyncRequest entitySyncReq { get; set; }
        [ProtoMember(25, IsRequired = false)]
        public ToOtherMapReq toOtherMapReq { get; set; }
        [ProtoMember(26, IsRequired = false)]
        public ToOtherMapRsp toOtherMapRsp { get; set; }
        [ProtoMember(27, IsRequired = false)]
        public RemoveMapPlayer removeMapPlayer { get; set; }
        [ProtoMember(28, IsRequired = false)]
        public AddPropertyPoint addPropertyPoint { get; set; }
        [ProtoMember(29, IsRequired = false)]
        public EnterMiniGameReq miniGameReq { get; set; }
        [ProtoMember(30, IsRequired = false)]
        public EnterMiniGameRsp miniGameRsp { get; set; }
        [ProtoMember(31, IsRequired = false)]
        public MiniGameScoreReq miniGameScoreReq { get; set; }
        [ProtoMember(32, IsRequired = false)]
        public MiniGameScoreRsp miniGameScoreRsp { get; set; }

        [ProtoMember(33, IsRequired = false)]
        public ChatRequest chatRequest { get; set; }
        [ProtoMember(34, IsRequired = false)]
        public ChatResponse chatResponse { get; set; }

        [ProtoMember(35, IsRequired = false)]
        public CalculatorAssign calculatorAssign { get; set; }
        [ProtoMember(36, IsRequired = false)]
        public CalculatorReady calculatorReady { get; set; }
        [ProtoMember(37, IsRequired = false)]
        public CalculatorReport calculatorReport { get; set; }

        [ProtoMember(38, IsRequired = false)]
        public MonsterGenerate monsterGenerate { get; set; }
        [ProtoMember(39, IsRequired = false)]
        public MonsterGetHurt monsterGetHurt { get; set; }
        [ProtoMember(40, IsRequired = false)]
        public MonsterDeath monsterDeath { get; set; }
        [ProtoMember(41, IsRequired = false)]
        public ExpPacket expPacket { get; set; }
        [ProtoMember(42, IsRequired = false)]
        public LevelUp levelUp { get; set; }
        [ProtoMember(43, IsRequired = false)]
        public WeatherPacket weatherPacket { get; set; }
        [ProtoMember(44, IsRequired = false)]
        public UpdateHpMp updateHpMp { get; set; }
        [ProtoMember(45, IsRequired = false)]
        public PlayerGetHurt playerGetHurt { get; set; }
        [ProtoMember(46, IsRequired = false)]
        public PlayerDeath playerDeath { get; set; }
        [ProtoMember(47, IsRequired = false)]
        public PlayerReliveReq playerReliveReq { get; set; }
        [ProtoMember(48, IsRequired = false)]
        public PlayerReliveRsp playerReliveRsp { get; set; }
        [ProtoMember(49, IsRequired = false)]
        public PlayerAction playerAction { get; set; }
        [ProtoMember(50, IsRequired = false)]
        public KnapsackOperation knapsackOperation { get; set; }
        [ProtoMember(51, IsRequired = false)]
        public EquipmentOperation equipmentOperation { get; set; }
        [ProtoMember(52, IsRequired = false)]
        public RecycleItems recycleItems { get; set; }
        [ProtoMember(53, IsRequired = false)]
        public MiniGameSetting miniGameSetting { get; set; }
        [ProtoMember(54, IsRequired = false)]
        public Rewards rewards { get; set; }
        [ProtoMember(55, IsRequired = false)]
        public CashShopRequest cashShopRequest { get; set; }
        [ProtoMember(56, IsRequired = false)]
        public CashShopResponse cashShopResponse { get; set; }
        [ProtoMember(57, IsRequired = false)]
        public TransactionRequest transactionRequest { get; set; }
        [ProtoMember(58, IsRequired = false)]
        public TransactionResponse transactionResponse { get; set; }
        [ProtoMember(60, IsRequired = false)]
        public LockerOperation lockerOperation { get; set; }
        [ProtoMember(61, IsRequired = false)]
        public MailBoxOperation mailBoxOperation { get; set; }
        [ProtoMember(62, IsRequired = false)]
        public StrengthenRequest strengthenRequest { get; set; }
        [ProtoMember(63, IsRequired = false)]
        public StrengthenResponse strengthenResponse { get; set; }
        [ProtoMember(64, IsRequired = false)]
        public LearnSkill learnSkill { get; set; }
        [ProtoMember(65, IsRequired = false)]
        public HotKeyOperation hotKeyOperation { get; set; }
        [ProtoMember(66, IsRequired = false)]
        public SkillCastRequest skillCastRequest { get; set; }
        [ProtoMember(67, IsRequired = false)]
        public SkillCastResponse skillCastResponse { get; set; }
        [ProtoMember(68, IsRequired = false)]
        public SkillHitResponse skillHitResponse { get; set; }
        [ProtoMember(69, IsRequired = false)]
        public BuffResponse buffResponse { get; set; }
        [ProtoMember(70, IsRequired = false)]
        public ManufactureRequest manufactureRequest { get; set; }
        [ProtoMember(71, IsRequired = false)]
        public ManufactureResponse manufactureResponse { get; set; }
        [ProtoMember(72, IsRequired = false)]
        public RunOperation runOperation { get; set; }
        [ProtoMember(73, IsRequired = false)]
        public DropItemsInfo dropItemsInfo { get; set; }
        [ProtoMember(74, IsRequired = false)]
        public PickUpRequest pickUpRequest { get; set; }
        [ProtoMember(75, IsRequired = false)]
        public PickUpResponse pickUpResponse { get; set; }
        [ProtoMember(76, IsRequired = false)]
        public ConsumableOperation consumableOperation { get; set; }
        [ProtoMember(77, IsRequired = false)]
        public QuestAcceptRequest questAcceptRequest { get; set; }
        [ProtoMember(78, IsRequired = false)]
        public QuestAcceptResponse questAcceptResponse { get; set; }
        [ProtoMember(79, IsRequired = false)]
        public QuestSubmitRequest questSubmitRequest { get; set; }
        [ProtoMember(80, IsRequired = false)]
        public QuestSubmitResponse questSubmitResponse { get; set; }
        [ProtoMember(81, IsRequired = false)]
        public QuestAbandonRequest questAbandonRequest { get; set; }
        [ProtoMember(82, IsRequired = false)]
        public QuestAbandonResponse questAbandonResponse { get; set; }
        [ProtoMember(83, IsRequired = false)]
        public ServerAnnouncement serverAnnouncement { get; set; }
        [ProtoMember(84, IsRequired = false)]
        public ChangeChannelOperation changeChannelOperation { get; set; }
        [ProtoMember(85, IsRequired = false)]
        public SellItemReq sellItemReq { get; set; }
        [ProtoMember(86, IsRequired = false)]
        public SellItemRsp sellItemRsp { get; set; }
        [ProtoMember(87, IsRequired = false)]
        public TidyUpOperation tidyUpOperation { get; set; }
        [ProtoMember(88, IsRequired = false)]
        public OtherProfileOperation otherProfileOperation { get; set; }
        [ProtoMember(89, IsRequired = false)]
        public ShipOperation shipOperation { get; set; }

        [ProtoMember(90, IsRequired = false)]
        public DragonTaxiOperation dragonTaxiOperation { get; set; }
        //Serialize
        public void SerializeToStream<T>(T data, Stream stream)
        {
            Serializer.Serialize(stream, data);
        }
        public byte[] SerializeToBytes<T>(T data, string Key)
        {
            byte[] result;
            using (var stream = new MemoryStream())
            {
                this.SerializeToStream(data, stream);
                result = stream.ToArray();
                result = AES.AESEncrypt(result, Key);
                stream.Dispose();
            }

            return result;
        }
        public byte[] SerializeToBytes<T>(T data)
        {
            byte[] result;
            using (var stream = new MemoryStream(1000))
            {
                this.SerializeToStream(data, stream);
                result = stream.ToArray();
                stream.Dispose();
            }
            return result;
        }
        //Deserialize 
        public static ProtoMsg ProtoDeserialize(byte[] data, string Key)
        {
            if (null == data) return null;
            byte[] NewBytes = AES.AESDecrypt(data, Key);
            using (var stream = new MemoryStream(NewBytes, 0, NewBytes.Length))
            {
                ProtoMsg result = Serializer.Deserialize<ProtoMsg>(stream);

                return result;
            }

        }
    }
    [ProtoContract]
    public class ServerChooseRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public int ChoosedServer { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int ChoosedChannel { get; set; }
    }

    [ProtoContract]
    public class ServerChooseResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public bool Result;
    }

    [ProtoContract]
    public class AccountInfo
    {
        [ProtoMember(1, IsRequired = false)]
        public int GMLevel { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Cash { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string LastLoginTime { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public string LastLogoutTime { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public MOFOption AccountMOFOption { get; set; }
    }


    [ProtoContract]
    public class LoginRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public string Account { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string Password { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string MAC { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public string IPAddress { get; set; }
    }
    [ProtoContract]
    public class LoginResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int ErrorType { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string ErrorCode { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int[] CharacterNums { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public GameServerStatus ServerStatus { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public string PrivateKey { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public AccountData accountData { get; set; }
    }


    [ProtoContract]
    public class GameServerStatus
    {
        [ProtoMember(1, IsRequired = false)]
        public int[] ServerStatus { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int[] ChannelNums { get; set; }
    }
    [ProtoContract]
    public class DeleteRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
    }
    [ProtoContract]
    public class DeleteRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public bool Result;
        [ProtoMember(2, IsRequired = false)]
        public string Message;
    }
    [ProtoContract]
    public class LogoutReq
    {
        [ProtoMember(1, IsRequired = false)]
        public string Account;
        [ProtoMember(2, IsRequired = false)]
        public string SessionID;
        [ProtoMember(3, IsRequired = false)]
        public string ActiveCharacterName;
        [ProtoMember(4, IsRequired = false)]
        public string LogoutTime;
    }

    [ProtoContract]
    public class PlayerEquipments
    {
        [ProtoMember(1, IsRequired = false)]
        public Equipment B_Head { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Equipment B_Ring1 { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public Equipment B_Neck { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public Equipment B_Ring2 { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public Weapon B_Weapon { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public Equipment B_Chest { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public Equipment B_Glove { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public Equipment B_Shield { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public Equipment B_Pants { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public Equipment B_Shoes { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public Equipment F_Hairacc { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public Equipment F_NameBox { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public Equipment F_ChatBox { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public Equipment F_FaceAcc { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public Equipment F_Glasses { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public Equipment F_HairStyle { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public Equipment F_Chest { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public Equipment F_Glove { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public Equipment F_Cape { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public Equipment F_Pants { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public Equipment F_Shoes { get; set; }
        [ProtoMember(22, IsRequired = false)]
        public Equipment F_FaceType { get; set; }
        [ProtoMember(23, IsRequired = false)]
        public Equipment Badge { get; set; }
    }
    [ProtoContract]
    public class CreateInfo
    {
        [ProtoMember(1, IsRequired = false)]
        public string name { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int gender { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int job { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int att { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int strength { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int agility { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int intellect { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int Fashionhairstyle { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int Fashionchest { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int Fashionpant { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public int Fashionshoes { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public int MaxHp { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public int MaxMp { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public int Server { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public string CreateTime { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public string LastLoginTime { get; set; }

    }

    //回傳的地圖資料
    [ProtoContract]
    public class MapInformation
    {
        [ProtoMember(1, IsRequired = false)]
        public long TimeStamp { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long Time { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string CalculaterName { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public Dictionary<string, float[]> CharactersPosition { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public Dictionary<string, int> CharactersMoveState { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public Dictionary<string, bool> CharactersIsRun { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public Dictionary<int, float[]> MonstersPosition { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public Dictionary<int, float[]> MonstersStates { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int MapID { get; set; }
    }

    //進入遊戲請求
    [ProtoContract]
    public class EnterGameReq
    {
        [ProtoMember(1, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public float[] Position { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public bool Isnew { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public bool Istrain { get; set; }

    }

    //進入遊戲回應
    [ProtoContract]
    public class EnterGameRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public List<TrimedPlayer> MapPlayers { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public bool IsCalculater { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public float[] Position { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public WeatherType weather { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public Dictionary<int, SerializedMonster> Monsters { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public List<NEntity> MapPlayerEntities { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public ConcurrentDictionary<int, DropItem> DropItems { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public string ServerAnnouncement { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public float AnnouncementValidTime { get; set; }
    };

    //地圖增加一名玩家通知
    [ProtoContract]
    public class AddMapPlayer
    {
        [ProtoMember(1, IsRequired = false)]
        public TrimedPlayer NewPlayer { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public NEntity nEntity { get; set; }
    }

    //移動同步
    [ProtoContract]
    public class EntitySyncRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public List<EntityEvent> entityEvent { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public List<NEntity> nEntity { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int MapID { get; set; }
    }

    //去別張地圖請求
    [ProtoContract]
    public class ToOtherMapReq
    {
        [ProtoMember(1, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public float[] Position { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int LastMapID { get; set; }
    }

    //去別張地圖回應
    [ProtoContract]
    public class ToOtherMapRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public List<TrimedPlayer> MapPlayers { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public bool IsCalculater { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public float[] Position { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public WeatherType weather { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public Dictionary<int, SerializedMonster> Monsters { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public List<NEntity> MapPlayerEntities { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public ConcurrentDictionary<int, DropItem> DropItems { get; set; }
    };

    //地圖移除一名玩家通知
    [ProtoContract]
    public class RemoveMapPlayer
    {
        [ProtoMember(1, IsRequired = false)]
        public string Name { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MapID { get; set; }
    }

    //點屬性點
    [ProtoContract]
    public class AddPropertyPoint
    {
        [ProtoMember(1, IsRequired = false)]
        public int Att { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Strength { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Agility { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Intellect { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int RestPoint { get; set; }
    }

    //進入小遊戲請求
    [ProtoContract]
    public class EnterMiniGameReq
    {
        [ProtoMember(1, IsRequired = false)]
        public int MiniGameID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int LastMapID { get; set; }
    }

    //進入小遊戲回應
    [ProtoContract]
    public class EnterMiniGameRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public Dictionary<string, int> MiniGameRanking { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MiniGameID { get; set; }
    }

    //小遊戲分數請求
    [ProtoContract]
    public class MiniGameScoreReq
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Score { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int MiniGameID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int SwordPoint { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int ArcheryPoint { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int MagicPoint { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int TheologyPoint { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public bool IsRecycle { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int ItemID { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public bool IsSuccess { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public int Difficulty { get; set; }
    }

    //小遊戲分數回應
    [ProtoContract]
    public class MiniGameScoreRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Score { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int MiniGameID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int SwordPoint { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int ArcheryPoint { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int MagicPoint { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int TheologyPoint { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public Dictionary<string, int> MiniGameRanking { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int DeleteItemPos { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public Item RecycleItem { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public bool DeleteItemIsCash { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public string ErrorMsg { get; set; }
    }

    //聊天請求
    [ProtoContract]
    public class ChatRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MessageType { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string Contents { get; set; }

    }

    //聊天回應
    [ProtoContract]
    public class ChatResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MessageType { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string Contents { get; set; }
    }

    //計算機指定
    [ProtoContract]
    public class CalculatorAssign
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
    }

    //計算機準備
    [ProtoContract]
    public class CalculatorReady
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
    }

    //計算機回傳
    [ProtoContract]
    public class CalculatorReport
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Dictionary<int, float[]> MonsterPos { get; set; }
    }

    //生怪通知
    [ProtoContract]
    public class MonsterGenerate
    {
        [ProtoMember(1, IsRequired = false)]
        public Dictionary<int, float[]> MonsterPos { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Dictionary<int, int> MonsterId { get; set; }
    }

    //怪物受傷
    [ProtoContract]
    public class MonsterGetHurt
    {
        [ProtoMember(1, IsRequired = false)]
        public Dictionary<int, int> MonsterMapID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Dictionary<int, bool> IsCritical { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public Dictionary<int, int[]> Damage { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public Dictionary<int, AttackType> AttackTypes { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int AnimID { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public bool FaceDir { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public float Delay { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public Dictionary<int, bool> IsDeath { get; set; }
    }

    //怪物死亡
    [ProtoContract]
    public class MonsterDeath
    {
        [ProtoMember(1, IsRequired = false)]
        public List<MonsterDeathInfo> DeathInfos { get; set; }
    }

    [ProtoContract(EnumPassthru = false)]
    public enum AttackType
    {
        [ProtoEnum]
        Common,
        [ProtoEnum]
        Burn,
        [ProtoEnum]
        Shock,
        [ProtoEnum]
        Freeze,
        [ProtoEnum]
        Faint
    }
    [ProtoContract(EnumPassthru = false)]
    public enum WeatherType
    {
        [ProtoEnum]
        Normal,
        [ProtoEnum]
        Snow,
        [ProtoEnum]
        LittleRain,
        [ProtoEnum]
        MiddleRain,
        [ProtoEnum]
        StrongRain
    }

    [ProtoContract]
    public class ExpPacket
    {
        [ProtoMember(1, IsRequired = false)]
        public Dictionary<string, int> Exp { get; set; }
    }

    [ProtoContract]
    public class LevelUp
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Level { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public long RestExp { get; set; }
    }

    [ProtoContract]
    public class WeatherPacket
    {
        [ProtoMember(1, IsRequired = false)]
        public WeatherType weatherType { get; set; }
    }

    [ProtoContract]
    public class UpdateHpMp
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int UpdateHp { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int UpdateMp { get; set; }
    }


    [ProtoContract(EnumPassthru = false)]
    public enum HurtType
    {
        [ProtoEnum]
        Normal,
        [ProtoEnum]
        Faint,

    }
    [ProtoContract]
    public class PlayerGetHurt
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int damage { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public HurtType hurtType { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int MonsterID { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public bool FaceDir { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public bool IsDeath { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public bool IsFaint { get; set; }
    }

    [ProtoContract]
    public class PlayerDeath
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long RestExp { get; set; }
    }

    [ProtoContract]
    public class PlayerReliveReq
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int ReliveMethod { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int TownID { get; set; }
    }

    [ProtoContract]
    public class PlayerReliveRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int ReliveMethod { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int UpdateHp { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int UpdateMp { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int TownID { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int UpdateCash { get; set; }
    }

    [ProtoContract(EnumPassthru = false)]
    public enum MonsterStatus
    {
        [ProtoEnum]
        Normal,
        [ProtoEnum]
        Frozen,
        [ProtoEnum]
        Death,
        [ProtoEnum]
        Faint,
        [ProtoEnum]
        Moving
    }

    [ProtoContract(EnumPassthru = false)]
    public enum EntityStatus
    {
        [ProtoEnum]
        Idle,
        [ProtoEnum]
        InBattle
    }

    [ProtoContract]
    public class SerializedMonster
    {
        [ProtoMember(1, IsRequired = false)]
        public int MonsterID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public float[] Position { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public MonsterStatus status { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public string Targets { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int HP { get; set; }
    }

    [ProtoContract]
    public class PlayerAction
    {
        [ProtoMember(1, IsRequired = false)]
        public int PlayerActionType { get; set; } //1:動畫機 2:技能 3:表情 4:特效 5:坐騎
        [ProtoMember(2, IsRequired = false)]
        public int AnimationType { get; set; } //1:Hurt 2:Death
        [ProtoMember(3, IsRequired = false)]
        public bool FaceDir { get; set; }
    }

    [ProtoContract]
    public class KnapsackOperation
    {
        //第一種: 存入一個道具
        //第二種: 增加數量
        //第三種: 增加不定數量道具
        //第四種: 交換
        //第五種: 刪除
        //第六種: 買東西
        //第七種: 賣東西
        //第八種: 整理背包
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long Ribi { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public List<Item> items { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int[] OldPosition { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int[] NewPosition { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int ErrorType { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public string ErrorMessage { get; set; }
    }

    [ProtoContract]
    public class EquipmentOperation
    {
        //第一種:穿裝備進空格
        //第二種:換裝備
        //第三種:脫下裝備進背包空格
        [ProtoMember(1, IsRequired = false)]
        public Item PutOnEquipment { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Item PutOffEquipment { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int KnapsackPosition;
        [ProtoMember(4, IsRequired = false)]
        public int EquipmentPosition;
        [ProtoMember(5, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public string PlayerName { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public PlayerEquipments OtherPlayerEquipments { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int OtherGender { get; set; }
    }

    [ProtoContract]
    public class RecycleItems //用法: 1.client請求，server驗證刪掉，回傳client刪掉 2.server刪掉，回傳client刪掉
    {
        [ProtoMember(1, IsRequired = false)]
        public int InventoryType { get; set; } //0:Knapsack 1:Locker
        [ProtoMember(2, IsRequired = false)]
        public List<int> Positions { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public List<int> Amount { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public List<int> ItemID { get; set; }
    }
    [ProtoContract]
    public class MiniGameSetting
    {
        [ProtoMember(1, IsRequired = false)]
        public int[] MiniGameArray { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MiniGameRatio { get; set; }
    }

    [ProtoContract]
    public class MOFOption
    {
        [ProtoMember(1, IsRequired = false)]
        public bool IsBGMMute { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public bool IsEmbiMute { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool IsUIMute { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public bool IsEffectMute { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public bool IsMonsterMute { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public float BGMVolume { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public float EmbiVolume { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public float UIVolume { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public float EffectVolume { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public float MonsterVolume { get; set; }
        [ProtoMember(11, IsRequired = false)]

        public bool IsParty { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public bool IsTransaction { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public bool IsGuild { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public bool IsFriend { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public bool IsPrivateMsg { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public bool IsAlumni { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public bool IsPVP { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public bool IsBroadcast { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public bool IsForiegn { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public bool IsMiniGame { get; set; }
        [ProtoMember(21, IsRequired = false)]


        public int ImageQuality { get; set; }
        [ProtoMember(22, IsRequired = false)]

        public bool IsAntiAliasing { get; set; }
        [ProtoMember(23, IsRequired = false)]
        public bool IsVsync { get; set; }
        [ProtoMember(24, IsRequired = false)]
        public int Language { get; set; }
    }

    [ProtoContract]
    public class DiaryInformation
    {
        [ProtoMember(1, IsRequired = false)]
        public Dictionary<int, int> NPC_Info { get; set; } //NPCID，解鎖程度
        [ProtoMember(2, IsRequired = false)]
        public Dictionary<int, int> Monster_Info { get; set; } //怪物ID，擊殺數
    }

    [ProtoContract(EnumPassthru = false)]
    public enum Quest_List_Type
    {
        [ProtoEnum]
        All = 0,
        [ProtoEnum]
        In_Progress = 1,
        [ProtoEnum]
        Finished = 2
    }

    [ProtoContract(EnumPassthru = false)]
    public enum QuestType
    {
        [ProtoEnum]
        Daily,
        [ProtoEnum]
        Weekly,
        [ProtoEnum]
        Monthly,
        [ProtoEnum]
        OneTime,
        [ProtoEnum]
        LimitTime
    }

    [ProtoContract(EnumPassthru = false)]
    public enum NPCQuestStatus
    {
        [ProtoEnum]
        None,      //無任務
        [ProtoEnum]
        DeliveryTarget, //可配送目標
        [ProtoEnum]
        Complete,  //擁有已完成任務
        [ProtoEnum]
        Available, //擁有可接受任務
        [ProtoEnum]
        InComplete,//擁有未完成任務
    }
    [ProtoContract(EnumPassthru = false)]
    public enum QuestStatus
    {
        [ProtoEnum]
        InProgress,
        [ProtoEnum]
        Completed,
        [ProtoEnum]
        Finished,
        [ProtoEnum]
        Failed
    }

    [ProtoContract(EnumPassthru = false)]
    public enum QuestTarget
    {
        [ProtoEnum]
        None,
        [ProtoEnum]
        Kill,
        [ProtoEnum]
        Item,
        [ProtoEnum]
        Delivery
    }
    [ProtoContract]
    public class NQuest
    {
        [ProtoMember(1, IsRequired = false)]
        public int quest_id { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public QuestStatus status { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public Dictionary<int, int> Targets { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public bool HasDeliveried { get; set; }
    }
    [ProtoContract]
    public class QuestDefine
    {
        public int ID { get; set; }
        public string QuestName { get; set; }
        public int LimitLevel { get; set; }
        public int LimitJob { get; set; }
        public int PreQuest { get; set; }
        public int PostQuest { get; set; }
        public QuestType Type { get; set; }
        public int AcceptNPC { get; set; }
        public int SubmitNPC { get; set; }
        public int DeliveryNPC { get; set; }
        public QuestTarget Target { get; set; }
        public List<int> TargetIDs { get; set; }
        public List<int> TargetNum { get; set; }
        public string Overview { get; set; }
        public List<string> DialogDelivery { get; set; }
        public List<string> DialogAccept { get; set; }
        public List<string> DialogDeny { get; set; }
        public List<string> DialogInComplete { get; set; }
        public List<string> DialogFinish { get; set; }
        public long RewardRibi { get; set; }
        public long RewardExp { get; set; }
        public List<int> RewardItemIDs { get; set; }
        public List<int> RewardItemsCount { get; set; }
        public int RewardHonerPoint { get; set; }
        public int RewardBadge { get; set; }
        public int RewardTitle { get; set; }
        public float LimitTime { get; set; }

    }
    [ProtoContract]
    public class QuestListRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public Quest_List_Type listType { get; set; }
    }

    [ProtoContract]
    public class QuestListResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string ErrorMsg { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public List<NQuest> quests { get; set; }
    }

    [ProtoContract]
    public class QuestAcceptRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public int quest_id { get; set; }
    }

    [ProtoContract]
    public class QuestAcceptResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string ErrorMsg { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public NQuest quest { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public Item DeliveryItem { get; set; }
    }

    [ProtoContract]
    public class QuestSubmitRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public int quest_id { get; set; }
    }

    [ProtoContract]
    public class QuestSubmitResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string ErrorMsg { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public NQuest quest { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public long RewardRibi { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public List<Item> RewardItems { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public long RewardExp { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int RewardHonerPoint { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int RewardBadge { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int RewardTitle { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public List<Item> RecycleItems { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public List<int> DeletePositions { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public List<bool> DeleteIsCashs { get; set; }
    }

    [ProtoContract]
    public class QuestAbandonRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public int quest_id { get; set; }
    }

    [ProtoContract]
    public class QuestAbandonResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string ErrorMsg { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public NQuest quest { get; set; }
    }

    [ProtoContract]
    public class Rewards
    {
        [ProtoMember(1, IsRequired = false)]
        public long Exp { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long Ribi { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Cash { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Title { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public Dictionary<int, Item> KnapsackItems_NotCash { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int Honor { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int SwordPoint { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int ArcheryPoint { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int MagicPoint { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int TheologyPoint { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public string Character { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public Dictionary<int, Item> KnapsackItems_Cash { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public Dictionary<int, Item> MailBoxItems { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public long MailBoxRibi { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public string ErrorMsg { get; set; }
    }

    [ProtoContract]
    public class CashShopRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public long Cash { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long TotalPrice { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public List<string> Cata { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public List<string> Tag { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public List<int> Orders { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public List<int> Amount { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int OperationType { get; set; } //1. 買東西 2. 確認點數 3. 送禮給別人 4. 放進背包 5. 增加物品到購物車 6.購物車購買 7.刪除購物車
        [ProtoMember(8, IsRequired = false)]
        public string GiftPlayerName { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public List<int> Quantity { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public List<int> Positions { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public bool IsFashionPanel { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public List<CartItem> CartItems { get; set; }
    }

    [ProtoContract]
    public class CashShopResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public long Cash { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long TotalPrice { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public Dictionary<int, Item> FashionItems { get; set; } // position, Item
        [ProtoMember(4, IsRequired = false)]
        public int ErrorLogType { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int OperationType { get; set; } //1. 買東西 2. 確認點數 3. 送禮給別人 4. 放東西進背包或信箱
        [ProtoMember(6, IsRequired = false)]
        public string GiftPlayerName { get; set; }
        [ProtoMember(7, IsRequired = false)] //
        public bool IsSuccess { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public Dictionary<int, Item> OtherItems { get; set; } // position, Item
        [ProtoMember(9, IsRequired = false)] //
        public List<int> ProcessPositions { get; set; }
        [ProtoMember(10, IsRequired = false)] //
        public bool IsFull { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public Dictionary<int, Item> CashKnapsack { get; set; } // position, Item
        [ProtoMember(12, IsRequired = false)]
        public Dictionary<int, Item> NonCashKnapsack { get; set; } // position, Item
        [ProtoMember(13, IsRequired = false)]
        public Dictionary<int, Item> MailBox { get; set; } // position, Item

        [ProtoMember(14, IsRequired = false)]
        public bool IsFashion { get; set; } // position, Item
        [ProtoMember(15, IsRequired = false)]
        public List<CartItem> CartItems { get; set; }
    }

    [ProtoContract]
    public class TransactionRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public string PlayerName { get; set; }

        [ProtoMember(2, IsRequired = false)]
        public string OtherPlayerName { get; set; }

        [ProtoMember(3, IsRequired = false)]
        public int OperationType { get; set; }

        [ProtoMember(4, IsRequired = false)]
        public long PutRubi { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public Dictionary<int, Item> PlayerItems { get; set; } // position, Item
        [ProtoMember(6, IsRequired = false)]
        public long OtherRubi { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public Dictionary<int, Item> OtherItems { get; set; } // position, Item

        [ProtoMember(8, IsRequired = false)]
        public int TransactionPos { get; set; }

        [ProtoMember(9, IsRequired = false)]
        public int KnapsackPos { get; set; }

        [ProtoMember(10, IsRequired = false)]
        public Item item { get; set; }



    }
    [ProtoContract]
    public class TransactionResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public string PlayerName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string OtherPlayerName { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool IsSuccess { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int ErrorLogType { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public long PutRubi { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public Dictionary<int, Item> PlayerItems { get; set; } // position, Item
        [ProtoMember(8, IsRequired = false)]
        public long OtherRubi { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public Dictionary<int, Item> OtherItems { get; set; } // position, Item
        [ProtoMember(10, IsRequired = false)]
        public int TransactionPos { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public Item item { get; set; }

    }
    [ProtoContract]
    public class LockerOperation
    {
        //第一種: 倉庫內操作
        //第二種: 放東西到倉庫空格
        //第三種: 放東西到倉庫非空格
        //第四種: 從倉庫放東西到背包空格
        //第五種: 從倉庫放東西到背包非空格
        //第六種: 放錢到倉庫
        //第七種: 從倉庫拿錢
        //第八種: 整理背包
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long Ribi { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public List<Item> items { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int[] OldPosition { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int[] NewPosition { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int ErrorType { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public string ErrorMessage { get; set; }
    }
    [ProtoContract]
    public class MailBoxOperation
    {
        //第一種: 信箱內操作
        //第二種: 從倉庫放東西到背包空格
        //第三種: 從倉庫放東西到背包非空格
        //第四種: 從倉庫拿錢
        //第五種: 整理信箱
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long Ribi { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public List<Item> items { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int[] OldPosition { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int[] NewPosition { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int ErrorType { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public string ErrorMessage { get; set; }
    }

    [ProtoContract]
    public class StrengthenRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Item item { get; set; }



    }
    [ProtoContract]
    public class StrengthenResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string Effect { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public Item strengthenItem { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public Item Stone { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public Item item { get; set; }

        [ProtoMember(6, IsRequired = false)]
        public long Ribi { get; set; }
    }
    [ProtoContract]
    public class LearnSkill
    {
        [ProtoMember(1, IsRequired = false)]
        public int SkillID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Level { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool IsSuccess { get; set; }
    }
    [ProtoContract]
    public class HotKeyOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; } //1.註冊快捷鍵 2.刪除快捷鍵 3.取代
        [ProtoMember(2, IsRequired = false)]
        public HotkeyData OldHotKeyData { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public HotkeyData NewHotKeyData { get; set; }
    }

    [ProtoContract]
    public class SkillCastRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public SkillCastInfo CastInfo { get; set; }
    }

    [ProtoContract]
    public class SkillCastResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public List<SkillCastInfo> CastInfos { get; set; }
    }

    [ProtoContract]
    public class SkillHitResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public List<SkillHitInfo> skillHits { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public SkillResult Result { get; set; }
    }

    [ProtoContract]
    public class BuffResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public SkillResult SkillResult { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public List<BuffInfo> Buffs { get; set; }
    }

    [ProtoContract]
    public class ManufactureRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int FormulaId { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Amount { get; set; }
    }

    [ProtoContract]
    public class ManufactureResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public int OperationType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Dictionary<int, int> RemoveAndConSume { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public Dictionary<int, Item> GetItem { get; set; }
    }

    [ProtoContract]
    public class RunOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public bool IsRun { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string CharacterName { get; set; }
    }
    [ProtoContract]
    public class DropItemsInfo
    {
        [ProtoMember(1, IsRequired = false)]
        public Dictionary<int, DropItem> DropItems { get; set; }
    }
    [ProtoContract]
    public class PickUpRequest
    {
        [ProtoMember(1, IsRequired = false)]
        public int ItemUUID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int InventoryID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int InventoryPosition { get; set; }

    }
    [ProtoContract]
    public class PickUpResponse
    {
        [ProtoMember(1, IsRequired = false)]
        public List<int> ItemUUIDs { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public List<string> CharacterNames { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public List<bool> Results { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public List<int> InventoryID { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public List<int> InventoryPosition { get; set; }
    }

    [ProtoContract]
    public class ConsumableOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int InventoryID { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int InventoryPosition { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public bool IsSuccess { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int RestNum { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int HP { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int MP { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public Item item { get; set; }
    }

    [ProtoContract]
    public class TidyUpOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public int InventoryID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Dictionary<int, Item> Inventory { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool Result { get; set; }
    }

    [ProtoContract]
    public class ServerAnnouncement
    {
        [ProtoMember(1, IsRequired = false)]
        public string Announcement { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public float ValidTime { get; set; }
    }

    [ProtoContract]
    public class ChangeChannelOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public int Channel { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public bool Result { get; set; }
    }

    [ProtoContract]
    public class SellItemReq
    {
        [ProtoMember(1, IsRequired = false)]
        public bool IsCash { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int ItemID { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Count { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Position { get; set; }
    }

    [ProtoContract]
    public class SellItemRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public int DeleteItemPos { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public Item OverrideItem { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public long CurrentRibi { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public bool DeleteIsCash { get; set; }
    }

    [ProtoContract]
    public class OtherProfileOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public string Name { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Job { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int CurrentBadge { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Level { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int Grade { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public string Title { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public string Guild { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int BattleWinTimes { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int BattleLoseTimes { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int PVPRank { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public int PVPWinTimes { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public int PVPLoseTimes { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public int PVPPoints { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public PlayerEquipments PlayerEquipments { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public int Gender { get; set; }
    }

    [ProtoContract]
    public class ShipOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public ShipDestination Destination { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public long UpdateRibi { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public string ErrorMsg { get; set; }
    }
    
    //飛龍計程車
    [ProtoContract]
    public class DragonTaxiOperation
    {
        [ProtoMember(1, IsRequired = false)]
        public DragonTaxiDestination Destination { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public bool Result { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public long UpdateRibi { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public string ErrorMsg { get; set; }
    }
}
