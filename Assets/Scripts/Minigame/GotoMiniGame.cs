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

    public void ReportScore(int MiniGameID, int Score, int SwordPoint,int ArcheryPoint, int MagicPoint, int TheologyPoint)
    {
        new MiniGameScoreReqSender(MiniGameID,Score,SwordPoint,ArcheryPoint,MagicPoint,TheologyPoint);
    }
}
