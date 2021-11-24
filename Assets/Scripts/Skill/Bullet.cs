using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Bullet
{
    Skill skill;
    int hit = 0;

    float flyTime = 0;
    float duration = 0;

    public bool Stopped = false;
    private ActiveSkillInfo active;
    Transform BulletGameObject = null;
    EntityController Target = null;

    public Bullet(Skill skill, EntityController Target, ActiveSkillInfo active)
    {
        this.skill = skill;
        this.Target = Target;
        this.hit = skill.Hit;
        this.active = active;
        float distance = GetDistance(skill.Owner.transform.position, Target.transform.position);
        duration = distance / active.BulletSpeed;
        Debug.Log("duration: " + duration);
        Debug.Log("distance: " + distance);
        InstantiateBullet();
    }
    public float GetDistance(Vector3 OwnerPos, Vector3 TargetPos)
    {
        return Mathf.Sqrt(Mathf.Pow(OwnerPos.x - TargetPos.x, 2) + Mathf.Pow(OwnerPos.y - TargetPos.y, 2));
    }

    public void Update()
    {
        if (Stopped) return;

        if (BulletGameObject != null && flyTime < duration)
        {
            float OriginalZ = BulletGameObject.position.z;
            float RestTime = duration - flyTime;
            Vector3 Distance = Target.transform.position - BulletGameObject.position;
            float cosine = Vector2.Dot(Vector2.right, Distance / ((Vector2)Distance).magnitude);
            float theta = Mathf.Acos(cosine) * 180 / Mathf.PI;
            if (Distance.y < 0) theta *= -1;
            BulletGameObject.position = BulletGameObject.position + Distance * (Time.deltaTime / RestTime);
            BulletGameObject.position = new Vector3(BulletGameObject.position.x, BulletGameObject.position.y, OriginalZ);
            BulletGameObject.localRotation = Quaternion.Euler(0, 0, theta);
        }
        flyTime += Time.deltaTime;
        if (this.flyTime > duration)
        {
            Debug.Log("¤l¼u©R¤¤");
            this.skill.DoHitDamages(this.hit);
            this.Stopped = true;
            GameObject.Destroy(BulletGameObject.gameObject);
        }
    }

    public void InstantiateBullet()
    {
        if (this.active != null && this.active.IsShoot)
        {
            if (active.AniPath["Shoot"] != "")
            {
                Transform go = ((GameObject)GameObject.Instantiate(Resources.Load("Prefabs/SkillPrefabs/" + active.AniPath["Shoot"]))).GetComponent<Transform>();
                go.SetParent(skill.Owner.transform.parent);
                go.position = skill.Owner.transform.position;
                if (go != null)
                {
                    this.BulletGameObject = go;
                }
            }
        }
    }
}
