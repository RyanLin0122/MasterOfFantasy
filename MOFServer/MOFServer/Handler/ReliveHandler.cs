using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class ReliveHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            if (msg.playerReliveReq == null)
            {
                return;
            }
            PlayerReliveReq pr = msg.playerReliveReq;
            MOFCharacter character = null;
            if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(pr.CharacterName, out character))
            {
                if (character != null)
                {
                    MOFMap map = character.mofMap;
                    if (map != null)
                    {
                        switch (pr.ReliveMethod)
                        {
                            case 0: //普通復活
                                MOFMap returnMap = MapSvc.Instance.GetMap(character.session.ActiveServer, character.session.ActiveChannel, pr.TownID);
                                ToOtherMapReq toOther = new ToOtherMapReq
                                {
                                    CharacterName = pr.CharacterName,
                                    LastMapID = map.mapid,
                                    Position = returnMap.playerBornPos,
                                    MapID = pr.TownID
                                };
                                msg.toOtherMapReq = toOther;
                                character.player.HP = 50;
                                character.nEntity.HP = 50;
                                character.trimedPlayer.HP = 50;
                                character.status = PlayerStatus.Normal;
                                character.entityStatus = EntityStatus.Idle;
                                ProtoMsg rsp = new ProtoMsg
                                {
                                    MessageType = 39,
                                    playerReliveRsp = new PlayerReliveRsp
                                    {
                                        CharacterName = pr.CharacterName,
                                        ReliveMethod = 0,
                                        UpdateHp = 50,
                                        UpdateMp = character.nEntity.MP,
                                        TownID = pr.TownID
                                    }
                                };
                                character.mofMap.BroadCastMassege(rsp);
                                returnMap.DoToOtherMapReq(msg, character.session);
                                
                                break;
                            case 1: //消耗點數復活
                                session.AccountData.Cash -= 5;
                                character.player.HP = character.nEntity.MaxHP;
                                character.nEntity.HP = character.nEntity.MaxHP;
                                character.trimedPlayer.HP = character.nEntity.MaxHP;
                                character.status = PlayerStatus.Normal;
                                character.entityStatus = EntityStatus.Idle;
                                ProtoMsg CashRsp = new ProtoMsg
                                {
                                    MessageType = 39,
                                    playerReliveRsp = new PlayerReliveRsp
                                    {
                                        CharacterName = pr.CharacterName,
                                        ReliveMethod = 1,
                                        UpdateHp = character.nEntity.MaxHP,
                                        UpdateMp = character.nEntity.MP,
                                        TownID = pr.TownID
                                    }
                                };
                                character.mofMap.BroadCastMassege(CashRsp);
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
    }
}

