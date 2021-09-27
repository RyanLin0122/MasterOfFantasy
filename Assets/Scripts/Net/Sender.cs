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
    public MoveSender(int MapID, float[] Destination)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 14,
            moveMapPlayer = new MoveMapPlayer
            {
                MapID = MapID,
                PlayerName = GameRoot.Instance.ActivePlayer.Name,
                Destination = Destination
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
    public MiniGameScoreReqSender(int MiniGameID, int Score, int SwordPoint, int ArcheryPoint, int MagicPoint, int TheologyPoint)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 22,
            miniGameScoreReq = new MiniGameScoreReq
            {
                CharacterName = GameRoot.Instance.ActivePlayer.Name,
                MiniGameID = MiniGameID,
                Score = Score,
                SwordPoint = SwordPoint,
                ArcheryPoint = ArcheryPoint,
                MagicPoint = MagicPoint,
                TheologyPoint = TheologyPoint
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

}
