using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class AbstractMonster : Entity
{
    public int MonsterID { get; set; }
    public MonsterStatus laststatus = MonsterStatus.Death;
    public MonsterStatus status = MonsterStatus.Death;
    public Dictionary<string, int> PlayerDamageRecord = new Dictionary<string, int>();
    public MOFCharacter AttackTarget;
    public override void OnDeath()
    {
        this.status = MonsterStatus.Death;
        LogSvc.Info("怪物死了");
    }
    public override void DoDamage(DamageInfo damage, string CasterName = "")
    {
        int AccumulateDamage = 0;
        foreach (var num in damage.Damage)
        {
            int HP = this.nEntity.HP - num;
            if (HP <= 0)
            {
                AccumulateDamage += this.nEntity.HP;
                HP = 0;
                this.nEntity.HP = HP;
                if (CasterName != "") AddDamgageRecord(CasterName,AccumulateDamage);
                OnDeath();
                return;
            }
            else
            {
                AccumulateDamage += num;
                this.nEntity.HP = HP;
            }
        }
        if (CasterName != "") AddDamgageRecord(CasterName, AccumulateDamage);
    }
    public void AddDamgageRecord(string Name, int damage)
    {
        int ac = 0;
        this.PlayerDamageRecord.TryGetValue(Name, out ac);
        ac += damage;
        this.PlayerDamageRecord[Name] = ac;
    }}



