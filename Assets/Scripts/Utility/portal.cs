using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class portal : MonoBehaviour
{
    [Header("傳點編號")]
    public int portalNum;
    public bool IsInPortal = false;
    public Action act = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Portal");
        foreach (var item in objs)
        {
            item.GetComponent<portal>().IsInPortal = false;
        }
        IsInPortal = true;   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IsInPortal = false;
    }
    private void Update()
    {
        if (IsInPortal)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MainCitySys.Instance.Transfer(portalNum, act);
                AudioSvc.Instance.PlayCharacterAudio(Constants.PortalAudio);
            }
        }
        
    }

    
}
