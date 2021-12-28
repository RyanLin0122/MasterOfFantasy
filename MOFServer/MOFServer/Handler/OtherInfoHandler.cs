using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class OtherInfoHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            if (msg.otherProfileOperation == null) return;
            var pr = msg.otherProfileOperation;
            var CharacterName = pr.Name;
            if (string.IsNullOrEmpty(CharacterName)) return;
            MOFCharacter character = null;

            if(CacheSvc.Instance.MOFCharacterDict.TryGetValue(CharacterName, out character))
            {
                if(character!=null && character.player != null)
                {
                    var player = character.player;
                    pr.Name = player.Name;
                    pr.Job = player.Job;
                    pr.CurrentBadge = player.CurrentBadge;
                    pr.Level = player.Level;
                    pr.Grade = player.Grade;
                    pr.Title = player.Title;
                    pr.Guild = player.Guild;
                    pr.BattleWinTimes = player.BattleWinTimes;
                    pr.BattleLoseTimes = player.BattleLoseTimes;
                    pr.PVPRank = player.PVPRank;
                    pr.PVPWinTimes = player.PVPWinTimes;
                    pr.PVPLoseTimes = player.PVPLoseTime;
                    pr.PVPPoints = player.PVPPoints;
                    pr.Gender = player.Gender;
                    if (player.playerEquipments != null)
                    {
                        pr.PlayerEquipments = player.playerEquipments;
                    }
                    else
                    {
                        pr.PlayerEquipments = new PlayerEquipments();
                    }
                    session.WriteAndFlush(msg);
                }
            }            
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }
}

