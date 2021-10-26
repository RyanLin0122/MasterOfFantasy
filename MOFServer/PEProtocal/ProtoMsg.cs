using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
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
        ///14:MoveMapPlayer移動角色請求
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
        public MoveMapPlayer moveMapPlayer { get; set; }
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
    public class AccountData
    {
        [ProtoMember(1, IsRequired = false)]
        public string Account { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string Password { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public long Cash { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public Dictionary<int, Item> LockerServer1 { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public Dictionary<int, Item> LockerServer2 { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public Dictionary<int, Item> LockerServer3 { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public Dictionary<int, Item> CashShopBuyPanelFashionServer1 { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public Dictionary<int, Item> CashShopBuyPanelOtherServer1 { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public Dictionary<int, Item> CashShopBuyPanelFashionServer2 { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public Dictionary<int, Item> CashShopBuyPanelOtherServer2 { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public Dictionary<int, Item> CashShopBuyPanelFashionServer3 { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public Dictionary<int, Item> CashShopBuyPanelOtherServer3 { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public Dictionary<int, string> Friends { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public Dictionary<int, string> BlockedPeople { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public int GMLevel { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public string LastLoginTime { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public string LastLogoutTime { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public long LockerServer1Ribi { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public long LockerServer2Ribi { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public long LockerServer3Ribi { get; set; }

        public Dictionary<int, Item> GetNewBuyPanelFashionS1()
        {
            CashShopBuyPanelFashionServer1 = new Dictionary<int, Item>();
            return CashShopBuyPanelFashionServer1;
        }
        public Dictionary<int, Item> GetNewBuyPanelFashionS2()
        {
            CashShopBuyPanelFashionServer2 = new Dictionary<int, Item>();
            return CashShopBuyPanelFashionServer2;
        }
        public Dictionary<int, Item> GetNewBuyPanelFashionS3()
        {
            CashShopBuyPanelFashionServer3 = new Dictionary<int, Item>();
            return CashShopBuyPanelFashionServer3;
        }

        public Dictionary<int, Item> GetNewBuyPanelOtherS1()
        {
            CashShopBuyPanelOtherServer1 = new Dictionary<int, Item>();
            return CashShopBuyPanelOtherServer1;
        }
        public Dictionary<int, Item> GetNewBuyPanelOtherS2()
        {
            CashShopBuyPanelOtherServer2 = new Dictionary<int, Item>();
            return CashShopBuyPanelOtherServer2;
        }
        public Dictionary<int, Item> GetNewBuyPanelOtherS3()
        {
            CashShopBuyPanelOtherServer3 = new Dictionary<int, Item>();
            return CashShopBuyPanelOtherServer3;
        }
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
    };

    //地圖增加一名玩家通知
    [ProtoContract]
    public class AddMapPlayer
    {
        [ProtoMember(1, IsRequired = false)]
        public TrimedPlayer NewPlayer { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MapID { get; set; }
    }

    //移動玩家
    [ProtoContract]
    public class MoveMapPlayer
    {
        [ProtoMember(1, IsRequired = false)]
        public string PlayerName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public float[] Destination { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int MoveState { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public bool IsRun { get; set; }
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
        public List<int> MonsterID { get; set; }
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
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public long Exp { get; set; }
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
        public bool FaceDir { get; set; }
    }

    [ProtoContract]
    public class PlayerReliveReq
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string IsSamePlace { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int TownID { get; set; }
    }

    [ProtoContract]
    public class PlayerReliveRsp
    {
        [ProtoMember(1, IsRequired = false)]
        public string CharacterName { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string IsSamePlace { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int UpdateHp { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int UpdateMp { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int UpdateExp { get; set; }
    }

    [ProtoContract(EnumPassthru = false)]
    public enum MonsterStatus
    {
        [ProtoEnum]
        Idle,
        [ProtoEnum]
        Follow,
        [ProtoEnum]
        Attack,
        [ProtoEnum]
        Frozen,
        [ProtoEnum]
        Death,
        [ProtoEnum]
        Faint,
        [ProtoEnum]
        Hurt,
        [ProtoEnum]
        Shocked,
        [ProtoEnum]
        Burned,
        [ProtoEnum]
        Stop
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
    public enum QuestState
    {
        [ProtoEnum]
        Unacceptable,
        [ProtoEnum]
        Acceptable,
        [ProtoEnum]
        Processing,
        [ProtoEnum]
        Complete,
        [ProtoEnum]
        Finished
    }
    [ProtoContract]
    public class Quest
    {
        #region MyRegion
        [ProtoMember(1, IsRequired = false)]
        public QuestType questType { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public DateTime StartDate { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public QuestState questState { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int QuestID { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public Dictionary<int, int> HasKilledMonsters { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public Dictionary<int, int> HasCollectItems { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public DateTime FinishedDate { get; set; }

        #endregion

        [ProtoMember(5, IsRequired = false)]
        public TimeSpan RestTime { get; set; }

        [ProtoMember(9, IsRequired = false)]
        public TimeSpan RestAcceptableTime { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int RestTimes { get; set; }
    }
    [ProtoContract]
    public class QuestInfo
    {
        #region MyRegion
        [ProtoMember(1, IsRequired = false)]
        public Quest questTemplate { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int StartNPCID { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int EndNPCID { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public Dictionary<int, int> RequiredMonsters { get; set; }

        [ProtoMember(6, IsRequired = false)]
        public Dictionary<int, int> RequiredItems { get; set; }

        [ProtoMember(7, IsRequired = false)]
        public int RequiredLevel { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public List<int> RequiredQuests { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int RequiredJob { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public List<int> OtherAcceptCondition { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public long RewardExp { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public List<Item> RewardItem { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public int RewardHonor { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public int RewardBadge { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public long RewardRibi { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public int RewardTitle { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public List<int> OtherRewardsTypes { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public List<int> OtherRewardsValues { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public List<int> OtherCompleteConditions { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public List<string> SNPC_R { get; set; }
        [ProtoMember(22, IsRequired = false)]
        public List<string> SNPC_O { get; set; }
        [ProtoMember(23, IsRequired = false)]
        public List<string> ENPC_O { get; set; }
        [ProtoMember(24, IsRequired = false)]
        public List<string> ENPC_F { get; set; }
        #endregion

        [ProtoMember(4, IsRequired = false)]
        public TimeSpan MaxRestTime { get; set; }

        [ProtoMember(20, IsRequired = false)]
        public int MaxAcceptableTimes { get; set; }
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
}
