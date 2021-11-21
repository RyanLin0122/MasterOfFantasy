using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class EntityController : MonoBehaviour
{
    public string Name;
    public Rigidbody2D rb;
    public Transform Shadow;
    public bool IsMoving = false;
    public Entity entity;
    public bool IsAIControlling = false;
    public Dictionary<int, Skill> SkillDict;
    public BuffManager buffManager;
    public EffectManager effectManager;
    public Player playerdata;
    public TrimedPlayer trimedPlayerdata;
    public void Init()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        this.buffManager = new BuffManager(this);
        this.effectManager = new EffectManager(this);
    }
    public virtual void SetFaceDirection(bool FaceDir)
    {
        //子類實現
    }
    public virtual void OnEntityEvent(EntityEvent entityEvent)
    {
        //子類實現
    }
    public virtual void DoDamage(DamageInfo damage, ActiveSkillInfo active)
    {
        //子類實現
        int NumberCount = damage.Damage.Length;
        
        if (NumberCount > 0)
        {
            int[] t = new int[NumberCount];
            for (int i = 0; i < NumberCount; i++)
            {
                MinusNEntityHP(damage.Damage[i]);
                if (i > 0)
                {
                    int index = i;
                    int num = damage.Damage[i];
                    if (this is MonsterController)
                    {
                        TimerSvc.Instance.AddTimeTask((a) =>
                        {
                            GenerateDamageNum(num, 0);
                        }, 300f * index, PETimeUnit.Millisecond, 1);
                    }
                    else
                    {
                        TimerSvc.Instance.AddTimeTask((a) =>
                        {
                            GenerateDamageNum(num, 2);
                        }, 300f * index, PETimeUnit.Millisecond, 1);
                    }
                }
                else
                {
                    if(this is MonsterController)
                    {
                        GenerateDamageNum(damage.Damage[i], 0);
                    }
                    else
                    {
                        GenerateDamageNum(damage.Damage[i], 2);
                    }
                } 
                    
            }
        }
        PlayHitAni(active);
    }
    public void MinusNEntityHP(int num)
    {
        entity.nEntity.HP = (int)Mathf.Clamp(entity.nEntity.HP - num, 0, entity.nEntity.HP);
    }
    public virtual void DoBuffDamage(DamageInfo damage, BuffDefine buffDefine)
    {
        Debug.Log("Do Buff Damage");
        //To do

    }
    public virtual void PlayHitAni(ActiveSkillInfo active)
    {
        //子類實現
    }           
    public void DoSkillHit(SkillHitInfo hit)
    {
        if (SkillDict == null) return;
        Skill skill = null;
        SkillDict.TryGetValue(hit.SkillID, out skill);
        if (skill != null) skill.DoHit(hit);
    }
    internal void DoBuffAction(BuffInfo buff)
    {
        switch (buff.Action)
        {
            case BUFF_Action.NONE:
                break;
            case BUFF_Action.ADD:
                this.AddBuff(buff.BuffID, buff.BuffDefineID, buff.CastType, buff.CasterName, buff.CasterID);
                break;
            case BUFF_Action.REMOVE:
                this.RemoveBuff(buff.BuffID);
                break;
            case BUFF_Action.HIT:
                this.DoBuffDamage(buff.DamageInfo, ResSvc.Instance.BuffDic[buff.BuffDefineID]);
                break;
            default:
                break;
        }
    }
    private Buff AddBuff(int BuffID, int BuffDefinID, SkillCasterType CasterType, string CasterName = "", int CasterID = -1)
    {
        if (this.buffManager != null) return this.buffManager.AddBuff(BuffID, BuffDefinID, CasterType, CasterName, CasterID);
        return null;
    }

    public Buff RemoveBuff(int BuffID)
    {
        if (this.buffManager != null) return this.buffManager.RemoveBuff(BuffID);
        return null;
    }
    internal void AddBuffEffect(BUFF_Effect buffEffect)
    {
        this.effectManager.AddBuffEffect(buffEffect);
    }

    internal void RemoveBuffEffect(BUFF_Effect buffEffect)
    {
        this.effectManager.RemoveEffect(buffEffect);
    }
    public virtual void Update()
    {
        if (this.SkillDict != null && this.SkillDict.Count > 0)
        {
            foreach (var skill in this.SkillDict.Values)
            {
                skill.Update(Time.deltaTime);
            }
        }
        if (this.buffManager != null)
        {
            this.buffManager.OnUpdate(Time.deltaTime);
        }
    }


    #region Generate Number
    public GameObject DamageContainer;
    public void GenerateDamageNum(int damage, int mode)
    {
        //DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
        GameObject container = Instantiate(Resources.Load("Prefabs/NumberContainer") as GameObject);
        container.transform.SetParent(BattleSys.Instance.MapCanvas.transform);
        container.transform.localScale = new Vector3(100, 100, 1f);
        container.transform.localPosition = transform.localPosition;

        GameObject obj = Instantiate(Resources.Load("Prefabs/Damage") as GameObject);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.transform.SetParent(container.transform);
        obj.GetComponent<DamageController>().SetNumber(damage, mode);
    }
    #endregion
}
