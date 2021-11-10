using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEntity : MonoBehaviour
{
    public bool IsHit = false;

    public void SetHit()
    {
        IsHit = true;
    }
    public void DestroyIfHit()
    {
        if (IsHit)
        {
            DestroySelf();
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
