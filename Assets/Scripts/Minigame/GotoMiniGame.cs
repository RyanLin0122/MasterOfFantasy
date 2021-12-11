using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class GotoMiniGame : SystemRoot
{
    public static GotoMiniGame Instance = null;
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init MiniGameSys...");
    }
    public enum MiniGameEnum
    {
        card,
        archery,
        shuriken,
        puzzle,
        shoot,
        kungfu,
        dummy,
        cure,
        none
    }
    public MiniGameEnum game;

    public Dictionary<string, int> ranking;
    public void GoToMiniGame()
    {
        if (game.Equals(MiniGameEnum.card))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 1, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.archery))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 2, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.shuriken))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 3, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.puzzle))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 4, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.shoot))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 5, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.kungfu))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 6, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.dummy))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 7, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.cure))
        {
            new MiniGameRequestSender(GameRoot.Instance.ActivePlayer.Name, 8, GameRoot.Instance.ActivePlayer.MapID);
        }
        else if (game.Equals(MiniGameEnum.none))
        {
            GameRoot.AddTips("尚未選擇小遊戲。 You haven't choose a minigame.");
        }
    }

    public bool CanPlay(int MiniGameID)
    {
        bool result = false;
        if (GameRoot.Instance.ActivePlayer.MiniGameArr != null)
        {
            if (GameRoot.Instance.ActivePlayer.MiniGameArr[0] == MiniGameID)
            {
                int CardID = -1;
                switch (GameRoot.Instance.ActivePlayer.MiniGameRatio)
                {
                    case 1:
                        CardID = 12001;
                        break;
                    case 2:
                        CardID = 12002;
                        break;
                    case 4:
                        CardID = 12003;
                        break;
                    default:
                        CardID = 12001;
                        break;
                }
                if (InventorySys.Instance.HasItem(CardID, 1))
                {
                    bool IsAferAllZero = true;
                    for (int i = 1; i < GameRoot.Instance.ActivePlayer.MiniGameArr.Length; i++)
                    {
                        if(GameRoot.Instance.ActivePlayer.MiniGameArr[i]>0)
                        {
                            IsAferAllZero = false;
                            break;
                        }    
                    }
                    if (GameRoot.Instance.ActivePlayer.MiniGameArr.Length == 1 || IsAferAllZero)
                    {
                        GameRoot.Instance.ActivePlayer.MiniGameArr = null;
                    }
                    else
                    {
                        int[] OGameArr = GameRoot.Instance.ActivePlayer.MiniGameArr;
                        int OLength = GameRoot.Instance.ActivePlayer.MiniGameArr.Length;
                        GameRoot.Instance.ActivePlayer.MiniGameArr = new int[OLength - 1];
                        for (int i = 0; i < OLength - 1; i++)
                        {
                            GameRoot.Instance.ActivePlayer.MiniGameArr[i] = OGameArr[i + 1];
                        }
                    }
                    UISystem.Instance.baseUI.SetClassImg();
                    result = true;
                }
            }
        }
        if (!result) GameRoot.AddTips("無法進行小遊戲，請檢查小遊戲設定，以及背包內訓練卡數量");
        return result;
    }

    public void ReportScore(int MiniGameID, int Score, int SwordPoint, int ArcheryPoint, int MagicPoint, int TheologyPoint, bool IsSuccess, int Difficulty)
    {
        int CardID = -1;
        switch (GameRoot.Instance.ActivePlayer.MiniGameRatio)
        {
            case 1:
                CardID = 12001;
                break;
            case 2:
                CardID = 12002;
                break;
            case 4:
                CardID = 12003;
                break;
            default:
                CardID = 12001;
                break;
        }
        UpdateMiniGameRecord(GameRoot.Instance.ActivePlayer, IsSuccess, MiniGameID, Difficulty, Score);
        DiaryWnd.Instance.Transcipt.SetScores();
        if (GameRoot.Instance.ActivePlayer.MiniGameArr == null || GameRoot.Instance.ActivePlayer.MiniGameArr.Length == 0)
        {
            new MiniGameScoreReqSender(MiniGameID, Score, SwordPoint, ArcheryPoint, MagicPoint, TheologyPoint, CardID, true, IsSuccess, Difficulty);
        }
        else
        {
            new MiniGameScoreReqSender(MiniGameID, Score, SwordPoint, ArcheryPoint, MagicPoint, TheologyPoint, CardID, false, IsSuccess, Difficulty);
        }
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
