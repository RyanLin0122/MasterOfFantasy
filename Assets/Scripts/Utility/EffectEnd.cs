using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEnd : MonoBehaviour
{
    
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void DestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }
    public void CloseObject()
    {
        gameObject.SetActive(false);
    }
}
