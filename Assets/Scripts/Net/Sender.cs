using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using DotNetty.Transport.Channels;
public abstract class BaseSender
{
    private ClientNettySession session;
    public void SendMsg(ProtoMsg msg)
    {
        session = NetSvc.Instance.NettySession;
        if (session == null)
        {
            Debug.Log("尚未連接到伺服器");
            return;
        }

        try
        {
            IChannel channel = session.channel;
            if (session.PrivateKey != null)
            {
                channel.WriteAndFlushAsync(msg.SerializeToBytes(msg, session.PrivateKey));
            }
            else
            {
                channel.WriteAndFlushAsync(msg.SerializeToBytes(msg, Constants.PUBLIC_KEY));
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}

public class LoginSender : BaseSender
{
    public LoginSender(string Account, string Password, string MAC, string IPAddress)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 1,
            Account = Account,
            loginRequest = new LoginRequest
            {
                Account = Account,
                Password = Password,
                MAC = MAC,
                IPAddress = IPAddress
            }
        };
        SendLoginMsg(msg);
    }

    public void SendLoginMsg(ProtoMsg msg)
    {
        base.SendMsg(msg);
    }
}

public class ServerSender : BaseSender
{
    public ServerSender(string Account, int servernum, int channelnum)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 3,
            Account = Account,
            serverChooseRequest = new ServerChooseRequest
            {
                ChoosedServer = servernum,
                ChoosedChannel = channelnum
            }
        };
        SendChoosedMsg(msg);
    }
    public void SendChoosedMsg(ProtoMsg msg)
    {
        base.SendMsg(msg);
    }
}

public class HeartSender : BaseSender
{
    public HeartSender()
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 0,
            SessionID = NetSvc.Instance.NettySession.sessionID,
        };
        base.SendMsg(msg);
    }
}

public class CreateSender : BaseSender
{
    public CreateSender(CreateInfo info)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 5,
            Account = GameRoot.Instance.Account,
            CreateRequest = info
        };
        base.SendMsg(msg);
    }
}

public class DeleteSender : BaseSender
{
    public DeleteSender(string Account, string Name)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 7,
            Account = GameRoot.Instance.Account,
            deleteRequest = new DeleteRequest
            {
                CharacterName = Name
            }
        };
        base.SendMsg(msg);
    }
}

public class LogoutSender : BaseSender
{
    public LogoutSender(string CharacterName = "")
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 9,

            logoutReq = new LogoutReq
            {
                Account = GameRoot.Instance.Account,
                SessionID = NetSvc.Instance.NettySession.sessionID,
                ActiveCharacterName = CharacterName
            }
        };
        base.SendMsg(msg);
    }
}

public class EnterGameSender : BaseSender
{
    public EnterGameSender(int MapID, float[] Position, bool IsNew, bool IsTrain)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 11,
            enterGameReq = new EnterGameReq
            {

                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                MapID = MapID,
                Position = Position,
                Isnew = IsNew,
                Istrain = IsTrain
            }
        };
        base.SendMsg(msg);
    }
}

public class MoveSender : BaseSender
{
    public MoveSender(EntityEvent entityEvent, NEntity nEntity)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 14,
            entitySyncReq = new EntitySyncRequest
            {
                MapID = GameRoot.Instance.ActivePlayer.MapID,
                entityEvent = new List<EntityEvent>() { entityEvent },
                nEntity = new List<NEntity>() { nEntity }
            }
        };
        base.SendMsg(msg);
    }
}

public class ToOtherMapSender : BaseSender
{
    public ToOtherMapSender(int mapID, Vector2 position)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 15,
            toOtherMapReq = new ToOtherMapReq
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                LastMapID = GameRoot.Instance.ActivePlayer.MapID,
                MapID = mapID,
                Position = new float[] { position.x, position.y }
            }
        };
        base.SendMsg(msg);
    }

}

public class AddPropertySender : BaseSender
{
    public AddPropertySender(AddPropertyPoint ap)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 18,
            addPropertyPoint = ap
        };
        base.SendMsg(msg);
    }
}

public class MiniGameRequestSender : BaseSender
{
    public MiniGameRequestSender(string Name, int MiniGameID, int lastMapID)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 20,
            miniGameReq = new EnterMiniGameReq
            {
                CharacterName = Name,
                MiniGameID = MiniGameID,
                LastMapID = lastMapID
            }
        };
        base.SendMsg(msg);
    }

}

public class MiniGameScoreReqSender : BaseSender
{
    public MiniGameScoreReqSender(int MiniGameID, int Score, int SwordPoint, int ArcheryPoint, int MagicPoint, int TheologyPoint, int CardID, bool IsRecycle, bool IsSuccess, int Difficulty)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 22,
            miniGameScoreReq = new MiniGameScoreReq
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                MiniGameID = MiniGameID,
                Score = Score,
                SwordPoint = GameRoot.Instance.ActivePlayer.MiniGameRatio * SwordPoint,
                ArcheryPoint = GameRoot.Instance.ActivePlayer.MiniGameRatio * ArcheryPoint,
                MagicPoint = GameRoot.Instance.ActivePlayer.MiniGameRatio * MagicPoint,
                TheologyPoint = GameRoot.Instance.ActivePlayer.MiniGameRatio * TheologyPoint,
                IsRecycle = IsRecycle,
                ItemID = CardID,
                IsSuccess = IsSuccess,
                Difficulty = Difficulty
            }
        };
        base.SendMsg(msg);
    }
}

public class ChatSender : BaseSender
{
    public ChatSender(int messageType, string contents)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 24,
            chatRequest = new ChatRequest
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                MessageType = messageType,
                Contents = contents
            }
        };
        base.SendMsg(msg);
    }
}

public class CalculatorReadySender : BaseSender
{
    public CalculatorReadySender()
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 27,
            calculatorReady = new CalculatorReady
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name
            }
        };
        base.SendMsg(msg);

    }
}

public class CalculatorReportSender : BaseSender
{
    public CalculatorReportSender(Dictionary<int, float[]> pos)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 28,
            calculatorReport = new CalculatorReport
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                MonsterPos = pos
            }
        };
        base.SendMsg(msg);
    }
}
public class DamageSender : BaseSender
{
    public DamageSender(Dictionary<int, int> monsterMapID, Dictionary<int, int[]> damages, Dictionary<int, bool> IsCritical, Dictionary<int, AttackType> attackTypes, int AnimID, bool FaceDir, float Delay)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 30,
            monsterGetHurt = new MonsterGetHurt
            {
                MonsterMapID = monsterMapID,
                AttackTypes = attackTypes,
                Damage = damages,
                IsCritical = IsCritical,
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                AnimID = AnimID,
                FaceDir = FaceDir,
                Delay = Delay
            }
        };
        base.SendMsg(msg);
    }
}

public class LevelUpSender : BaseSender
{
    public LevelUpSender(long RestExp)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 33,
            levelUp = new LevelUp
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                Level = GameRoot.Instance.ActivePlayer.Level + 1,
                RestExp = RestExp
            }
        };
        base.SendMsg(msg);
    }
}

public class PlayerGetHurtSender : BaseSender
{
    public PlayerGetHurtSender(int damage, HurtType hurtType, int monsterID, bool faceDir)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 36,
            playerGetHurt = new PlayerGetHurt
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                damage = damage,
                hurtType = hurtType,
                MonsterID = monsterID,
                FaceDir = faceDir
            }
        };
        base.SendMsg(msg);
    }
}

public class PlayerActionSender : BaseSender
{
    public PlayerActionSender(int playerActionType, bool faceDir, int AniType)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 40,
            playerAction = new PlayerAction
            {
                PlayerActionType = playerActionType,
                FaceDir = faceDir,
                AnimationType = AniType
            }
        };
        base.SendMsg(msg);
    }
}

public class KnapsackSender : BaseSender
{
    public KnapsackSender(int OperationType, List<Item> items, int[] OldPos, int[] NewPos, long ribi = 0)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 41,
            knapsackOperation = new KnapsackOperation
            {
                OperationType = OperationType,
                items = items,
                OldPosition = OldPos,
                NewPosition = NewPos,
                Ribi = ribi
            }
        };
        base.SendMsg(msg);
    }
}

public class EquipmentSender : BaseSender
{
    public EquipmentSender(int operationType, int equipmentPos, Item putOffEqiupment, int knapsackPos, Item putOnEquipment)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 42,
            equipmentOperation = new EquipmentOperation
            {
                OperationType = operationType,
                EquipmentPosition = equipmentPos,
                PutOffEquipment = putOffEqiupment,
                KnapsackPosition = knapsackPos,
                PutOnEquipment = putOnEquipment
            }
        };
        base.SendMsg(msg);
    }
}

public class RecycleItemsSender : BaseSender
{
    public ProtoMsg msg;
    public RecycleItemsSender(int InventoryType) //0:knapsack 1:locker
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 43,
            recycleItems = new RecycleItems
            {
                InventoryType = InventoryType,
                ItemID = new List<int>(),
                Amount = new List<int>(),
                Positions = new List<int>()
            }
        };
        this.msg = msg;
    }
    public void AddItem(int ItemID, int Position, int Amount)
    {
        if (msg != null && msg.recycleItems.ItemID != null && msg.recycleItems.Positions != null && msg.recycleItems.Amount != null)
        {
            msg.recycleItems.ItemID.Add(ItemID);
            msg.recycleItems.Positions.Add(Position);
            msg.recycleItems.Amount.Add(Amount);
        }
    }
    public void SendMsg()
    {
        if (msg != null && msg.recycleItems != null && msg.recycleItems.ItemID.Count > 0)
        {
            Debug.Log("SendRecycle");
            base.SendMsg(msg);
        }
    }

}

public class MiniGameSettingSender : BaseSender
{
    public MiniGameSettingSender(int[] MiniGameArr, int MiniGameRatio)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 44,
            miniGameSetting = new MiniGameSetting
            {
                MiniGameArray = MiniGameArr,
                MiniGameRatio = MiniGameRatio
            }
        };
        base.SendMsg(msg);
    }
}

public class CashShopSender : BaseSender
{
    /// <summary>
    /// 買東西
    /// </summary>
    /// <param name="Operation"></param>
    /// <param name="Cata"></param>
    /// <param name="Tag"></param>
    /// <param name="ID"></param>
    /// <param name="Amount"></param>
    /// <param name="TotalPrice"></param>
    public CashShopSender(int Operation, List<string> Cata, List<string> Tag, List<int> Order, List<int> Amount, List<int> Quantity, int TotalPrice)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 46,
            cashShopRequest = new CashShopRequest
            {
                OperationType = 1,
                Cash = GameRoot.Instance.AccountData.Cash,
                TotalPrice = TotalPrice,
                Cata = Cata,
                Tag = Tag,
                Orders = Order,
                Quantity = Quantity,
                Amount = Amount
            }
        };
        base.SendMsg(msg);
    }
    /// <summary>
    /// 送禮
    /// </summary>
    /// <param name="Operation"></param>
    /// <param name="Cata"></param>
    /// <param name="Tag"></param>
    /// <param name="ID"></param>
    /// <param name="Amount"></param>
    /// <param name="TotalPrice"></param>
    public CashShopSender(int Operation, List<string> Cata, List<string> Tag, List<int> Order, List<int> Amount, int TotalPrice, string GiftPlayerName)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 46,
            cashShopRequest = new CashShopRequest
            {
                OperationType = 2,
                Cash = GameRoot.Instance.AccountData.Cash,
                TotalPrice = TotalPrice,
                Cata = Cata,
                Tag = Tag,
                Orders = Order,
                Amount = Amount,
                GiftPlayerName = GiftPlayerName
            }
        };
        base.SendMsg(msg);
    }

    /// <summary>
    /// 放入背包
    /// </summary>
    /// <param name="Operation"></param>
    /// <param name="Position"></param>
    public CashShopSender(int Operation, List<int> Position, bool IsFashionPanel)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 46,
            cashShopRequest = new CashShopRequest
            {
                OperationType = 4,
                Positions = Position,
                IsFashionPanel = IsFashionPanel
            }
        };
        base.SendMsg(msg);
    }
    /// <summary>
    /// 放東西進購物車，或從購物車刪除
    /// </summary>
    /// <param name="Operation"></param>
    public CashShopSender(int Operation, List<CartItem> cartItems)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 46,
            cashShopRequest = new CashShopRequest
            {
                OperationType = Operation,
                CartItems = cartItems
            }
        };
        base.SendMsg(msg);
    }
}

public class TransactionSender : BaseSender
{
    /// <summary>
    /// 發起邀請,接受邀請，開啟交易，取消交易
    /// </summary>
    /// <param name="OperationType"></param>
    /// <param name="items"></param>
    /// <param name="OldPos"></param>
    /// <param name="NewPos"></param>
    /// <param name="ribi"></param>
    public TransactionSender(int OperationType, string OtherName)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 48,
            transactionRequest = new TransactionRequest
            {
                OperationType = OperationType,
                PlayerName = GameRoot.Instance.ActivePlayer.Name,
                OtherPlayerName = OtherName,
            }
        };
        base.SendMsg(msg);
    }

    /// <summary>
    /// 放東西進去
    /// </summary>
    /// <param name="OperationType"></param>
    /// <param name="OtherName"></param>
    public TransactionSender(int OperationType, string OtherName, int TransactionPos, int KnapsackPos, Item item)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 48,
            transactionRequest = new TransactionRequest
            {
                OperationType = OperationType,
                PlayerName = GameRoot.Instance.ActivePlayer.Name,
                OtherPlayerName = OtherName,
                TransactionPos = TransactionPos,
                KnapsackPos = KnapsackPos,
                item = item

            }
        };
        base.SendMsg(msg);
    }

    public TransactionSender(int OperationType, string OtherName, long Ribi)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 48,
            transactionRequest = new TransactionRequest
            {
                OperationType = OperationType,
                PlayerName = GameRoot.Instance.ActivePlayer.Name,
                OtherPlayerName = OtherName,
                PutRubi = Ribi
            }
        };
        base.SendMsg(msg);

    }

}
public class LockerSender : BaseSender
{
    public LockerSender(int OperationType, List<Item> items, int[] OldPos, int[] NewPos)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 50,
            lockerOperation = new LockerOperation
            {
                OperationType = OperationType,
                items = items,
                OldPosition = OldPos,
                NewPosition = NewPos,
                Ribi = 0
            }
        };
        base.SendMsg(msg);
    }
    public LockerSender(int OperationType, long Ribi)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 50,
            lockerOperation = new LockerOperation
            {
                OperationType = OperationType,
                Ribi = Ribi
            }
        };
        base.SendMsg(msg);
    }
}

public class MailBoxSender : BaseSender
{
    public MailBoxSender(int OperationType, List<Item> items, int[] OldPos, int[] NewPos)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 51,
            mailBoxOperation = new MailBoxOperation
            {
                OperationType = OperationType,
                items = items,
                OldPosition = OldPos,
                NewPosition = NewPos,
                Ribi = 0
            }
        };
        base.SendMsg(msg);
    }
    public MailBoxSender(int OperationType, long Ribi)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 51,
            mailBoxOperation = new MailBoxOperation
            {
                OperationType = OperationType,
                Ribi = Ribi
            }
        };
        base.SendMsg(msg);
    }
}
public class StrengthenSender : BaseSender
{
    public StrengthenSender(int OperationType, Item item)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 52,
            strengthenRequest = new StrengthenRequest
            {
                OperationType = OperationType,
                item = item
            }
        };
        base.SendMsg(msg);
    }
    public StrengthenSender(int OperationType)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 52,
            strengthenRequest = new StrengthenRequest
            {
                OperationType = OperationType,
            }
        };
        base.SendMsg(msg);

    }
}
public class LearnSkillSender : BaseSender
{
    public LearnSkillSender(int SkillID, int Level)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 56,
            learnSkill = new LearnSkill
            {
                SkillID = SkillID,
                Level = Level
            }
        };
        base.SendMsg(msg);
    }
}

public class HotKeySender : BaseSender
{
    public HotKeySender(int OperationType, HotkeyData NewData, HotkeyData OldData)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 57,
            hotKeyOperation = new HotKeyOperation
            {
                OperationType = OperationType,
                NewHotKeyData = NewData,
                OldHotKeyData = OldData
            }
        };
        base.SendMsg(msg);
    }
}

public class SkillSender : BaseSender
{
    public SkillSender(SkillCastInfo castInfo)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 54,
            skillCastRequest = new SkillCastRequest
            {
                CastInfo = castInfo
            }
        };
        base.SendMsg(msg);
    }
}
public class ManufactureSender : BaseSender
{
    public ManufactureSender(int OperationType, int FormulaID, int Amount)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 58,
            manufactureRequest = new ManufactureRequest
            {
                OperationType = OperationType,
                FormulaId = FormulaID,
                Amount = Amount
            }
        };
        base.SendMsg(msg);
    }
}

public class RunSender : BaseSender
{
    public RunSender()
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 61,
            runOperation = new RunOperation
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                IsRun = !PlayerInputController.Instance.entityController.IsRun
            }
        };
        base.SendMsg(msg);
    }
}

public class PickUpSender : BaseSender
{
    public PickUpSender(int UUID, int InventoryID, int InventoryPosition)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 63,
            pickUpRequest = new PickUpRequest
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                ItemUUID = UUID,
                InventoryID = InventoryID,
                InventoryPosition = InventoryPosition
            }
        };
        base.SendMsg(msg);
    }
}

public class ConsumableSender : BaseSender
{
    public ConsumableSender(Item item, int InventoryID, int InventoryPosition)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 64,
            consumableOperation = new ConsumableOperation
            {
                item = item,
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                HP = -1,
                MP = -1,
                IsSuccess = false,
                RestNum = item.Count - 1,
                InventoryID = InventoryID,
                InventoryPosition = InventoryPosition
            }
        };
        base.SendMsg(msg);
    }
}

public class AcceptQuestSender : BaseSender
{
    public AcceptQuestSender(int QuestID)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 65,
            questAcceptRequest = new QuestAcceptRequest
            {
                quest_id = QuestID
            }
        };
        base.SendMsg(msg);
    }
}

public class SubmitQuestSender : BaseSender
{
    public SubmitQuestSender(int QuestID)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 67,
            questSubmitRequest = new QuestSubmitRequest
            {
                quest_id = QuestID
            }
        };
        base.SendMsg(msg);
    }
}

public class AbandonQuestSender : BaseSender
{
    public AbandonQuestSender(int QuestID)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 69,
            questAbandonRequest = new QuestAbandonRequest
            {
                quest_id = QuestID
            }
        };
        base.SendMsg(msg);
    }
}

public class ChangeChannelSender : BaseSender
{
    public ChangeChannelSender(int Channel)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 72,
            changeChannelOperation = new ChangeChannelOperation
            {
                Result = false,
                Channel = Channel
            }
        };
        base.SendMsg(msg);
    }
}

public class SellItemSender : BaseSender
{
    public SellItemSender(bool IsCash, int ItemID, int Count, int Position)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 73,
            sellItemReq = new SellItemReq
            {
                IsCash = IsCash,
                ItemID = ItemID,
                Count = Count,
                Position = Position
            }
        };
        base.SendMsg(msg);
    }
}