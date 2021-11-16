using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using System;

public class Buff
{
    public int BuffID;
    private EntityController Owner;
    public BuffDefine Define;
    private SkillCasterType CasterType;
    private int CasterID = -1;
    private string CasterName = "";
    public bool Stopped = false;
    private float time = 0;
    public Buff(EntityController owner, int buffID, BuffDefine buffDefine, SkillCasterType CasterType, string CasterName, int CasterID)
    {
        this.Owner = owner;
        this.BuffID = buffID;
        this.Define = buffDefine;
        this.CasterType = CasterType;
        this.CasterID = CasterID;
        this.CasterName = CasterName;
        this.OnAdd();
    }

    private void OnAdd()
    {
        if(this.Define.BuffState != BUFF_Effect.NONE)
        {
            this.Owner.AddBuffEffect(this.Define.BuffState);
        }
        AddAttr();
    }

    public void OnRemove()
    {
        RemoveAttr();
        Stopped = true;
        if(this.Define.BuffState != BUFF_Effect.NONE)
        {
            this.Owner.RemoveBuffEffect(this.Define.BuffState);
        }
    }

    private void AddAttr()
    {
        if(this.Owner is PlayerController)
        {
            if(this.Owner.entity.entityData.EntityName == GameRoot.Instance.ActivePlayer.Name)
            {
                BattleSys.Instance.InitBuffAttribute(this.Define.AttributeGain, "add");
                BattleSys.Instance.InitFinalAttribute();
            }
            else //其他人不鳥他們?
            {
                return;
            }
        }
        else //怪物
        {

        }
    }

    private void RemoveAttr()
    {
        if (this.Owner is PlayerController)
        {
            if (this.Owner.entity.entityData.EntityName == GameRoot.Instance.ActivePlayer.Name)
            {
                BattleSys.Instance.InitBuffAttribute(this.Define.AttributeGain, "minus");
                BattleSys.Instance.InitFinalAttribute();
            }
            else //其他人不鳥他們?
            {
                return;
            }
        }
        else //怪物
        {

        }
    }

    internal void OnUpdate(float delta)
    {
        if (Stopped) return;
        this.time += delta;
        if(time > this.Define.Duration)
        {
            this.OnRemove();
        }
    }
}
