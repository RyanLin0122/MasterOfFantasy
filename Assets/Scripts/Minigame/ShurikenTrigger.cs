using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenTrigger : MonoBehaviour
{
    private int num = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "ShurikenChr")
        {
            GameObject.Find("ShurikenGameManager").GetComponent<ShurikenGameManager>().Trigger();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "bg")
        {
            if (num != 0)
            {
                Destroy(gameObject);
                return;
            }
            num++;
        }
    }
}
