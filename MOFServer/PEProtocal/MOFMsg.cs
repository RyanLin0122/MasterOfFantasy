using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEProtocal
{
    [Serializable]
    public class MOFMsg
    {
        public int id;
        public int cmd;
        public int err;
        public MovePlayer movePlayer;
        public AddPlayer addPlayer;
        public RemovePlayer removePlayer;
        public ReadMapPlayers readMapPlayers;
        public PlayerData playerData;
        public ChatMessage chatMessage;
        public ReqCharacterItem reqCharacterItem;
        public KnapsackRelated knapsackRelated;
        public EquipmentRelated equipmentRelated;
        public ChangeEquipment changeEquipment;
        public LockerRelated lockerRelated;
        public MailBoxRelated mailBoxRelated;
        public PlayerData PsuedoPd;
        public RecoverHp recoverHp;
        public RecoverMp recoverMp;
        public PropertyData property;
    }

    //通知同地圖的客戶端角色移動中 
    [Serializable]
    public class MovePlayer //cmd:10
    {
        //一個封包紀錄N個位置
        public int N;
        public int MapID;
        public int Transition; //1: 靜止到移動 2: 穩定移動 3:移動到靜止 
        public bool IsRun;
        public int CharacterID;
        public int Serialization;
        public int SendClock;
        public bool[] FaceDirections; //false 向左 true 向右
        public float[] Positions;
    }
    //通知同地圖的客戶端增加角色在地圖上 
    [Serializable]
    public class AddPlayer //cmd:11
    {
        public int CharacterID;
        public string CharacterName;
        public PlayerData pd;
        public int MapID;
        public float[] Position;
        public bool IsPortal;
        public int LastMapID;
        public PlayerData PsuedoPd;
    }
    //通知同地圖的客戶端從地圖刪除角色
    [Serializable]
    public class RemovePlayer //cmd:12
    {
        public int CharacterID;
    }

    //讀入地圖上所有已存在角色
    [Serializable]
    public class ReadMapPlayers //cmd:13
    {
        public bool IsPortal;
        public float[] Position;
        public int MapID;
        public ReadMapPlayers()
        {
            mapPlayers = new List<AddPlayer>();
        }
        public void addplayer(int ID, float[] position, string ChrName, PlayerData pd, PlayerData PsuedoPd)
        {
            mapPlayers.Add(new AddPlayer { CharacterName = ChrName, CharacterID = ID, Position = position ,pd = pd});
        }
        public List<AddPlayer> mapPlayers;

    }
    [Serializable]
    public class ReqReadMapPlayer //cmd:14
    {
        public int channel;
        public int mapID;

    }
    //聊天訊息
    [Serializable]
    public class ChatMessage //cmd:16
    {
        public string ChrName;
        public int MessageType; //1: 所有人 2: 好友 3: 隊伍 4: 公會 5: 密語 6:廣播
        public string Content;
        public int targetID;
        public int MapID;
    }
    
    //cmd:18 可否換頻
    
    //cmd:19 請求讀入角色物品、倉庫、信箱、裝備
    [Serializable]
    public class EncodedItem
    {
        public int DataBaseID;
        public Item item;
        public int position; //格數
        public int amount;
        public int WindowType; //1: 背包
    }
    [Serializable]
    public class ReqCharacterItem
    {
        public Dictionary<int, EncodedItem> KnapsackItems;
        public Dictionary<int, EncodedItem> KnapsackCashItems;
        public Dictionary<int, EncodedItem> LockerItems;
        public Dictionary<int, EncodedItem> LockerCashItems;
        public Dictionary<int, EncodedItem> MailBoxItems;
        public Dictionary<int, EncodedItem> EquipmentItems;
    }

    //cmd:20 背包相關操作
    [Serializable]
    public class KnapsackRelated
    {
        public long Price;
        public int Type;
        ///第一種: 存入一個道具
        ///第二種: 增加數量
        ///第三種: 增加不定數量道具
        ///第四種: 交換
        ///第五種: 刪除
        ///第六種: 買東西
        ///第七種: 賣東西
        ///第八種: 整理背包
        public List<EncodedItem> encodedItems;
        public int OldPosition;
        public int NewPosition;
        public int OldDBID;
        public int NewDBID;
    }

    //cmd: 21 裝備相關操作
    [Serializable]
    public class EquipmentRelated
    {
        public int Type;
        ///第一種:穿裝備進空格
        ///第二種:換裝備
        ///第三種:脫下裝備進背包空格
        public EncodedItem PutOnEquipment;
        public EncodedItem PutOffEquipment;
        public int KnapsackPosition;
        public int EquipmentPosition;
    }

    //cmd: 22 通知別人換裝
    [Serializable]
    public class ChangeEquipment
    {
        public PlayerData pd;
        public int WeaponQuality;
        //ToDo
    }
    //cmd: 23 倉庫
    [Serializable]
    public class LockerRelated
    {
        public long Price;
        public int Type;
        ///第一種: 存入道具
        ///第二種: 交換
        ///第三種: 取出道具
        public List<EncodedItem> encodedItems;
        public int OldPosition;
        public int NewPosition;
        public int LockerPosition;
        public int KnapsackPosition;
        public int OldDBID;
        public int NewDBID;
    }
    //cmd: 24 信箱
    [Serializable]
    public class MailBoxRelated
    {
        public long Price;
        public int Type;
        ///第一種: 存入道具
        ///第二種: 取出道具
        public List<EncodedItem> encodedItems;
        public int OldPosition;
        public int NewPosition;
        public int MailBoxPosition;
        public int KnapsackPosition;
        public int OldDBID;
        public int NewDBID;
    }
    //cmd: 25 回復HP
    [Serializable]
    public class RecoverHp
    {
        public int AddHp;
    }
    //cmd: 26 回復MP
    [Serializable]
    public class RecoverMp
    {
        public int AddMp;
    }
    //cmd: 27 更新能力值
    [Serializable]
    public class PropertyData
    {
        public int RestPoint;
        public int Att;
        public int Health;
        public int Dex;
        public int Int;
    }
}
