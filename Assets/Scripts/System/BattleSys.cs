using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using Random = UnityEngine.Random;
using System;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance = null;
    public Dictionary<int, MonsterAI> Monsters;
    public Canvas MapCanvas = null;

    public MonsterAI CurrentTarget = null;
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        Monsters = new Dictionary<int, MonsterAI>();
        Debug.Log("Init BattleSys...");
    }

    public void AddMonster(int MapMonsterID, int MonsterID, float[] pos)
    {
        GameObject mon = Instantiate(Resources.Load("Prefabs/Enemy") as GameObject);
        mon.transform.SetParent(MapCanvas.transform);
        mon.transform.localPosition = new Vector3(pos[0], pos[1], 0f);
        if (Monsters.ContainsKey(MapMonsterID))
        {
            Monsters[MapMonsterID] = mon.GetComponent<MonsterAI>();
        }
        else
        {
            Monsters.Add(MapMonsterID, mon.GetComponent<MonsterAI>());
        }
        Monsters[MapMonsterID].Init(ResSvc.Instance.MonsterInfoDic[MonsterID], MapMonsterID);
    }

    public void CommonAttack(int MapMonsterID)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        int damage = Random.Range(5, 15);
        bool isCritical = false;
        float probibility = Random.Range(0f, 1f);
        if (probibility >= 0.8)
        {
            isCritical = true;
            damage *= 2;
        }
        bool FaceDir = false;
        if (GameRoot.Instance.MainPlayerControl.transform.localScale.x >= 0)
        {
            FaceDir = true;
        }
        float Delay = 0.5f;
        int AnimType = Constants.GetCommonAttackAnimID(WeaponType.Bow);
        AttackType type = AttackType.Common;
        AttackMonster(MapMonsterID, damage, isCritical, type, Delay, AnimType, FaceDir);
    }
    public void AttackMonster(int MapMonsterID, int damage, bool IsCritical, AttackType attackType, float Delay, int AnimType, bool FaceDir)
    {
        Dictionary<int, int> monsterMapID = new Dictionary<int, int>();
        monsterMapID.Add(MapMonsterID, MapMonsterID);
        Dictionary<int, int[]> damages = new Dictionary<int, int[]>();
        damages.Add(MapMonsterID, new int[] { damage });
        Dictionary<int, bool> isCritical = new Dictionary<int, bool>();
        isCritical.Add(MapMonsterID, IsCritical);
        Dictionary<int, AttackType> attackTypes = new Dictionary<int, AttackType>();
        attackTypes.Add(MapMonsterID, attackType);
        new DamageSender(monsterMapID, damages, isCritical, attackTypes, AnimType, FaceDir, Delay);
    }


    public void AttackMonsters(int[] MapMonsterID, List<int[]> Damage, bool[] IsCritical, AttackType[] attackType)
    {
        //To-Do
    }

    public void MonsterGetHurt(MonsterGetHurt gh)
    {
        foreach (var ID in gh.MonsterMapID.Keys) //遍歷怪物
        {
            if (Monsters.ContainsKey(ID) && Monsters[ID] != null)
            {
                if (!Monsters[ID].IsReadyDeath)
                {
                    if (gh.IsDeath[ID])
                    {
                        Monsters[ID].ReadyToDeath();
                    }
                    int totalDamage = 0;
                    foreach (var damage in gh.Damage[ID])
                    {
                        totalDamage += damage;
                    }
                    Monsters[ID].MinusHP(totalDamage);
                    if (gh.IsCritical[ID] == false) //不是爆擊
                    {
                        TimerSvc.Instance.AddTimeTask((a) =>
                        {
                            Monsters[ID].GenerateAttackEffect();
                            Monsters[ID].GenerateDamageNum(totalDamage, 0);
                            Monsters[ID].PlayHitSound();
                            Monsters[ID].HurtMonster();
                        }, gh.Delay, PETimeUnit.Second, 1);
                        if (gh.CharacterName == GameRoot.Instance.ActivePlayer.Name)
                        {
                            //自己打
                            Monsters[ID].SetTargetPlayer(GameRoot.Instance.MainPlayerControl);
                        }
                        else
                        {
                            //其他人打的 只處理播動畫
                            if (GameRoot.Instance.otherPlayers.ContainsKey(gh.CharacterName))
                            {
                                if (GameRoot.Instance.otherPlayers[gh.CharacterName] != null)
                                {
                                    OtherPeopleCtrl ctrl = GameRoot.Instance.otherPlayers[gh.CharacterName];
                                    OtherPlayerTask task = new OtherPlayerTask(ctrl, gh.AnimID, gh.FaceDir);
                                    ctrl.Actions.Enqueue(task);
                                }
                            }
                        }
                    }
                    else //爆擊
                    {
                        TimerSvc.Instance.AddTimeTask((a) =>
                        {
                            Monsters[ID].GenerateAttackEffect();
                            Monsters[ID].GenerateDamageNum(totalDamage, 1);
                            Monsters[ID].PlayHitSound();
                            Monsters[ID].HurtMonster();
                        }, gh.Delay, PETimeUnit.Second, 1);
                        if (gh.CharacterName == GameRoot.Instance.ActivePlayer.Name)
                        {
                            //自己打
                            Monsters[ID].SetTargetPlayer(GameRoot.Instance.MainPlayerControl);
                        }
                        else
                        {
                            //其他人打的
                            if (GameRoot.Instance.otherPlayers.ContainsKey(gh.CharacterName))
                            {
                                if (GameRoot.Instance.otherPlayers[gh.CharacterName] != null)
                                {
                                    OtherPeopleCtrl ctrl = GameRoot.Instance.otherPlayers[gh.CharacterName];
                                    OtherPlayerTask task = new OtherPlayerTask(ctrl, gh.AnimID, gh.FaceDir);
                                    ctrl.Actions.Enqueue(task);
                                }
                            }
                        }
                    }
                    
                }
            }

        }
    }

    public void AddPlayerAction()
    {

    }

    public void MonsterDeath(MonsterDeath md)
    {
        if (md.MonsterID != null)
        {
            foreach (var ID in md.MonsterID)
            {
                if (Monsters.ContainsKey(ID))
                {
                    Monsters[ID].MonsterDeath();
                    if (CurrentTarget == Monsters[ID])
                    {
                        CurrentTarget = null;
                    }
                }
            }
        }

    }

    public void LockTarget(MonsterAI monAi)
    {
        CurrentTarget = monAi;
    }

    public void ClearTarget()
    {
        CurrentTarget = null;
    }

    public void ClearMonsters()
    {
        Monsters = new Dictionary<int, MonsterAI>();
    }
    public void SetupMonsters(Dictionary<int, SerializedMonster> mons)
    {
        if (mons.Count > 0)
        {
            print("怪量:" + mons.Count);
            foreach (var id in mons.Keys)
            {
                AddMonster(id, mons[id].MonsterID, mons[id].Position);
                Monsters[id].hp = mons[id].HP;
                Monsters[id].SetHpBar();
                Monsters[id].TrySetTargetPlayer(mons[id].Targets);
                mons[id].status = mons[id].status;
            }
        }

    }

    public void RefreshMonster()
    {
        if (MapCanvas != null)
        {
            MonsterAI[] ais = MapCanvas.GetComponentsInChildren<MonsterAI>();
            if (ais.Length > 0)
            {
                Monsters.Clear();
                foreach (var ai in ais)
                {
                    Monsters.Add(ai.MapMonsterID, ai);
                }
            }
        }
    }

    public void ClearBugMonster()
    {
        if(MapCanvas != null)
        {
            MonsterAI[] ais = MapCanvas.GetComponentsInChildren<MonsterAI>();
            if (ais.Length > 0)
            {
                ais[0].GetComponent<NodeCanvas.Framework.Blackboard>().SetVariableValue("IsDeath",true);
            }
        }
    }
}

