using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;
public class SkillManager
{
    Entity Owner;
    public Dictionary<int, Skill> ActiveSkills { get; private set; }
    public Dictionary<int, Skill> NegativeSkills { get; private set; }
    public SkillManager(Entity owner)
    {
        this.Owner = owner;
        this.ActiveSkills = new Dictionary<int, Skill>();
        this.NegativeSkills = new Dictionary<int, Skill>();
        this.InitSkills();
    }
    private void InitSkills()
    {
        this.ActiveSkills.Clear();
        this.NegativeSkills.Clear();
        if (Owner is MOFCharacter)
        {
            MOFCharacter chr = (MOFCharacter)Owner;
            if (chr.player.Skills != null && chr.player.Skills.Count > 0)
            {
                foreach (var skillInfo in chr.player.Skills)
                {
                    Skill skill = new Skill(skillInfo.Value.SkillID, skillInfo.Value.SkillLevel, this.Owner);
                    if (skill.Info.IsActive)
                    {
                        this.ActiveSkills.Add(skill.Info.SkillID, skill);
                    }
                    else
                    {
                        this.NegativeSkills.Add(skill.Info.SkillID, skill);
                    }
                }
            }
        }
        else if (Owner is AbstractMonster)//怪物技能
        {

        }
    }
    public void AddSkill(Skill skill)
    {
        this.ActiveSkills.Add(skill.Info.SkillID, skill);
    }
    public Skill GetNegativeSkill(int SkillID)
    {
        if (this.NegativeSkills.ContainsKey(SkillID))
        {
            return NegativeSkills[SkillID];
        }
        LogSvc.Error("該對象無此被動技能");
        return null;
    }
    public Skill GetSkill(int SkillID)
    {
        if (this.ActiveSkills.ContainsKey(SkillID))
        {
            return ActiveSkills[SkillID];
        }
        LogSvc.Error("該對象無此技能");
        return null;
    }
    public void Update()
    {
        if (ActiveSkills.Count < 1)
        {
            return;
        }
        foreach (var skill in ActiveSkills.Values)
        {
            skill.Update();
        }
    }
}

