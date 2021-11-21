using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PEProtocal;

public class BuffManager
{
    EntityController owner;
    public Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();
    public BuffManager(EntityController controller)
    {
        this.owner = controller;
    }

    internal Buff AddBuff(int buffID, int BuffType,SkillCasterType CasterType, string CasterName = "", int CasterID = -1)
    {
        BuffDefine buffDefine = null;
        if(ResSvc.Instance.BuffDic.TryGetValue(BuffType, out buffDefine))
        {
            Buff buff = new Buff(this.owner, buffID, buffDefine, CasterType, CasterName, CasterID);
            this.Buffs[buffID] = buff;
            Debug.Log("新增Buff: " + buffDefine.BuffName);
            if (this.owner is PlayerController && this.owner.entity.nentity.EntityName == GameRoot.Instance.ActivePlayer.Name)
            {
                BattleSys.Instance.AddBuffIcon(buffID, Resources.Load<Sprite>("Effect/SkillIcon/" + buffDefine.Icon), buffDefine.Duration);
            }
            return buff;
        }
        
        return null;
    }

    internal Buff RemoveBuff(int buffID)
    {
        Buff buff;
        if(this.Buffs.TryGetValue(buffID, out buff))
        {
            buff.OnRemove();
            this.Buffs.Remove(buffID);
            Debug.Log("外部移除Buff: " + buff.Define.BuffName);
            return buff;
        }
        return null;
    }

    internal void OnUpdate(float delta)
    {
        List<int> needRemove = new List<int>();
        foreach (var kv in this.Buffs)
        {
            kv.Value.OnUpdate(delta);
            if (kv.Value.Stopped)
            {
                needRemove.Add(kv.Key);
            }
        }
        if (needRemove.Count > 0)
        {
            foreach (var buffid in needRemove)
            {
                this.owner.RemoveBuff(buffid);
            }
        }
    }

    public bool IsBuffValid(int BuffType)
    {
        if (Buffs.Count == 0) return false;
        foreach (var buff in Buffs)
        {
            if (buff.Value.Define.ID == BuffType && (!buff.Value.Stopped)) return true;
        }
        return false;
    }
    
}
