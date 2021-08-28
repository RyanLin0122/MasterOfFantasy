using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(transform.gameObject);
    }
}
