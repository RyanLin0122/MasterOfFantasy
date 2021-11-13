using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class LearnSkillHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        LearnSkill req = msg.learnSkill;
        if (req == null)
        {
            return;
        }
        SkillInfo info = CacheSvc.Instance.SkillDic[req.SkillID];
        int SkillLevel = req.Level;
        Player player = session.ActivePlayer;
        if (player.SwordPoint >= info.SwordPoint[SkillLevel - 1] && player.ArcheryPoint >= info.ArcheryPoint[SkillLevel - 1]
            && player.MagicPoint >= info.MagicPoint[SkillLevel - 1] && player.TheologyPoint >= info.MagicPoint[SkillLevel - 1] && player.Level >= info.RequiredLevel[SkillLevel - 1])
        {
            //成功
            req.IsSuccess = true;
            if (session.ActivePlayer.Skills.ContainsKey(req.SkillID))
            {
                session.ActivePlayer.Skills[req.SkillID].SkillLevel += 1;
                session.ActivePlayer.SwordPoint -= info.SwordPoint[SkillLevel - 1];
                session.ActivePlayer.ArcheryPoint -= info.ArcheryPoint[SkillLevel - 1];
                session.ActivePlayer.MagicPoint -= info.MagicPoint[SkillLevel - 1];
                session.ActivePlayer.TheologyPoint -= info.TheologyPoint[SkillLevel - 1];
            }
            else
            {
                session.ActivePlayer.Skills.Add(req.SkillID, new SkillData { SkillID = req.SkillID, SkillLevel = 1 });
                session.ActivePlayer.SwordPoint -= info.SwordPoint[0];
                session.ActivePlayer.ArcheryPoint -= info.ArcheryPoint[0];
                session.ActivePlayer.MagicPoint -= info.MagicPoint[0];
                session.ActivePlayer.TheologyPoint -= info.TheologyPoint[0];
                MOFCharacter character = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name];
                Skill skill = new Skill(req.SkillID, 1, character);
                if (info.IsActive)
                {
                    character.skillManager.ActiveSkills.Add(req.SkillID, skill);
                }
                else
                {
                    character.skillManager.NegativeSkills.Add(req.SkillID, skill);
                }
                character.InitNegativeAttribute(character.skillManager.NegativeSkills);
                character.InitBuffAttribute();
                character.InitFinalAttribute();
            }
            session.WriteAndFlush(msg);
        }
        else
        {
            //失敗
            req.IsSuccess = false;
            session.WriteAndFlush(msg);
        }
    }
}
