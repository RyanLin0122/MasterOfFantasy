using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class Bullet
{
    private Skill skill;
    private Entity target;
    private SkillHitInfo hitInfo;
    private ActiveSkillInfo active;
    bool TimeMode = true;

    float flyTime = 0;
    float duration = 0;

    NVector3 pos = new NVector3(0, 0, 0);

    public bool Stopped = false;
    public float speed = 0;
    public Bullet(Skill skill, Entity target, ActiveSkillInfo active, SkillHitInfo hitInfo)
    {
        this.skill = skill;
        this.target = target;
        this.hitInfo = hitInfo;
        this.active = active;
        this.speed = active.BulletSpeed;
        double distance = skill.Owner.Distance(target.nEntity.Position);
        if (TimeMode)
        {
            duration = (float)distance / active.BulletSpeed;
        }
        //Console.WriteLine("發射子彈 Target: {0} Distance: {1}, Duration: {2}", target.nEntity.Id, distance, duration);
    }

    public void Update()
    {
        if (Stopped) return;
        if (TimeMode) this.UpdateTime();
        else this.UpdatePos();
    }

    public void UpdateTime()
    {
        this.flyTime += Time.deltaTime;
        if (this.flyTime > duration)
        {
            this.hitInfo.IsBullet = true;
            this.skill.DoHit();
            this.Stopped = true;
        }
    }

    public void UpdatePos()
    {
        float radius = 50;
        double distance = skill.Owner.Distance(target.nEntity.Position);
        if (distance > radius)
        {
            float delta = speed * Time.deltaTime;
            pos = new NVector3(pos.X + delta, pos.Y + delta, pos.Z);
        }
        else
        {
            this.hitInfo.IsBullet = true;
            this.skill.DoHit(this.hitInfo, active);
            this.Stopped = true;
        }
    }
}

