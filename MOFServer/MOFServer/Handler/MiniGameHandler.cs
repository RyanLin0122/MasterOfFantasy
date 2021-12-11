using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class MiniGameHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        MiniGameScoreReq mg = msg.miniGameScoreReq;
        if (mg == null)
        {
            SendErrorBack(session, "封包為空");
            return;
        }
        session.ActivePlayer.SwordPoint += msg.miniGameScoreReq.SwordPoint;
        session.ActivePlayer.ArcheryPoint += msg.miniGameScoreReq.ArcheryPoint;
        session.ActivePlayer.MagicPoint += msg.miniGameScoreReq.MagicPoint;
        session.ActivePlayer.TheologyPoint += msg.miniGameScoreReq.TheologyPoint;
        UpdateMiniGameRecord(session.ActivePlayer, mg.IsSuccess, mg.MiniGameID, mg.Difficulty, mg.Score);
        CacheSvc.Instance.ReportScore(session.ActivePlayer.Name, msg.miniGameScoreReq.Score, msg.miniGameScoreReq.MiniGameID);
        
        int DeleteItemPos = -1;
        bool DeleteItemIsCash = false;
        Item RecycleItem = null;
        if (mg.IsRecycle)
        {
            int RecycleItemID = mg.ItemID;
            MOFCharacter character = null;
            if (RecycleItemID <= 0) { SendErrorBack(session, "回收物品ID為空"); return; }
            if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out character))
            {
                if (character.HasItem(RecycleItemID, 1))
                {
                    var result = character.RecycleItem(RecycleItemID, 1);
                    if(result.Item1.Count == 0)
                    {
                        //只有一張，刪除
                        DeleteItemPos = result.Item2[0];
                        DeleteItemIsCash = result.Item3[0];
                    }
                    else
                    {
                        RecycleItem = result.Item1[0];
                    }
                }
                else
                {
                    SendErrorBack(session, "沒有道具: " + RecycleItemID); return;
                }
            }
            else
            {
                SendErrorBack(session, "MOFCharacter為空"); return;
            }
        }
        ProtoMsg back = new ProtoMsg
        {
            MessageType = 23,
            miniGameScoreRsp = new MiniGameScoreRsp
            {
                Result = true,
                MiniGameID = msg.miniGameScoreReq.MiniGameID,
                MiniGameRanking = CacheSvc.Instance.MiniGame_Records[msg.miniGameScoreReq.MiniGameID - 1],
                SwordPoint = session.ActivePlayer.SwordPoint,
                ArcheryPoint = session.ActivePlayer.ArcheryPoint,
                MagicPoint = session.ActivePlayer.MagicPoint,
                TheologyPoint = session.ActivePlayer.TheologyPoint,
                RecycleItem = RecycleItem,
                DeleteItemPos = DeleteItemPos,
                DeleteItemIsCash = DeleteItemIsCash
            }
        };
        session.WriteAndFlush(back);       
    }

    public void SendErrorBack(ServerSession session, string ErrorMsg)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 23,
            miniGameScoreRsp = new MiniGameScoreRsp
            {
                Result = false,
                ErrorMsg = ErrorMsg
            }
        };
        session.WriteAndFlush(msg);
    }

    public void UpdateMiniGameRecord(Player player, bool IsSuccess, int GameID, int Difficulty, int Score)
    {
        //輸贏紀錄
        if (Difficulty == 0)
        {
            switch (GameID)
            {
                case 6:
                case 7:
                    if (IsSuccess) player.EasySuccess[0] += 1;
                    else player.EasyFail[0] += 1;
                    break;
                case 2:
                case 3:
                    if (IsSuccess) player.EasySuccess[1] += 1;
                    else player.EasyFail[1] += 1;
                    break;
                case 4:
                case 5:
                    if (IsSuccess) player.EasySuccess[2] += 1;
                    else player.EasyFail[2] += 1;
                    break;
                case 1:
                case 8:
                    if (IsSuccess) player.EasySuccess[3] += 1;
                    else player.EasyFail[3] += 1;
                    break;
                default:
                    break;
            }
        }
        else if (Difficulty == 1)
        {
            switch (GameID)
            {
                case 6:
                case 7:
                    if (IsSuccess) player.NormalSuccess[0] += 1;
                    else player.NormalFail[0] += 1;
                    break;
                case 2:
                case 3:
                    if (IsSuccess) player.NormalSuccess[1] += 1;
                    else player.NormalFail[1] += 1;
                    break;
                case 4:
                case 5:
                    if (IsSuccess) player.NormalSuccess[2] += 1;
                    else player.NormalFail[2] += 1;
                    break;
                case 1:
                case 8:
                    if (IsSuccess) player.NormalSuccess[3] += 1;
                    else player.NormalFail[3] += 1;
                    break;
                default:
                    break;
            }
        }
        else if (Difficulty == 2)
        {
            switch (GameID)
            {
                case 6:
                case 7:
                    if (IsSuccess) player.HardSuccess[0] += 1;
                    else player.HardFail[0] += 1;
                    break;
                case 2:
                case 3:
                    if (IsSuccess) player.HardSuccess[1] += 1;
                    else player.HardFail[1] += 1;
                    break;
                case 4:
                case 5:
                    if (IsSuccess) player.HardSuccess[2] += 1;
                    else player.HardFail[2] += 1;
                    break;
                case 1:
                case 8:
                    if (IsSuccess) player.HardSuccess[3] += 1;
                    else player.HardFail[3] += 1;
                    break;
                default:
                    break;
            }
        }
        //總分最高分
        switch (GameID)
        {
            case 6:
            case 7:
                player.TotalMiniGameScores[0] += Score;
                if (Score > player.HighestMiniGameScores[0]) player.HighestMiniGameScores[0] += Score;
                break;
            case 2:
            case 3:
                player.TotalMiniGameScores[1] += Score;
                if (Score > player.HighestMiniGameScores[1]) player.HighestMiniGameScores[1] += Score;
                break;
            case 4:
            case 5:
                player.TotalMiniGameScores[2] += Score;
                if (Score > player.HighestMiniGameScores[2]) player.HighestMiniGameScores[2] += Score;
                break;
            case 1:
            case 8:
                player.TotalMiniGameScores[3] += Score;
                if (Score > player.HighestMiniGameScores[3]) player.HighestMiniGameScores[3] += Score;
                break;
            default:
                break;
        }
    }
}
