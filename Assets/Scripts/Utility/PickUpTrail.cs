using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrail : MonoBehaviour
{
    public bool HasInit = false;
    public bool IsFirst = false;
    public float Speed = 700;
    public EntityController Target;
    public bool Stopped = false;
    public void Init(EntityController Target, bool IsFirst)
    {
        this.Target = Target;
        this.IsFirst = IsFirst;

        HasInit = true;
    }
    public void FixedUpdate()
    {
        if (!HasInit) return;
        if (!Stopped)
        {
            this.Speed += 1;
            float Distance = GetDistance(transform.localPosition, Target.transform.localPosition);
            if (Distance > 30)
            {
                float OriginalZ = transform.localPosition.z;
                Vector2 DirectionVector = new Vector2(Target.transform.localPosition.x - transform.localPosition.x, Target.transform.localPosition.y - transform.localPosition.y);
                transform.localPosition = (Vector2)transform.localPosition + this.Speed / 50 * DirectionVector.normalized;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, OriginalZ);
            }
            else
            {
                this.Stopped = true;
                if (IsFirst)
                {
                    Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/GetItem_Effect_F"), transform.parent)).GetComponent<Transform>();
                    go.localPosition = Target.transform.localPosition;
                    //go.localScale = Vector3.one;
                }
                Destroy(this.gameObject);
            }
        }
    }

    public float GetDistance(Vector3 From, Vector3 To)
    {
        return Mathf.Sqrt(Mathf.Pow(From.x - To.x, 2) + Mathf.Pow(From.y - To.y, 2));
    }
}
