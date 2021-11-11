using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance = null;
    private static readonly string obj = "lock";

    private Queue<ProtoMsg> MOFmsgQue = new Queue<ProtoMsg>();
    public NettyClient Nettyclient;
    public ClientNettySession NettySession;
    public void InitSvc()
    {
        Instance = this;
        //Netty的部分
        Nettyclient = new NettyClient();
        Nettyclient.doconnect();
        print("InitNetSvc...");

    }
    public void AddMOFPkg(ProtoMsg msg)
    {
        lock (obj)
        {
            MOFmsgQue.Enqueue(msg);
        }
    }
    private void Update()
    {
        if (MOFmsgQue.Count > 0)
        {
            lock (obj)
            {
                ProtoMsg msg = MOFmsgQue.Dequeue();
                ProcessMOFMsg(msg);
            }
        }
    }
    private void ProcessMOFMsg(ProtoMsg msg)
    {
        switch (msg.MessageType)
        {
            case 1:
                return;
            case 2:
                DoLoginRsp(msg);
                break;
            case 3:
                DoServerChooseRsp(msg);
                break;
            case 6:
                DoCreatePlayerRsp(msg);
                break;
            case 8:
                DoDeletePlayerRsp(msg);
                break;
            case 10:
                DoMapInformation(msg);
                break;
            case 12:
                DoEnterGameRsp(msg);
                break;
            case 13:
                DoAddMapPlayer(msg);
                break;
            case 14:
                DoSyncEntities(msg);
                break;
            case 16:
                DoToOtherMapRsp(msg);
                break;
            case 17:
                DoRemoveMapPlayer(msg);
                break;
            case 19:
                DoAddProperty(msg);
                break;
            case 21:
                DoEnterMiniGame(msg);
                break;
            case 23:
                DoMiniGameScoreRsp(msg);
                break;
            case 25:
                DoChatRsp(msg);
                break;
            case 29:
                DoGenerateMonsters(msg);
                break;
            case 30:
                DoHurtMonsters(msg);
                break;
            case 31:
                DoMonsterDeath(msg);
                break;
            case 32:
                DoExpPacket(msg);
                break;
            case 33:
                DoLevelUpPacket(msg);
                break;
            case 34:
                DoWeatherPacket(msg);
                break;
            case 35:
                DoUpdateHPMP(msg);
                break;
            case 36:
                DoPlayerGetHurt(msg);
                break;
            case 37:
                DoPlayerDeath(msg);
                break;
            case 40:
                DoPlayerAction(msg);
                break;
            case 41:
                DoKnapsackOperation(msg);
                break;
            case 42:
                DoEquipmentOperation(msg);
                break;
            case 43:
                DoRecycleItems(msg);
                break;
            case 44:
                DoMiniGameSetting(msg);
                break;
            case 45:
                DoRewards(msg);
                break;
            case 47:
                DoCashShopResponse(msg);
                break;
            case 49:
                DoTransactionResponse(msg);
                break;
            case 50:
                DoLockerOperation(msg);
                break;
            case 51:
                DoMailBoxOperation(msg);
                break;
            case 53:
                DoStrengthenResponse(msg);
                break;
            case 56:
                DoLearnSkill(msg);
                break;
            case 57:
                DoHotKeyOperation(msg);
                break;
            case 55:
                DoSkillCastResponse(msg);
                break;
            case 60:
                DoSkillHitResponse(msg);
                break;
        }
    }

    #region 業務邏輯處理
    public void DoLoginRsp(ProtoMsg msg)
    {
        GameRoot.Instance.WindowUnlock();
        if (msg.players != null)
        {
            GameRoot.Instance.SetPlayers(msg.players);
        }
        if (msg.loginResponse.accountData != null)
        {
            GameRoot.Instance.AccountData = msg.loginResponse.accountData;
        }
        else
        {
            print("Account Data is null");
        }
        LoginSys.Instance.EnterServerWnd();
        LoginSys.Instance.serverWnd.ProcessChannelStat(msg.loginResponse.ServerStatus);
    }
    public void DoServerChooseRsp(ProtoMsg msg)
    {
        GameRoot.Instance.WindowUnlock();
        if (!msg.serverChooseResponse.Result)
        {
            GameRoot.AddTips("人數已滿囉!");
        }
        else
        {
            LoginSys.Instance.serverWnd.OpenSelectWnd();
        }
    }
    public void DoCreatePlayerRsp(ProtoMsg msg)
    {
        GameRoot.Instance.WindowUnlock();
        Debug.Log("收到創角回應");
        if (msg.CreateResponse == null) //失敗
        {
            GameRoot.AddTips("角色名稱重複，請更換名字!");
        }
        else //成功
        {
            GameRoot.Instance.SetPlayer(msg.CreateResponse);
            LoginSys.Instance.selectCharacterWnd.SetCharacterProperty(GameRoot.Instance.PlayersDic[GameRoot.Instance.ActiveServer]);
            LoginSys.Instance.BackToSelectCharacterWnd();
        }
    }
    public void DoDeletePlayerRsp(ProtoMsg msg)
    {
        GameRoot.Instance.WindowUnlock();
        Debug.Log("收到刪角回應");
        if (msg.deleteRsp.Result) //刪除成功
        {
            LoginSys.Instance.RspDeletePlayer(msg.deleteRsp.Message);
        }
    }
    public void DoEnterGameRsp(ProtoMsg msg)
    {
        GameRoot.Instance.WindowUnlock();
        BattleSys.Instance.ClearMonsters();
        LoginSys.Instance.selectCharacterWnd.SetWndState(false);
        MainCitySys.Instance.EnterMap(msg.enterGameRsp);
        UISystem.Instance.InfoWnd.RefreshIInfoUI();
        BattleSys.Instance.HotKeyManager.ReadHotKey();
        BattleSys.Instance.InitAllAtribute();       
    }
    public void DoToOtherMapRsp(ProtoMsg msg)
    {
        if (msg.toOtherMapRsp.CharacterName == GameRoot.Instance.ActivePlayer.Name)
        {
            BattleSys.Instance.ClearMonsters();
            MainCitySys.Instance.GoToOtherMap(msg.toOtherMapRsp);
        }
    }
    public void DoAddPlayerRsp(ProtoMsg msg)
    {
        if (msg.addMapPlayer.MapID == GameRoot.Instance.ActivePlayer.MapID)
        {
            MainCitySys.Instance.AddPlayerToMap(msg.addMapPlayer.NewPlayer);
        }

    }
    public void DoRemoveMapPlayer(ProtoMsg msg)
    {
        MainCitySys.Instance.RemovePlayer(msg.removeMapPlayer);
    }
    public void DoMapInformation(ProtoMsg msg)
    {
        MainCitySys.Instance.ProcessMapInformation(msg.mapInformation);
    }
    public void DoAddMapPlayer(ProtoMsg msg)
    {
        if (msg.addMapPlayer.MapID == GameRoot.Instance.ActivePlayer.MapID)
        {
            MainCitySys.Instance.AddPlayerToMap(msg.addMapPlayer.NewPlayer);
        }
    }
    public void DoAddProperty(ProtoMsg msg)
    {
        AddPropertyPoint ap = msg.addPropertyPoint;
        Player player = GameRoot.Instance.ActivePlayer;
        player.Att += ap.Att;
        player.Strength += ap.Strength;
        player.Agility += ap.Agility;
        player.Intellect += ap.Intellect;
        player.RestPoint = ap.RestPoint;
        UISystem.Instance.InfoWnd.RefreshIInfoUI();
    }

    public void DoEnterMiniGame(ProtoMsg msg)
    {
        MainCitySys.Instance.EnterMiniGame(msg.miniGameRsp);
    }

    public void DoMiniGameScoreRsp(ProtoMsg msg)
    {
        MiniGameScoreRsp rsp = msg.miniGameScoreRsp;
        GameRoot.Instance.ActivePlayer.SwordPoint = rsp.SwordPoint;
        GameRoot.Instance.ActivePlayer.ArcheryPoint = rsp.ArcheryPoint;
        GameRoot.Instance.ActivePlayer.MagicPoint = rsp.MagicPoint;
        GameRoot.Instance.ActivePlayer.TheologyPoint = rsp.TheologyPoint;
        GotoMiniGame.Instance.ranking = rsp.MiniGameRanking;
        print(GameRoot.Instance.ActivePlayer.SwordPoint);
        print(GameRoot.Instance.ActivePlayer.ArcheryPoint);
        print(GameRoot.Instance.ActivePlayer.MagicPoint);
        print(GameRoot.Instance.ActivePlayer.TheologyPoint);
    }
    public void DoChatRsp(ProtoMsg msg)
    {
        UISystem.Instance.chatWnd.AddChatMsg(msg);
    }
    public void DoGenerateMonsters(ProtoMsg msg)
    {
        MonsterGenerate mg = msg.monsterGenerate;
        if (mg.MonsterId != null)
        {
            foreach (var key in mg.MonsterId.Keys)
            {
                Tools.Log("GenMon");
                BattleSys.Instance.AddMonster(key, mg.MonsterId[key], mg.MonsterPos[key]);
            }
        }
    }
    public void DoHurtMonsters(ProtoMsg msg)
    {
        MonsterGetHurt hurt = msg.monsterGetHurt;
        BattleSys.Instance.MonsterGetHurt(hurt);
    }
    public void DoMonsterDeath(ProtoMsg msg)
    {
        /*
        MonsterDeath death = msg.monsterDeath;
        BattleSys.Instance.MonsterDeath(death);
        */
    }
    public void DoExpPacket(ProtoMsg msg)
    {
        ExpPacket exp = msg.expPacket;
        if (exp.CharacterName == GameRoot.Instance.ActivePlayer.Name)
        {
            UISystem.Instance.baseUI.AddExp(exp.Exp);
        }
    }
    public void DoLevelUpPacket(ProtoMsg msg)
    {
        LevelUp levelUp = msg.levelUp;
        if (levelUp.CharacterName == GameRoot.Instance.ActivePlayer.Name)
        {
            UISystem.Instance.baseUI.ProcessLevelUpMsg(levelUp.RestExp);
        }
    }
    public void DoWeatherPacket(ProtoMsg msg)
    {
        WeatherPacket weather = msg.weatherPacket;
        MainCitySys.Instance.UpdateWeather(weather.weatherType);
    }
    public void DoUpdateHPMP(ProtoMsg msg)
    {
        UpdateHpMp update = msg.updateHpMp;
        if (update.UpdateHp != GameRoot.Instance.ActivePlayer.HP)
        {
            UISystem.Instance.InfoWnd.UpdateHp(update.UpdateHp);
        }
        if (update.UpdateMp != GameRoot.Instance.ActivePlayer.MP)
        {
            UISystem.Instance.InfoWnd.UpdateMp(update.UpdateMp);
        }

    }
    public void DoPlayerGetHurt(ProtoMsg msg)
    {

        PlayerGetHurt playerGetHurt = msg.playerGetHurt;
        if (playerGetHurt.CharacterName == GameRoot.Instance.ActivePlayer.Name)
        {
            //自己受傷
            if (GameRoot.Instance.ActivePlayer.HP - playerGetHurt.damage > 0)
            {
                //不會死的受傷               
                //GameRoot.Instance.MainPlayerControl.ProcessGetHurt(playerGetHurt.damage, playerGetHurt.hurtType, playerGetHurt.MonsterID);
            }
            else
            {
                //會死，有問題
                print("血量算錯");
            }
        }
        else
        {
            //別人受傷

        }
    }
    public void DoPlayerDeath(ProtoMsg msg)
    {
        /*
        PlayerDeath death = msg.playerDeath;
        print("收到死亡封包" + death.CharacterName);
        if (death.CharacterName == GameRoot.Instance.ActivePlayer.Name)
        {
            //自己死掉
            GameRoot.Instance.PlayerControl.Death();
        }
        else
        {
            
            //別人死掉
            if (GameRoot.Instance.otherPlayers.ContainsKey(death.CharacterName))
            {
                if (GameRoot.Instance.otherPlayers[death.CharacterName] != null)
                {
                    OtherPlayerTask task = new OtherPlayerTask(GameRoot.Instance.otherPlayers[death.CharacterName],2,death.FaceDir);
                    GameRoot.Instance.otherPlayers[death.CharacterName].Actions.Enqueue(task);
                }
                
            }
            
        }
        */
    }
    public void DoPlayerAction(ProtoMsg msg)
    {
        PlayerAction action = msg.playerAction;


    }
    public void DoKnapsackOperation(ProtoMsg msg)
    {
        KnapsackOperation ko = msg.knapsackOperation;
        if (ko.ErrorType == 0)
        {
            Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
            Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
            if (nk == null)
            {
                GameRoot.Instance.ActivePlayer.NotCashKnapsack = new Dictionary<int, Item>();
                nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
            }
            if (ck == null)
            {
                GameRoot.Instance.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();
                ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
            }
            try
            {
                switch (ko.OperationType)
                {
                    case 1:
                    case 2:
                    case 3:
                        for (int i = 0; i < ko.items.Count; i++)
                        {
                            if (!ko.items[i].IsCash)
                            {
                                if (!nk.ContainsKey(ko.NewPosition[i]))
                                {
                                    nk.Add(ko.NewPosition[i], ko.items[i]);
                                }
                                else
                                {
                                    nk[ko.NewPosition[i]] = ko.items[i];
                                }
                                KnapsackWnd.Instance.FindSlot(ko.NewPosition[i]).StoreItem(ko.items[i], ko.items[i].Count);
                            }
                            else
                            {
                                if (!ck.ContainsKey(ko.NewPosition[i]))
                                {
                                    ck.Add(ko.NewPosition[i], ko.items[i]);
                                }
                                else
                                {
                                    ck[ko.NewPosition[i]] = ko.items[i];
                                }
                                KnapsackWnd.Instance.FindCashSlot(ko.NewPosition[i]).StoreItem(ko.items[i], ko.items[i].Count);
                            }
                        }
                        break;
                    case 4:
                        KnapsackWnd.Instance.ProcessKnapsackExchage(ko);
                        break;
                    case 5:
                        if (ko.items.Count > 0)
                        {
                            foreach (var item in ko.items)
                            {
                                if (!item.IsCash)
                                {
                                    if (nk.ContainsKey(item.Position))
                                    {
                                        nk.Remove(item.Position);
                                        GameObject.Destroy(KnapsackWnd.Instance.FindSlot(item.Position).GetComponentInChildren<ItemUI>());
                                    }
                                }
                                else
                                {
                                    if (ck.ContainsKey(item.Position))
                                    {
                                        ck.Remove(item.Position);
                                        GameObject.Destroy(KnapsackWnd.Instance.FindSlot(item.Position).GetComponentInChildren<ItemUI>());
                                    }
                                }
                            }
                        }
                        break;
                    case 6:
                        if (ko.items.Count > 0)
                        {
                            foreach (var item in ko.items)
                            {
                                if (!item.IsCash)
                                {
                                    nk[item.Position] = item;
                                    KnapsackWnd.Instance.FindSlot(item.Position).StoreItem(item, item.Count);
                                }
                                else
                                {
                                    ck[item.Position] = item;
                                    KnapsackWnd.Instance.FindCashSlot(item.Position).StoreItem(item, item.Count);
                                }
                            }
                            GameRoot.Instance.ActivePlayer.Ribi -= ko.Ribi;
                        }
                        break;
                }
            }
            catch (System.Exception e)
            {
                Tools.Log(e.Message);
            }
        }
        else
        {
            GameRoot.AddTips(ko.ErrorMessage);
        }
    }

    public void DoEquipmentOperation(ProtoMsg msg)
    {
        PlayerEquipments eq = GameRoot.Instance.ActivePlayer.playerEquipments;
        if (eq == null)
        {
            eq = new PlayerEquipments();
        }
        EquipmentOperation eo = msg.equipmentOperation;
        UISystem.Instance.equipmentWnd.ProcessEquipmentOperation(eo);
    }

    public void DoRecycleItems(ProtoMsg msg)
    {
        InventorySys.Instance.ProcessRecycleItem(msg.recycleItems);
    }

    public void DoMiniGameSetting(ProtoMsg msg)
    {
        MainCitySys.Instance.SetMiniGameSchedule(msg.miniGameSetting);
    }

    public void DoRewards(ProtoMsg msg)
    {
        if (msg.rewards.Character == GameRoot.Instance.ActivePlayer.Name)
        {
            InventorySys.Instance.RecieveRewards(msg.rewards);
        }
    }

    public void DoCashShopResponse(ProtoMsg msg)
    {
        CashShopWnd.Instance.ProcessCashShopResponse(msg.cashShopResponse);
    }


    public void DoTransactionResponse(ProtoMsg msg)
    {
        TransationWnd.Instance.ProessTransactionResponse(msg.transactionResponse);
    }
    #endregion

    public void DoLockerOperation(ProtoMsg msg)
    {
        LockerWnd.Instance.ProcessLockerOperation(msg.lockerOperation);
    }

    public void DoMailBoxOperation(ProtoMsg msg)
    {
        MailBoxWnd.Instance.ProcessMailBoxOperation(msg.mailBoxOperation);
    }
    public void DoStrengthenResponse(ProtoMsg msg)
    {
        StrengthenWnd.Instance.ProessStrengthenReaponse(msg.strengthenResponse);
    }
    public void DoLearnSkill(ProtoMsg msg)
    {
        UISystem.Instance.learnSkillWnd.ProcessLearnSkillResponse(msg);
    }

    public void DoHotKeyOperation(ProtoMsg msg)
    {
        BattleSys.Instance.HotKeyManager.ProcessHotKeyOperation(msg);
    }
    public void DoSyncEntities(ProtoMsg msg)
    {
        EntitySyncRequest es = msg.entitySyncReq;
        for (int i = 0; i < es.nEntity.Count; i++)
        {
            BattleSys.Instance.UpdateEntity(es.entityEvent[i], es.nEntity[i]);
        }
    }
    public void DoSkillCastResponse(ProtoMsg msg)
    {
        BattleSys.Instance.ProcessSkillCastResponse(msg);
    }

    public void DoSkillHitResponse(ProtoMsg msg)
    {
        BattleSys.Instance.ProcessSkillHitResponse(msg);
    }
}
